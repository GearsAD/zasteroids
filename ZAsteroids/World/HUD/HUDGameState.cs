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
using ZitaAsteria.MenuSystem;

namespace ZAsteroids.World.HUD
{
    class HUDGameState : HUDComponent
    {
        public SpriteFont Font { get; set; }

        private Vector2 safePositionBottomCenter;

        //private string state;

        private bool enabled;

        public HUDGameState()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            Font = WorldContent.fontAL28pt;

            safePositionBottomCenter = new Vector2(HUDDrawSafeArea.Center.X, HUDDrawSafeArea.Bottom);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                string output = "";

                switch (HUDProperties.GameState)
                {
                    case (GameStateEnum.AsteroidsMode): output = "...Asteroid Barrage Incoming - " ; break;
                    case (GameStateEnum.SphereMode): output = "...SPHERE Upgrade+Repair Mode - " ; break;
                }
                ///Use 00:00 to standarize the string length.
                safePositionBottomCenter = new Vector2(HUDDrawSafeArea.Center.X - (Font.MeasureString(output + "00:00 remaining...").X / 2), HUDDrawSafeArea.Bottom - 30);
                //Now replace it.
                output += HUDProperties.CurrendRoundTimeLeftS.ToString("00.00") + " remaining...";

                HUDSpriteBatch.DrawString(WorldContent.fontAL28pt, output, safePositionBottomCenter, WorldContent.hudContent.hudTextColor);

                HUDSpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets whether the component should be drawn.
        /// </summary>
        /// <param name="enabled">Enable the component</param>
        public void Enable(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}
