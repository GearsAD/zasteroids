using System;
using System.Collections.Generic;

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
    public abstract class HUDComponent
    {
        protected SpriteBatch HUDSpriteBatch;

        protected Rectangle HUDDrawSafeArea;

        /// <summary>
        /// Default constructor
        /// </summary>
        public HUDComponent()
        {
        }

        /// <summary>
        /// Initialize the HUD compnenet
        /// </summary>
        public virtual void Initialize()
        {
            HUDSpriteBatch = WorldContainer.spriteBatch;

            HUDDrawSafeArea = GetDrawSafeArea(0.9f);
        }

        /// <summary>
        /// Update the HUD component
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw the HUD component
        /// </summary>
        public virtual void Draw()
        {
        }

        protected Rectangle GetDrawSafeArea(float percent)
        {
            Rectangle retval = new Rectangle(WorldContainer.graphicsDevice.Viewport.X,
                WorldContainer.graphicsDevice.Viewport.Y,
                WorldContainer.graphicsDevice.Viewport.Width,
                WorldContainer.graphicsDevice.Viewport.Height);

            float border = (1 - percent) / 2;

            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);

            return retval;
        }
    }
}
