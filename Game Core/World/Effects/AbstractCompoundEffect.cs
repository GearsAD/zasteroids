using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZitaAsteria.World.Effects
{
    /// <summary>
    /// A compound effect that doesn't get drawn, it just controls items in the WorldContainer...
    /// Use AddACEComponentsToWorldConainer to add the effect's components to the world container, don't do it in initialize because the ObjectManager will then add them to the world when it caches them!
    /// </summary>
    public abstract class AbstractCompoundEffect
    {
        #region Private Fields
        /// <summary>
        /// The elapsed time of the system.
        /// </summary>
        protected float _elapsedLifeMS = 0;
        #endregion

        #region Public Properties
        /// <summary>
        /// The total lifespan of this ACE.
        /// </summary>
        public float LifeSpanMS {get; set;}

        /// <summary>
        /// If true then it's using the lifespan - transient effect.
        /// </summary>
        public bool IsUsingLifeSpan { get; set; }
        #endregion

        #region Public Events
        /// <summary>
        /// Fired when it is removed from the world container. Probably need to add the other ones in as well - soon!
        /// </summary>
        public event EventHandler RemovedFromWorldContainer;
        #endregion

        /// <summary>
        /// Create a new compound effect.
        /// </summary>
        public AbstractCompoundEffect()
        {
            IsUsingLifeSpan = false;
        }

        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Adds the effect to the world container - i.e. actually starts the effect. Must be called for an effect to be used. DO NOT ADD EFFECTS DURING INITIALIZE! [Gears]
        /// </summary>
        public abstract void AddChildrenToWorldContainer();

        /// <summary>
        /// Called when this item is deleted.
        /// </summary>
        public abstract void DeleteChildrenFromWorldContainer();

        /// <summary>
        /// This is fired from the game loop when the effect is removed.
        /// </summary>
        public void Event_TriggerRemovedFromWorldContainer()
        {            
            if (RemovedFromWorldContainer != null)
                RemovedFromWorldContainer(this, new EventArgs());
        }

        public virtual void Update(GameTime gameTime)
        {
            //ACE's don't do general physics updates.
            //base.Update(gameTime);

            if(IsUsingLifeSpan)
                _elapsedLifeMS += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Reset this effect.
        /// </summary>
        public virtual void Reset()
        {
            _elapsedLifeMS = 0;   
        }

        /// <summary>
        /// Returns true if this effect should be deleted from the WorldContainer. If it has exceeded it's life, then true.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldDeleteFromWorldContainer()
        {
            if (IsUsingLifeSpan && _elapsedLifeMS > LifeSpanMS) return true;
            return false;
        }

    }
}
