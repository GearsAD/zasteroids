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
    public class HUDControls : HUDComponent
    {
        public Texture2D ControlsTexture { get; set; }

        private RelativeTexture ControlsInfo;

        public HUDControls()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            ControlsTexture = WorldContent.hudContent.help;

            ControlsInfo = new RelativeTexture(ControlsTexture);
            ControlsInfo.EnableDraw = true;

            ControlsInfo.Position = new Vector2(HUDDrawSafeArea.Center.X, HUDDrawSafeArea.Center.Y);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            if (HUDProperties.IsHelpShown)
            {
                HUDSpriteBatch.Begin();

                ControlsInfo.Draw(HUDSpriteBatch);

                HUDSpriteBatch.End();
            }
        }

    }
}
