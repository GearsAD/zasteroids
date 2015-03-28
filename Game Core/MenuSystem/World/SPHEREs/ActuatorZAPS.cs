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

namespace ZAsteroids.World.SPHEREs
{
    /// <summary>
    /// Create a new Particle System class that inherits from a
    /// Default DPSF Particle System
    /// </summary>
    public class ActuatorZAPS : AbstractBillboardZAPS
    {
        #region Public Properties
        #endregion

        #region Private Fields

        /// <summary>
        /// The source actuator.
        /// </summary>
        Vector3 _pointingDirection;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ActuatorZAPS(Vector3 pointingDirection)
        {
            this._pointingDirection = pointingDirection;
            this.ParticleBudgetTotal = 100;
            this.ParticleTexture = WorldContent.effectContent.cloudParticle;
        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndQuickFadeOut);
        }

        public override void SetupInitialParticleProperties()
        {
            InitialProperties.StartSizeMin = 0.1f; InitialProperties.StartSizeMax = 0.1f;
            InitialProperties.EndSizeMin = 2f;  InitialProperties.EndSizeMax = 2f; //Otherwise the size is ignored...
            Color col = Color.LightSteelBlue;
            col.A = 128;
            InitialProperties.StartColorMin = InitialProperties.StartColorMax = InitialProperties.EndColorMin = InitialProperties.EndColorMax = col;
            InitialProperties.VelocityIsAffectedByEmittersOrientation = true;
            InitialProperties.PositionIsAffectedByEmittersPosition = true;
            //Assuming that we're not going to get 
            InitialProperties.VelocityMin = 3.0f*Vector3.Normalize(_pointingDirection);
            InitialProperties.VelocityMax = 6.0f * Vector3.Normalize(_pointingDirection);
            InitialProperties.LifetimeMin = 0.5f;
            InitialProperties.LifetimeMax = 2.5f;
            InitialProperties.RotationalVelocityMax = 6;
            InitialProperties.RotationalVelocityMin = -6;
        }

        public override void SetupZAPSEmitterProperties()
        {
            //Make it additive blending.
            ParticleBlendingType = BlendState.Additive;
            Emitter.EmitParticlesAutomatically = true;
            Emitter.ParticlesPerSecond = 70;
        }

        public override void InitializeZAPSParticle(DefaultSprite3DBillboardParticle cParticle)
        {
            base.InitializeZAPSParticle(cParticle); 
        }
    }
}