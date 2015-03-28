using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws NightVision over the screen.
    /// </summary>
    public class NightVisionShader : ShaderEffect
    {
        float lightMagnificationFactor = 0.0f;
        float maxLightMagnificationFactor = 0.0f;
        
        public NightVisionShader()
        {        }

        /// <summary>
        /// Initialize the NightVision effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            lightMagnificationFactor = 0.0f;
            maxLightMagnificationFactor = 15.5f;  

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.nightVisionEffect;
            this.hlslEffect.Parameters["lightMagnificationFactor"].SetValue(lightMagnificationFactor);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (lightMagnificationFactor < maxLightMagnificationFactor)
            {
                lightMagnificationFactor += 0.1f;
            }

            this.hlslEffect.Parameters["lightMagnificationFactor"].SetValue(lightMagnificationFactor);
        }
    }
}
