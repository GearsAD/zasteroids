using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ZitaAsteria.World.Effects.DPSF
{
    /// <summary>
    /// A simple quad superclass for all DPSF particle systems - one to bind them, one to rule them and all that [Gears].
    /// </summary>
    public abstract class AbstractQuadZAPS : DefaultTexturedQuadParticleSystem
    {
        #region Public Properties

        /// <summary>
        /// The texture used for the particle.
        /// </summary>
        public Texture2D ParticleTexture { get; set; }

        /// <summary>
        /// The total number of particles to budget for - this won't be exceeded! (unless u hack it, i suppose) [Gears]
        /// </summary>
        public int ParticleBudgetTotal { get; set; }

        /// <summary>
        /// If true then the particle system is emitting.
        /// </summary>
        public bool IsEmitting
        {
            get { return this.Emitter.Enabled; }
            set { this.Emitter.Enabled = value; }
        }

        /// <summary>
        /// Remapping of this.Visible. Same thing, but better convention (I have standards, okay? [Gears])
        /// </summary>
        public bool IsVisible
        {
            get { return this.Visible; }
            set { this.Visible = value; }
        }

        /// <summary>
        /// The location of the system emitter - set this, not the emitter directly!
        /// </summary>
        public Vector3 Location
        {
            get { return this.Emitter.PositionData.Position; }
            set { this.Emitter.PositionData.Position = value; }
        }

        /// <summary>
        /// Use to set the blend state to additive or alpha blend.
        /// </summary>
        public BlendState ParticleBlendingType
        {
            get { return RenderProperties.BlendState; }
            set {
                if (RenderProperties != null)
                    RenderProperties.BlendState = value;
                else throw new Exception("ParticleBlendingType can only be set after init - best place to do it is SetupZAPSEmitterProperties()!");
            }
        }


        #endregion

        #region Private Fields
        #endregion

        /// <summary>
        /// Just initialize it with the standard game class.
        /// </summary>
        public AbstractQuadZAPS()
            : base(WorldContainer.gameClass)
        {
            ParticleBudgetTotal = 0; // Need force this to be set.
        }

        /// <summary>
        /// Initialize this system, i.e. load it if it hasn't been loaded already. Will ignore multiple calls if already init'd.
        /// </summary>
        public void Initialize()
        {
            AutoInitialize(
                WorldContainer.graphicsDevice,
                WorldContent.contentManager,
                WorldContainer.spriteBatch);
        }

        /// <summary>
        /// Reset the particle system, will still keep it initialized but removes all active particles.
        /// </summary>
        public virtual void Reset()
        {
            this.RemoveAllParticles();
        }

        /// <summary>
        /// Auto initialize the system.
        /// </summary>
        /// <param name="cGraphicsDevice"></param>
        /// <param name="cContentManager"></param>
        /// <param name="cSpriteBatch"></param>
        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            if (!this.IsInitialized)
            {
                if (ParticleTexture == null) throw new Exception("Need to have a valid particle texture set before initializing the particle system! Did u do it in the constructor? ?");
                if (ParticleBudgetTotal == 0) throw new Exception("The particle budget for this system is set to zero! Did u do it in the constructor?");

                base.AutoInitialize(cGraphicsDevice, cContentManager, cSpriteBatch);
                InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, ParticleBudgetTotal, ParticleBudgetTotal,
                                                     UpdateVertexProperties, ParticleTexture);
                Name = "QuadZAPS";
                SetupZAPSEmitterProperties();

                ParticleInitializationFunction = InitializeZAPSParticle;

                SetupInitialSystemEvents();
                SetupInitialParticleProperties();
                this.Reset(); //Reset the effect.
            }
        }

        /// <summary>
        /// Initialize a single particle - you don't need to call it's initial properties method, that's already done!
        /// </summary>
        /// <param name="cParticle"></param>
        public virtual void InitializeZAPSParticle(DefaultTexturedQuadParticle cParticle)
        {
            InitializeParticleUsingInitialProperties(cParticle);
        }

        #region Abstract Methods for a subclass to fill in.

        /// <summary>
        /// Setup the budget, texture, etc. for the particle system before it gets initialized.
        /// </summary>
        public abstract void SetupZAPSEmitterProperties();
        /// <summary>
        /// Setup the initial particle system events.
        /// </summary>
        public abstract void SetupInitialSystemEvents();
        /// <summary>
        /// Set up the initial properties of the particle - life span, etc.
        /// </summary>
        public abstract void SetupInitialParticleProperties();
        #endregion
    }
}
