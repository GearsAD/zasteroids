using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria;

namespace ZitaAsteria.World.Effects.ExplosionsSmoke
{
    /// <summary>
    /// A shockwave-based shader.
    /// </summary>
    public class ShockWaveShader : ShaderEffect
    {
        #region Public Properties
        /// <summary>
        /// The Location of the shockwave in screen coordinates. Updated from shockWaveCompoundEffect.
        /// </summary>
        public Vector2 Location { get; set; }

        public float waveRadiusPx { get; set; }
        public float waveAmplitudePx { get; set; }
        public float waveWidthPx { get; set; }
        public float rateOfExpansionPx { get; set; }
        public float decelerationRatePx { get; set; }
        public float amplitudeDropS { get; set; }
        public float widthDilationS { get; set; }
        #endregion

        #region Private Fields
        float
            _initialRadius,
            _initialWidth,
            _initialExpansionRate,
            _initialWaveAmplitude;
        #endregion

        /// <summary>
        /// Create a new lighting class.
        /// </summary>
        public ShockWaveShader()
        {
            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.ShockwaveEffect.Clone();
            //Default initialization.
            InitializeWaveCharacteristics(10, 20, 50, 50, 450, 0, 10,
                WorldContainer.graphicsDevice.Viewport.Width,
                WorldContainer.graphicsDevice.Viewport.Height);

        }

        /// <summary>
        /// Setup the shockwave characteristics.
        /// </summary>
        /// <param name="shockRadius">The total radius of the wave</param>
        /// <param name="shockWidth">The width of the wave</param>
        /// <param name="shockAmplitude">The amplitude of the wave</param>
        public void InitializeWaveCharacteristics(float initialRadius, float initialWidth, float widthDilationS, float shockAmplitude, float rateOfExpansion, float decelerationRatePx, float dropOffRateS, int screenWidth, int screenHeight)
        {
            waveRadiusPx = initialRadius;
            waveWidthPx = initialWidth;
            this.widthDilationS = widthDilationS;
            this.decelerationRatePx = decelerationRatePx;
            rateOfExpansionPx = rateOfExpansion;
            amplitudeDropS = dropOffRateS;
            waveAmplitudePx = shockAmplitude;
            hlslEffect.Parameters["screenSize"].SetValue(new Vector2(screenWidth, screenHeight));

            _initialExpansionRate = rateOfExpansion;
            _initialRadius = initialRadius;
            _initialWidth = initialWidth;
            _initialWaveAmplitude = shockAmplitude;
        }

        /// <summary>
        /// Reset the effect.
        /// </summary>
        public void Reset()
        {
            waveRadiusPx = _initialRadius;
            waveWidthPx = _initialWidth;
            rateOfExpansionPx = _initialExpansionRate;
            waveAmplitudePx = _initialWaveAmplitude;
        }

        /// <summary>
        /// Draw the world (in texture form) onto another texture with the lighting.
        /// </summary>
        /// <param name="originalScene"></param>
        /// <returns></returns>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsEnabled) //Only update if enabled.
            {
                float gameTimeElapsedS = (float)gameTime.ElapsedGameTime.TotalSeconds;
                waveRadiusPx += gameTimeElapsedS * rateOfExpansionPx;
                rateOfExpansionPx -= gameTimeElapsedS * decelerationRatePx;
                waveAmplitudePx -= gameTimeElapsedS * amplitudeDropS;
                waveWidthPx += gameTimeElapsedS * widthDilationS;
            }
            //Update the parameters as well.
            hlslEffect.Parameters["shockCentre"].SetValue(this.Location);
            hlslEffect.Parameters["shockRadius"].SetValue(waveRadiusPx);
            hlslEffect.Parameters["shockWidth"].SetValue(waveWidthPx);
            hlslEffect.Parameters["shockAmplitude"].SetValue(waveAmplitudePx);
        }

    }
}
