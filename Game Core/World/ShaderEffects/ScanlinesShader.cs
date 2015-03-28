﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws scanlines over the screen.
    /// </summary>
    public class ScanlinesShader : ShaderEffect
    {
        public ScanlinesShader()
        {
        }

        /// <summary>
        /// Initialize the scanline effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.scanlineEffect;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Set the timer on the effect.
            this.hlslEffect.Parameters["Timer"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// Set the scanline intensity.
        /// </summary>
        /// <param name="intensity"></param>
        public void SetScanlineIntensity(float intensity)
        {
            this.hlslEffect.Parameters["Intensity"].SetValue(intensity);
        }

    }
}
