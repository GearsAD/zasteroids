using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws scanlines over the screen.
    /// </summary>
    public class EclipseEffectShader : ShaderEffect
    {

        public EclipseEffectShader()
        {        }

        /// <summary>
        /// Initialize the Thermal effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.eclipseEffect;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
