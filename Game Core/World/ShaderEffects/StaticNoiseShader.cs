using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws  over the screen.
    /// </summary>
    public class StaticNoiseShader : ShaderEffect
    {
        float m_Timer = 0;
        float noiseLevel = 1.0f; // Default : 0.01f
        double noiseLevelCycle = 1.0 * Math.PI;
        RandomNumbers RandomNumber; 
        
        public StaticNoiseShader()
        { 
            RandomNumber = new RandomNumbers();
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.staticNoiseEffect;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            m_Timer += (float)gameTime.ElapsedGameTime.Milliseconds / 500;
            
            this.hlslEffect.Parameters["fTimer"].SetValue(m_Timer);
            this.hlslEffect.Parameters["iSeed"].SetValue(1337);
            this.hlslEffect.Parameters["fNoiseAmount"].SetValue(noiseLevel);   // Default : 0.01f
            
            noiseLevelCycle -= (2.0 * Math.PI)* 0.001;
            if (noiseLevelCycle < 0.0f)
            {
                noiseLevelCycle = 0.0f;
            }

            noiseLevel = (float)Math.Sin(noiseLevelCycle);    // 1.0 * Math.PI

        }
    }
}
