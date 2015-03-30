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
    public class HUDIntroScreen : HUDComponent
    {
        public Texture2D LogoTexture { get; set; }

        private RelativeTexture LogoInfo;

        public SpriteFont Font { get; set; }

        public HUDIntroScreen()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            LogoTexture = WorldContent.hudContent.logo;

            // Ok, no idea why you played with the origin or why you resized the texture instead of using code, but its working so im not going to fix it.
            // Just a heads up, if you mess with the resolution it wont always draw in the same bottom left area of the screen.
            // ie. 60 pixels at a high resolution is smaller than 60 pixels at a low resolution, hence HUDDrawSafeArea works on %...
            // P.S. Now you know how i feel when i have to use your code. :P lol
            // [Alucard]
            LogoInfo = new RelativeTexture(LogoTexture);
            LogoInfo.Origin = new Vector2(0, LogoTexture.Height );
            LogoInfo.EnableDraw = true;

            // LogoInfo.Scale to scale texture
            // LogoInfo.Position to position texture based off its origin
            // Origin of texture calculated internally, only use LogoInfo.Origin to override in special cases
            // Only draw within HUDDrawSafeArea to avoid texture being draw off screen.
            // I had a good laugh [Alucard] :) 
            
            LogoInfo.Position = new Vector2(HUDDrawSafeArea.Left - 60, HUDDrawSafeArea.Bottom + 30);

            Font = WorldContent.fontAL28pt;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            HUDSpriteBatch.Begin();

            LogoInfo.Draw(HUDSpriteBatch);

            //Draw the string to the right.
            string text = "Press Space or Gamepad 'Start' to begin...";
            HUDSpriteBatch.DrawString(
                Font,
                text,
                new Vector2(HUDDrawSafeArea.Right - Font.MeasureString(text).X, HUDDrawSafeArea.Bottom - 30), 
                WorldContent.hudContent.hudTextColor);

            //Draw the string to the right.
            text = "[F1 or Gamepad 'A' for controls]";
            HUDSpriteBatch.DrawString(
                WorldContent.fontAL18pt,
                text,
                new Vector2(HUDDrawSafeArea.Right - WorldContent.fontAL18pt.MeasureString(text).X, HUDDrawSafeArea.Top + 10),
                WorldContent.hudContent.hudTextColor);

            HUDSpriteBatch.End();
        }
    }
}
