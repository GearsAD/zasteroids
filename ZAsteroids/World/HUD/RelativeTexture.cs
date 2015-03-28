using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZAsteroids.World.HUD
{
    public class RelativeTexture
    {
        public Texture2D Texture { get; private set; }

        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }

        public float Angle { get; set; }
        public float Scale { get; set; }
        public float RotationDirection { get; set; }
        public float PulseSpeed { get; set; }

        public bool RotateEffect { get; set; }
        public bool PulseEffect { get; set; }
        public bool EnableDraw { get; set; }

        public Dictionary<string, RelativeTexture> Children { get; private set; }

        private float mAlphaValue = 1;
        private int fadevalue = 1;
        private Color FadeColor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelativeTexture(Texture2D texture)
        {
            Texture = texture;
            FadeColor = new Color(mAlphaValue, mAlphaValue, mAlphaValue, mAlphaValue);
            Scale = 1.0f;
            Angle = 0.0f;
            Origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            Children = new Dictionary<string, RelativeTexture>();
        }

        public void Update(GameTime gameTime)
        {
            if (RotateEffect)
            {
                if ((Angle >= 6.283185307179585f) || (Angle <= -6.283185307179585f))
                {
                    Angle = 0.0f;
                }
                Angle += MathHelper.ToRadians(90 * (float)gameTime.ElapsedGameTime.TotalSeconds) * RotationDirection;
            }
            if (PulseEffect)
            {
                if (mAlphaValue >= 1)
                {
                    fadevalue = -1;
                }
                if (mAlphaValue <= 0)
                {
                    fadevalue = 1;
                }

                mAlphaValue += (0.03f * fadevalue); // Coz setting the Alpha of a color does 50% of the job? Thanks Microsoft! Fuckers!
            }
            foreach (KeyValuePair<string, RelativeTexture> Child in Children)
            {
                Child.Value.Update(gameTime);
            }
            //Children.ForEach(c => c.Update(gameTime));
        }

        private void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            
            Matrix globalTransform = CalculateLocalTransform() * parentTransform;
            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);

            if (EnableDraw)
            {
                spriteBatch.Draw(Texture, position, null, Color.White * mAlphaValue, rotation, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }

            foreach (KeyValuePair<string, RelativeTexture> Child in Children)
            {
                Child.Value.Draw(spriteBatch, globalTransform);
            }
            //Children.ForEach(c => c.Draw(spriteBatch, globalTransform));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Matrix.Identity);
        }

        private Matrix CalculateLocalTransform()
        {
            return Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f) * Matrix.CreateScale(Scale, Scale, 1f) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }

        private static void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }
    }
}
