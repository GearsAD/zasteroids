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
    public class LaserZAPS : AbstractBillboardZAPS
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
        /// The flare particle acceleration.
        /// </summary>
        public Vector3 FlareAcceleration { get; set; }

        /// <summary>
        /// If true then the flare particle's disperse.
        /// </summary>
        public bool FlareParticleDisperse { get; set; }

        /// <summary>
        /// The pointing vector in the direction.
        /// </summary>
        public Quaternion PointingVector { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LaserZAPS(Color colour, float minLifeS, float maxLifeS, float initialSize, float radius, float particleRateS)
        {
            this.FlareColor = colour;
            this.LifeSpanMinS = minLifeS;
            this.LifeSpanMaxS = maxLifeS;
            this.radius = radius;
            this.FlareParticleInitialSize = initialSize;
            this.FlareParticleEndSize = 0.01f;
            this.ParticleRate = particleRateS;
            this.ParticleBudgetTotal = (int)Math.Ceiling(particleRateS * LifeSpanMaxS);
            this.FlareParticleDisperse = true;
            this.FlareAcceleration = new Vector3(0, 0.0f, 0);
            this.ParticleTexture = WorldContent.effectContent.cloudParticle;
        }

        /// <summary>
        /// Initialize this easy-like.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="minLifeS"></param>
        /// <param name="maxLifeS"></param>
        /// <param name="initialSize"></param>
        public LaserZAPS(Color colour, float minLifeS, float maxLifeS, float initialSize, float endSize, float radius, float pRate, bool particlesDisperse, Vector3 particleAccleration)
        {
            this.FlareColor = colour;
            this.LifeSpanMinS = minLifeS;
            this.LifeSpanMaxS = maxLifeS;
            this.radius = radius;
            this.FlareParticleInitialSize = initialSize;
            this.FlareParticleEndSize = endSize;
            this.ParticleRate = pRate;
            this.FlareAcceleration = particleAccleration;
            this.FlareParticleDisperse = particlesDisperse;
            this.ParticleBudgetTotal = (int)Math.Ceiling(pRate * LifeSpanMaxS);
            this.ParticleTexture = WorldContent.effectContent.cloudParticle;
        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
        }

        public override void SetupInitialParticleProperties()
        {
            InitialProperties.StartSizeMin = InitialProperties.StartSizeMax = FlareParticleInitialSize;
            InitialProperties.EndSizeMin = InitialProperties.EndSizeMax; //Otherwise the size is ignored...
            InitialProperties.StartColorMin = InitialProperties.StartColorMax = InitialProperties.EndColorMin = InitialProperties.EndColorMax = FlareColor;
            InitialProperties.AccelerationMin = InitialProperties.AccelerationMax = FlareAcceleration;
            InitialProperties.VelocityIsAffectedByEmittersOrientation = false;
            InitialProperties.PositionIsAffectedByEmittersPosition = false;
            //InitialProperties.VelocityMin = 0;
            //InitialProperties.VelocityMax = 0;
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
            Vector3 offset = Vector3.UnitZ * 5.0f * this.NumberOfActiveParticles;
            offset = Vector3.Transform(offset, PointingVector);

            cParticle.Position = this.Emitter.PositionData.Position + offset;
            cParticle.Color = FlareColor;
        }
    }
}