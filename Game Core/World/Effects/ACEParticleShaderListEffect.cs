using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects.DPSF;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.MenuSystem;

namespace ZitaAsteria.World.Effects
{
    /// <summary>
    /// A class that just controls a list of particle systems and shaders if they're used as an effect - makes life easier to build compound particle effects.
    /// Don't add them to the manager, this will be done in the class. Just add them to the list before Initialize() (NOTE: in BuildParticleList() and BuildShaderList()!) call,
    /// and then set the lifetime of the ACE if you need to.
    /// </summary>
    public abstract class ACEParticleShaderListEffect : AbstractCompoundEffect
    {
        #region Public Properties
        #endregion

        #region Private Fields
        /// <summary>
        /// The managed particle systems
        /// </summary>
        protected List<AbstractBillboardZAPS> _particleSystems = new List<AbstractBillboardZAPS>();
        /// <summary>
        /// The managed shader effects
        /// </summary>
        protected List<ShaderEffect> _shaderEffects = new List<ShaderEffect>();
        #endregion

        public ACEParticleShaderListEffect()
        {
        }

        /// <summary>
        /// Here you build the list of particlesystems.
        /// </summary>
        protected abstract List<AbstractBillboardZAPS> BuildParticleList();
        /// <summary>
        /// Here you build the list of shaders.
        /// </summary>
        /// <returns></returns>
        protected abstract List<ShaderEffect> BuildShaderList();

        /// <summary>
        /// Will initialize all particle systems if they aren't already.
        /// </summary>
        public override void Initialize()
        {
            _particleSystems = BuildParticleList();
            _shaderEffects = BuildShaderList();
            if (_particleSystems.Count == 0 && _shaderEffects.Count == 0) throw new Exception("You must add the particle systems into _particleSystems from BuildParticleList() and the shaders into _shaderEffects drom BuildShaderList()! There are none in the list during an initialization.");
            //base.Initialize();

            //Initialize the particle systems and add them to the manager.
            for (int i = 0; i < _particleSystems.Count; i++)
            {
                if (!_particleSystems[i].IsInitialized)
                    _particleSystems[i].Initialize();
            }

            //Initialize the shader effects
            for (int i = 0; i < _shaderEffects.Count; i++)
            {
                if (!_shaderEffects[i].IsInitialized)
                    _shaderEffects[i].Initialize();
            }

            //Now reset the system
            Reset();
        }

        /// <summary>
        /// Enables all the particle system emitters and shaders - used by weapons.
        /// </summary>
        public virtual void EnableSystems()
        {
            for (int i = 0; i < _particleSystems.Count; i++)
                _particleSystems[i].IsEmitting = true;
            //Enable the shaders
            for (int i = 0; i < _shaderEffects.Count; i++)
                _shaderEffects[i].IsEnabled = true;
        }

        /// <summary>
        /// Disables all the particle system emitters and shaders - used by weapons.
        /// </summary>
        public virtual void DisableSystems()
        {
            for (int i = 0; i < _particleSystems.Count; i++)
                _particleSystems[i].IsEmitting = false;
            for (int i = 0; i < _shaderEffects.Count; i++)
                _shaderEffects[i].IsEnabled = false;
        }

        /// <summary>
        /// Adds all the particle systems to the world container.
        /// </summary>
        public override void AddChildrenToWorldContainer()
        {
            for (int i = 0; i < _particleSystems.Count; i++)
            {
                //Add the particles systems to the manager.
                WorldContainer.particleSystemManager.AddParticleSystem(_particleSystems[i]);
            }
            for (int i = 0; i < _shaderEffects.Count; i++)
            {
                WorldContainer.shaders.Add(_shaderEffects[i]);
            }
        }


        /// <summary>
        /// Adds all the particle systems to the menu container.
        /// </summary>
        public virtual void AddChildrenToMenuContainer()
        {
            for (int i = 0; i < _particleSystems.Count; i++)
            {
                //Add the particles systems to the manager.
                MenuContainer.ParticleSystemManager.AddParticleSystem(_particleSystems[i]);
            }
            for (int i = 0; i < _shaderEffects.Count; i++)
            {
                MenuContainer.Shaders.Add(_shaderEffects[i]);
            }
        }

        public override void Reset()
        {
            base.Reset();
            //Reset all the ZAPS
            for (int i = 0; i < _particleSystems.Count; i++)
                _particleSystems[i].Reset();
        }

        /// <summary>
        /// Update all their ground locations.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            //Update all their locations if they're bound.
            for (int i = 0; i < _particleSystems.Count; i++)
            {
                //if (_particleSystems[i].IsBoundToParentACELocation)
                //    _particleSystems[i].Location = new Vector3(this.Location.X, _particleSystems[i].Location.Y, this.Location.Y);
            }
        }

        /// <summary>
        /// When deleting, remove all the children items.
        /// </summary>
        public override void DeleteChildrenFromWorldContainer()
        {
            for (int i = 0; i < _particleSystems.Count; i++)
                WorldContainer.particleSystemManager.RemoveParticleSystem(_particleSystems[i]);
            for (int i = 0; i < _shaderEffects.Count; i++)
                if(WorldContainer.shaders.Contains(_shaderEffects[i]))
                    WorldContainer.shaders.Remove(_shaderEffects[i]);
        }

    }
}
