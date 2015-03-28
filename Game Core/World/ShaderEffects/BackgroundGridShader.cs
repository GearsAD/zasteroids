using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws a surreal background grid over the screen - used for the scene background.
    /// </summary>
    public class BackgroundGridShader : ShaderEffect
    {
        /// <summary>
        /// The width of the grid in pixels.
        /// </summary>
        public int gridWidthPixels { get; set; }
        /// <summary>
        /// The height of the grid in pixels.
        /// </summary>
        public int gridHeightPixels { get; set; }
        /// <summary>
        /// The height of the grid in pixels - max height.
        /// </summary>
        public float gridZHeightPixels { get; set; }

        /// <summary>
        /// A timer for keeping track of the ms that have passed.
        /// </summary>
        float timer = 0;

        public BackgroundGridShader()
        { 
            //Set the defaults.
            gridWidthPixels = 25;
            gridHeightPixels = 25;
            gridZHeightPixels = 10;

        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.backgroundGridEffect;

            //Set the width and height.
            this.hlslEffect.Parameters["screenWidth"].SetValue(WorldContainer.graphicsDevice.Viewport.Width);
            this.hlslEffect.Parameters["screenHeight"].SetValue(WorldContainer.graphicsDevice.Viewport.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.Milliseconds;

            this.hlslEffect.Parameters["gridWidth"].SetValue(gridWidthPixels);
            this.hlslEffect.Parameters["gridHeight"].SetValue(gridHeightPixels);
            this.hlslEffect.Parameters["gridZHeightPixels"].SetValue(gridZHeightPixels);   
            this.hlslEffect.Parameters["timerMS"].SetValue(timer);

        }
    }
}
