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
    public class HUDScore : HUDComponent
    {
        public SpriteFont Font { get; set; }

        private Vector2 safePositionTopCenter;

        private float _scoreCounter;

        private string _text;

        private bool enabled;

        public HUDScore()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            _scoreCounter = 0;

            Font = WorldContent.fontAL28pt;

            _text = "Score : ";

            safePositionTopCenter = new Vector2(HUDDrawSafeArea.Center.X - (Font.MeasureString(_text).X / 2), HUDDrawSafeArea.Top);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            _scoreCounter += (HUDProperties.Score - _scoreCounter) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                //HUDSpriteBatch.DrawString(WorldContent.fontAL28pt, MenuContainer.ScoreZAst.ToString(), safePositionTopCenter, WorldContent.hudContent.hudTextColor);
                HUDSpriteBatch.DrawString(Font, _text + ((int)Math.Ceiling(_scoreCounter)).ToString(), safePositionTopCenter, WorldContent.hudContent.hudTextColor);

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
