#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ZitaAsteria;
using DPSF;
using ZitaAsteria.World.Effects.DPSF;
#endregion

namespace ZitaAsteria.World.Effects.ExplosionSmoke.IncendiaryExplosionWithTrails
{
    /// <summary>
    /// The smoking bits of stuff that are thrown around the map.
    /// [Gears]
    /// </summary>
    class DebrisTrailZAPS : AbstractBillboardZAPS
    {
        #region Public Properties
        /// <summary>
        /// The colour of the particle trail.
        /// </summary>
        public Color TrailColour { get; set; }
        /// <summary>
        /// The upward velocity of the trail.
        /// </summary>
        public float ParticleRiseRateS { get; set; }
        /// <summary>
        /// The life span of the trail.
        /// </summary>
        public float LifeSpanS { get; set; }

        /// <summary>
        /// The number of particles per second.
        /// </summary>
        public int ParticlesPerSecond { get; set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The direction and rate of the emitter's change in position. Acceleration is only gravity, and drag is not considered - simplified this considerably.
        /// </summary>
        private Vector3 _emitterVelocity = Vector3.Zero;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DebrisTrailZAPS(Color trailColour)
        {
            ParticleRiseRateS = 3.0f;
            ParticlesPerSecond = 15;
            LifeSpanS = 2;
            ParticleBudgetTotal = (int)Math.Ceiling(LifeSpanS * ParticlesPerSecond);
            ParticleTexture = WorldContent.effectContent.fireParticle;
            TrailColour = trailColour;
            //Important - unbind from parent emitter!
            this.IsBoundToParentACELocation = false;
        }

        public override void SetupZAPSEmitterProperties()
        {
            //Set up the emitter
            Emitter.ParticlesPerSecond = ParticlesPerSecond;
            Emitter.EmitParticlesAutomatically = true;
            this.ParticleBlendingType = BlendState.Additive;
        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
        }

        public override void  SetupInitialParticleProperties()
        {
            InitialProperties.LifetimeMin = InitialProperties.LifetimeMax = LifeSpanS;
            InitialProperties.StartSizeMin = 3.5f;
            InitialProperties.StartSizeMax = 4.5f;
            InitialProperties.EndSizeMin = InitialProperties.EndSizeMax = 0.1f;
            InitialProperties.StartColorMin = InitialProperties.StartColorMax = TrailColour;
            InitialProperties.EndColorMin = InitialProperties.EndColorMax = Color.Black;
            InitialProperties.RotationalVelocityMin = -1;
            InitialProperties.RotationalVelocityMax = 1;
            InitialProperties.VelocityMin = InitialProperties.VelocityMax = new Vector3(0, ParticleRiseRateS, 0);
        }

        /// <summary>
        /// Reset the position and velocity of the ZAPS to the parent location
        /// </summary>
        public void ResetPositionVelocity(Vector3 position)
        {
            this.Location = position;
            //Reinitialize the velocities
            _emitterVelocity = 9.8f * new Vector3( //4 Gravities max!
                    2.0f * (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 1.0f, 
                   (float)ZAMathTools.uniformRandomGenerator.NextDouble() * 3.0f,  
                    2.0f * (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 1.0f);
        }

        protected override void BeforeUpdate(float fElapsedTimeInSeconds)
        {
            base.BeforeUpdate(fElapsedTimeInSeconds);

            //Update the physics.
            this.Location += fElapsedTimeInSeconds * _emitterVelocity;
            _emitterVelocity.Y -= fElapsedTimeInSeconds * 9.8f; //Gravity!

            //If the emitter must be higher than the ground - bounce!
#if HIGH_QUALITY_PARTICLES
            float groundHeight = 0;
 
            if (Location.Y < groundHeight)
            {
                this.Location = new Vector3(Location.X, groundHeight, Location.Z);
                //And make it bounce...
                if (_emitterVelocity.Y < 0)
                    _emitterVelocity.Y *= -1; //Bam!
            }
#endif
        }
    }
}