using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects.DPSF;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DPSF;
using ZitaAsteria.Scene;

namespace ZitaAsteria.MenuSystem.World.Satellite
{
    /// <summary>
    /// The satellite targeting reticle.
    /// </summary>
    public class SatelliteTargetingZAPS : AbstractQuadZAPS
    {
        #region Public Properties
        #endregion

        #region Private Fields
        Satellite _satellite = null;
        #endregion

       /// <summary>
        /// Constructor
        /// </summary>
        public SatelliteTargetingZAPS(Satellite satellite, Texture2D texture)
        {
            _satellite = satellite;

            this.ParticleBudgetTotal = 10;
            this.ParticleTexture = texture;
        }

        public override void SetupZAPSEmitterProperties()
        {
            //Set up the emitter
            Emitter.ParticlesPerSecond = 1000;
            Emitter.EmitParticlesAutomatically = true;
            RenderProperties.RasterizerState.CullMode = CullMode.None;

            Emitter.PositionData.Position = _satellite.Location; //LOCATION3D, otherwise bust...
            this.ParticleBlendingType = BlendState.Additive;

        }

        public override void SetupInitialSystemEvents()
        {
            ParticleEvents.AddEveryTimeEvent(this.UpdateParticleToFaceTheCamera);
        }

        public override void SetupInitialParticleProperties()
        {
            // Set the Fire Ring Settings
            InitialProperties.LifetimeMin = float.PositiveInfinity;
            InitialProperties.LifetimeMax = float.PositiveInfinity;
            InitialProperties.StartSizeMin = 10.0f;
            InitialProperties.StartSizeMax = 10.0f;
            InitialProperties.EndSizeMin = 10;
            InitialProperties.EndSizeMax = 10;
            InitialProperties.StartColorMin = Color.White;
            InitialProperties.StartColorMax = Color.White;
            InitialProperties.EndColorMin = Color.White;
            InitialProperties.EndColorMax = Color.White;
            InitialProperties.InterpolateBetweenMinAndMaxColors = false;
        }

        public override void InitializeZAPSParticle(DefaultTexturedQuadParticle cParticle)
        {
            base.InitializeZAPSParticle(cParticle);
            cParticle.Size = 100;
            cParticle.Lifetime = Single.PositiveInfinity;
            cParticle.Position = _satellite.Location + 10 * Vector3.UnitX * (NumberOfActiveParticles + 1);            
        }

        /// <summary>
        /// Before the update, move the particles in the right place.
        /// </summary>
        /// <param name="fElapsedTimeInSeconds"></param>
        protected override void BeforeUpdate(float fElapsedTimeInSeconds)
        {
            base.BeforeUpdate(fElapsedTimeInSeconds);

            Emitter.PositionData.Position = _satellite.Location;
            this.SetWorldViewProjectionMatrices(Matrix.Identity, MenuContainer.Camera.GetViewMatrix(), MenuContainer.Camera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport));

            //Now set up the particles.
            Vector3 offset = 100.0f * Vector3.UnitZ;
            offset = Vector3.Transform(offset, _satellite.ObjectRotation);
            for (int i = 0; i < ParticleBudgetTotal; i++)
            {
                Particles[i].Position = _satellite.Location + offset * (i+1);
                Particles[i].Orientation = _satellite.ObjectRotation;
            }

        }

    }
}
