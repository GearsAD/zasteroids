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
    public class PitchBlackShader : ShaderEffect
    {
        float darknessCoefficient = 0.0f;
        float maxDarknessCoeff    = 0.9f;
        public PitchBlackShader()
        {
        }

        /// <summary>
        /// Initialize the scanline effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.pitchBlackEffect;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if(darknessCoefficient < maxDarknessCoeff)
            {
                darknessCoefficient += 0.0025f;
            }
            
            this.hlslEffect.Parameters["darknessCoefficient"].SetValue(darknessCoefficient);
        }
    }
}
