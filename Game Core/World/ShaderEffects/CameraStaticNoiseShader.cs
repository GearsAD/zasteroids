using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using Microsoft.Xna.Framework.Graphics;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws  over the screen.
    /// </summary>
    public class CameraStaticNoiseShader : ShaderEffect
    {
        float m_Timer = 0;
        public float noiseLevel { get; set; }
        double noiseLevelCycle = 1.0 * Math.PI;
        RandomNumbers RandomNumber;

        Texture2D cameraNoiseTexture = null;

        public CameraStaticNoiseShader()
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
            this.hlslEffect = WorldContent.effectContent.cameraStaticNoiseEffect;
            this.cameraNoiseTexture = WorldContent.effectContent.cameraStaticNoiseTexture;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            m_Timer += (float)gameTime.ElapsedGameTime.Milliseconds / 500;
            
            this.hlslEffect.Parameters["fTimer"].SetValue(m_Timer);
            this.hlslEffect.Parameters["iSeed"].SetValue(1337);
            this.hlslEffect.Parameters["fNoiseAmount"].SetValue(noiseLevel);   // Default : 0.01f
            this.hlslEffect.Parameters["NoiseTexture"].SetValue(cameraNoiseTexture);
            
            //noiseLevelCycle -= (2.0 * Math.PI)* 0.001;
            //if (noiseLevelCycle < 0.0f)
            //{
            //    noiseLevelCycle = 0.0f;
            //}

            //noiseLevel = (float)Math.Sin(noiseLevelCycle);    // 1.0 * Math.PI

        }
    }
}
