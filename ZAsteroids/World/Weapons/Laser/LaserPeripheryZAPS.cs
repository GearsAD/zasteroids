#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ZitaAsteria;
using ZitaAsteria.World.Effects.DPSF;
using DPSF;
#endregion

namespace ZAsteroids.World.Weapons.Laser
{
    /// <summary>
    /// Create a new Particle System class that inherits from a
    /// Default DPSF Particle System
    /// </summary>
    public class LaserPeripheryZAPS : AbstractBillboardZAPS
    {
        #region Public Properties
        /// <summary>
        /// The flare color.
        /// </summary>
        public Color FlareColor { get; set; }

        public float LifeSpanMinS { get; set; }

        public float LifeSpanMaxS { get; set; }

        public float radius = 3;

        public float ParticleRate { get; set; }

        /// <summary>
        /// The initial flare particle size.
        /// </summary>
        public float FlareParticleInitialSize { get; set; }

        /// <summary>
        /// The initial flare particle end size.
        /// </summary>
        public float FlareParticleEndSize { get; set; }

        /// <summary>
        /// The pointing vector in the direction.
        /// </summary>
        public Quaternion PointingVector { get; set; }

        /// <summary>
        /// The phase shift of the periphery, between 0 and 2pi.
        /// </summary>
        float PhaseShift = 0;

        #endregion

        #region Private Fields
        /// <summary>
        /// The faster wave frequency.
        /// </summary>
        float _freqFasterWave = 0.02f;
        /// <summary>
        /// The slower wave frequency.
        /// </summary>
        float _freqSlowerWave = 0.02f;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        public LaserPeripheryZAPS(Color colour, float minLifeS, float maxLifeS, float initialSize, float radius, Quaternion pointingVector, float phaseShift)
        {
            this.FlareColor = colour;
            this.LifeSpanMinS = minLifeS;
            this.LifeSpanMaxS = maxLifeS;
            this.radius = radius;
            this.FlareParticleInitialSize = initialSize;
            this.FlareParticleEndSize = 0.01f;
            this.ParticleRate = 2000;
            this.ParticleBudgetTotal = 2000;
            this.ParticleTexture = WorldContent.effectContent.cloudParticle;
            PhaseShift = phaseShift;
            this.PointingVector = pointingVector;
        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp);
        }

        public override void SetupInitialParticleProperties()
        {
            InitialProperties.StartSizeMin = InitialProperties.StartSizeMax = FlareParticleInitialSize;
            InitialProperties.EndSizeMin = InitialProperties.EndSizeMax = FlareParticleInitialSize * 10.0f; //Otherwise the size is ignored...
            InitialProperties.StartColorMin = InitialProperties.StartColorMax = InitialProperties.EndColorMin = InitialProperties.EndColorMax = FlareColor;
            InitialProperties.AccelerationMin = InitialProperties.AccelerationMax = Vector3.Zero;
            InitialProperties.VelocityIsAffectedByEmittersOrientation = false;
            InitialProperties.PositionIsAffectedByEmittersPosition = false;
            InitialProperties.StartColorMin = InitialProperties.StartColorMax = InitialProperties.EndColorMin = InitialProperties.EndColorMax = FlareColor;
            InitialProperties.VelocityMin = Vector3.Zero;
            InitialProperties.VelocityMax = Vector3.Zero;
            InitialProperties.LifetimeMin = this.LifeSpanMinS;
            InitialProperties.LifetimeMax = this.LifeSpanMaxS;
            InitialProperties.RotationalVelocityMax = 6;
            InitialProperties.RotationalVelocityMin = -6;
        }

        public override void SetupZAPSEmitterProperties()
        {
            //Make it additive blending.
            ParticleBlendingType = BlendState.Additive;
            Emitter.EmitParticlesAutomatically = true;
            Emitter.ParticlesPerSecond = ParticleRate;
        }

        public override void InitializeZAPSParticle(DefaultSprite3DBillboardParticle cParticle)
        {
            base.InitializeZAPSParticle(cParticle);

            //Now set up the particles.
            Vector3 offset = Vector3.UnitZ * 5.0f * this.NumberOfActiveParticles +
                15.0f * Vector3.UnitY * (float)Math.Sin(2.0f * Math.PI * _freqFasterWave * this.NumberOfActiveParticles + PhaseShift) +
                15.0f * Vector3.UnitX * (float)Math.Cos(2.0f * Math.PI * _freqSlowerWave * this.NumberOfActiveParticles + PhaseShift); //Along the vector
            offset = Vector3.Transform(offset, PointingVector);

            cParticle.Position = this.Emitter.PositionData.Position + offset;
            cParticle.Color = FlareColor;
        }

    }
}