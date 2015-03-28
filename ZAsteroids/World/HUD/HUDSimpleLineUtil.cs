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
    /// <summary>
    /// A utility class for drawing a simple line.
    /// </summary>
    class HUDSimpleLineUtil 
    {
        private Texture2D blank = new Texture2D(WorldContainer.graphicsDevice, 1, 1, false, SurfaceFormat.Color);

        private SpriteBatch _hudSpriteBatch = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public HUDSimpleLineUtil()
        {
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            // Must initialize base to get safe draw area
            _hudSpriteBatch = new SpriteBatch(MenuContainer.GraphicsDevice);

            blank.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Draw
        /// </summary>
        public void DrawLine(Color col, float width, Vector2 from, Vector2 to)
        {
            _hudSpriteBatch.Begin();
            DrawLine(_hudSpriteBatch, blank, width, col, from, to);
            _hudSpriteBatch.End();
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="blank"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        private void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}
