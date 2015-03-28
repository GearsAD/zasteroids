using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws Fadeinout with colour over the screen.
    /// </summary>
    public class FadeInOutShader : ShaderEffect
    {
        /// <summary>
        /// If true then increases red.
        /// </summary>
        public bool IsIncreasingRed { get; set; }
        /// <summary>
        /// If true then increases red.
        /// </summary>
        public bool IsIncreasingGreen { get; set; }
        /// <summary>
        /// If true then increases red.
        /// </summary>
        public bool IsIncreasingBlue { get; set; }

        /// <summary>
        /// In the form of RGB.
        /// </summary>
        public Vector3 CurrentFactors { get; set; }

        /// <summary>
        /// In the form of RGB/s.
        /// </summary>
        public Vector3 DeltaFactorPerS { get; set; }

        public FadeInOutShader()
        {
            CurrentFactors = Vector3.Zero;
            DeltaFactorPerS = 0.3f * Vector3.One;
            IsIncreasingBlue = IsIncreasingGreen = IsIncreasingRed = true;
        }

        /// <summary>
        /// Initialize the FadeInOutShader effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize(); 

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.fadeInOutEffect;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Increase Flags
            this.hlslEffect.Parameters["increaseRed"].SetValue(IsIncreasingRed);
            this.hlslEffect.Parameters["increaseGreen"].SetValue(IsIncreasingGreen);
            this.hlslEffect.Parameters["increaseBlue"].SetValue(IsIncreasingBlue);

            // Colour Increase Factors
            this.hlslEffect.Parameters["redIncreaseFactor"].SetValue(CurrentFactors.X);
            this.hlslEffect.Parameters["greenIncreaseFactor"].SetValue(CurrentFactors.Y);
            this.hlslEffect.Parameters["blueIncreaseFactor"].SetValue(CurrentFactors.Z);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentFactors += DeltaFactorPerS * deltaTime;
        }
    }
}
