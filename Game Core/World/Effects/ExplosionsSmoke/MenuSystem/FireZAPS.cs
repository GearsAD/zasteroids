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

namespace ZAsteroids.World.Effects.ExplosionSmoke
{
    /// <summary>
    /// A simple, persistent, fire effect.
    /// </summary>
    class FireZAPS : AbstractBillboardZAPS
    {
        #region Public Fields
        /// <summary>
        /// Used by the DebrisExplosion to grow the flame on startup.
        /// </summary>
        public float FireRingRadius { get; set; }

        /// <summary>
        /// If true then using the fire ring effect, otherwise ignore! NOTE - flame thrower effect ignores this so that it inherits the emitter's orientation [Gears].
        /// </summary>
        public bool IsUsingFireRingEffect { get; set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The fire colour.
        /// </summary>
        Color _fireColour = Color.Yellow;

        Vector3 _velMin;
        Vector3 _velMax;

        int _particlesPerSecond;
        float _maxLifeTimeS;

        Vector3 _startLocation;
        Vector3 _velocity;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public FireZAPS(Color fireColour, Vector3 startLocation, Vector3 velocity, int particlePerSecond, float maxLifeTimeS)
        {
            _startLocation = startLocation;
            _velocity = velocity;
            _fireColour = fireColour;

            _velMin = new Vector3(-100, -100, -100);
            _velMax = new Vector3(100, 100, 100);
            _particlesPerSecond = particlePerSecond;
            _maxLifeTimeS = maxLifeTimeS;

            this.ParticleBudgetTotal = (int)Math.Ceiling(_particlesPerSecond * _maxLifeTimeS);
            this.ParticleTexture = WorldContent.effectContent.fireParticle;
        }

        public override void SetupZAPSEmitterProperties()
        {
            //Set up the emitter
            Emitter.ParticlesPerSecond = _particlesPerSecond;
            Emitter.EmitParticlesAutomatically = true;
            Emitter.PositionData.Position = _startLocation; //LOCATION3D, otherwise bust...
            Emitter.PositionData.Velocity = _velocity;
            this.ParticleBlendingType = BlendState.Additive;
        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndSlowFadeOut);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
        }

        public override void SetupInitialParticleProperties()
        {
            // Set the Fire Ring Settings
            InitialProperties.LifetimeMin = 0.1f;
            InitialProperties.LifetimeMax = _maxLifeTimeS;
            InitialProperties.StartSizeMin = 100.0f;
            InitialProperties.StartSizeMax = 200.0f;
            InitialProperties.EndSizeMin = 1.0f;
            InitialProperties.EndSizeMax = 50.0f;
            InitialProperties.StartColorMin = _fireColour;
            InitialProperties.StartColorMax = _fireColour;
            InitialProperties.EndColorMin = _fireColour;
            InitialProperties.EndColorMax = _fireColour;
            InitialProperties.InterpolateBetweenMinAndMaxColors = false;
            InitialProperties.VelocityMin = _velMin;
            InitialProperties.VelocityMax = _velMax;
            InitialProperties.VelocityIsAffectedByEmittersOrientation = true;
            InitialProperties.AccelerationMin = Vector3.Zero;
            InitialProperties.AccelerationMax = Vector3.Zero;
            InitialProperties.RotationalVelocityMin = -_velMax.Y / 8.0f;
            InitialProperties.RotationalVelocityMax = _velMax.Y / 8.0f;
        }

        public override void InitializeZAPSParticle(DefaultSprite3DBillboardParticle cParticle)
        {
            base.InitializeZAPSParticle(cParticle);
            if (IsUsingFireRingEffect)
            {
                Quaternion cBackup = Emitter.OrientationData.Orientation;
                Emitter.OrientationData.Orientation = Quaternion.Identity;
                InitializeParticleUsingInitialProperties(cParticle);
                Emitter.OrientationData.Orientation = cBackup;

                cParticle.Position = DPSFHelper.PointOnSphere(RandomNumber.Between(0, MathHelper.TwoPi), 0, FireRingRadius);
                cParticle.Position = Vector3.Transform(cParticle.Position, Emitter.OrientationData.Orientation);
                cParticle.Position += Emitter.PositionData.Position;
            }
        }
    }
}