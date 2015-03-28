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
    public class HUDDamage : HUDComponent
    {
        private Texture2D DamageTexture01 { get; set; }
        private Texture2D DamageTexture02 { get; set; }
        private Texture2D DamageTexture03 { get; set; }

        private Vector2 DamageTexture01Center;
        private Vector2 DamageTexture02Center;
        private Vector2 DamageTexture03Center;

        private Vector2 DamageTexture01Position;
        private Vector2 DamageTexture02Position;
        private Vector2 DamageTexture03Position;

        private bool enabled;

        public HUDDamage()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            DamageTexture01 = WorldContent.hudContent.damageEffect01;
            DamageTexture02 = WorldContent.hudContent.damageEffect02;
            DamageTexture03 = WorldContent.hudContent.damageEffect03;

            DamageTexture01Center = new Vector2(DamageTexture01.Width / 2, DamageTexture01.Height / 2);
            DamageTexture02Center = new Vector2(DamageTexture02.Width / 2, DamageTexture02.Height / 2);
            DamageTexture03Center = new Vector2(DamageTexture03.Width / 2, DamageTexture03.Height / 2);

            DamageTexture01Position = new Vector2(HUDDrawSafeArea.Right * (float)ZAMathTools.uniformRandomGenerator.NextDouble(), HUDDrawSafeArea.Bottom * (float)ZAMathTools.uniformRandomGenerator.NextDouble());
            DamageTexture02Position = new Vector2(HUDDrawSafeArea.Right * (float)ZAMathTools.uniformRandomGenerator.NextDouble(), HUDDrawSafeArea.Bottom * (float)ZAMathTools.uniformRandomGenerator.NextDouble());
            DamageTexture03Position = new Vector2(HUDDrawSafeArea.Right * (float)ZAMathTools.uniformRandomGenerator.NextDouble(), HUDDrawSafeArea.Bottom * (float)ZAMathTools.uniformRandomGenerator.NextDouble());
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                if (HUDProperties.HealthAmount <= 75)
                {
                    HUDSpriteBatch.Draw(DamageTexture01, DamageTexture01Position, null, Color.White, 0.0f, DamageTexture01Center, 0.8f, SpriteEffects.None, 0.0f);
                }
                if (HUDProperties.HealthAmount <= 50)
                {
                    HUDSpriteBatch.Draw(DamageTexture02, DamageTexture02Position, null, Color.White, 0.0f, DamageTexture02Center, 0.8f, SpriteEffects.None, 0.0f);
                }
                if (HUDProperties.HealthAmount <= 35)
                {
                    HUDSpriteBatch.Draw(DamageTexture03, DamageTexture03Position, null, Color.White, 0.0f, DamageTexture03Center, 0.8f, SpriteEffects.None, 0.0f);
                }

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
