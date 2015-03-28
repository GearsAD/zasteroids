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

using ZitaAsteria;
using ZitaAsteria.World;

namespace ZAsteroids.World.HUD
{
    public class HUDCrosshair : HUDComponent
    {
        public Texture2D CrosshairTexture { get; set; }
        public Texture2D FireHintTexture { get; set; }

        private RelativeTexture CrosshairInfo;
        private RelativeTexture WorkingValue;

        //private Vector2 safePositionCenter;
        //private Vector2 crosshairCenter;

        private bool enabled;

        public HUDCrosshair()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            CrosshairTexture = WorldContent.hudContent.crosshair;
            FireHintTexture = WorldContent.hudContent.fire;

            CrosshairInfo = new RelativeTexture(CrosshairTexture);
            CrosshairInfo.Children.Add("FireHint", new RelativeTexture(FireHintTexture) { Position = new Vector2(206, 51), EnableDraw = false });

            CrosshairInfo.EnableDraw = true;

            CrosshairInfo.Position = new Vector2(HUDDrawSafeArea.Center.X, HUDDrawSafeArea.Center.Y);

            //safePositionCenter = new Vector2(HUDDrawSafeArea.Center.X, HUDDrawSafeArea.Center.Y);

            //crosshairCenter = new Vector2(CrosshairTexture.Width / 2, CrosshairTexture.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            CrosshairInfo.Children.TryGetValue("FireHint", out WorkingValue);
            WorkingValue.EnableDraw = HUDProperties.FireHintStatis;
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                CrosshairInfo.Draw(HUDSpriteBatch);

                //HUDSpriteBatch.Draw(CrosshairTexture, safePositionCenter, null, Color.White, 0.0f, crosshairCenter, 1.0f, SpriteEffects.None, 0.0f);

                HUDSpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets whether the component should be drawn.
        /// </summary>
        /// <param name="enabled">enable the component</param>
        public void Enable(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}
