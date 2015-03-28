using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.World.Effects.DPSF;
using ZitaAsteria.World.Effects;
using ZitaAsteria;
using ZitaAsteria.MenuSystem;

namespace ZAsteroids.World.Effects.ExplosionSmoke
{
    /// <summary>
    /// A incendiary debris explosion [Gears].
    /// </summary>
    public class IncendiaryExplosionACE : ACEParticleShaderListEffect
    {
        #region Public Properties
        /// <summary>
        /// The 3D location.
        /// </summary>
        public Vector3 Location3D { get; set; }

        /// <summary>
        /// The 3D velocity.
        /// </summary>
        public Vector3 Velocity3D { get; set; }        
        #endregion

        #region Private Fields
        /// <summary>
        /// Stanard fire effect.
        /// </summary>
        FireZAPS _firePS;
        #endregion

        public IncendiaryExplosionACE()
        {
            this.IsUsingLifeSpan = true;
            this.LifeSpanMS = 6000;
        }

        public IncendiaryExplosionACE(Vector3 location, Vector3 velocity)
        {
            this.Location3D = location;
            this.Velocity3D = velocity;
            this.IsUsingLifeSpan = true;
            this.LifeSpanMS = 6000;
        }

        protected override List<AbstractBillboardZAPS> BuildParticleList()
        {
            List<AbstractBillboardZAPS> effects = new List<AbstractBillboardZAPS>();

            //Add the main explosion
            _firePS = new FireZAPS(Color.White, Location3D, Velocity3D, 100, 3.0f);
            _firePS.IsBoundToParentACELocation = false;
            effects.Add(_firePS);
            return effects;
        }

        protected override List<ZitaAsteria.World.ShaderEffects.ShaderEffect> BuildShaderList()
        {
            return new List<ZitaAsteria.World.ShaderEffects.ShaderEffect>();
        }


        public override void Reset()
        {
            //And now we can reset the base.
            base.Reset();
            //And restart the fire.
            _firePS.IsEmitting = true;
            _firePS.Emitter.PositionData.Position = Location3D; //LOCATION3D, otherwise bust...
            _firePS.Emitter.PositionData.Velocity = Velocity3D;
        }

        /// <summary>
        /// When this is done cause the effect.
        /// </summary>
        public override void AddChildrenToMenuContainer()
        {
            base.AddChildrenToMenuContainer();

            //Play the sound.
            //WorldContainer.soundEffectsMgr.PlaySoundEffect(WorldContent.sfxContent.IncendiaryExplosion);

            //MenuContainer.Camera.ApplyCameraShake(10);
        }

        /// <summary>
        /// Update the compound effect
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            _firePS.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            //Disable the fire if two seconds have passed.
            if (_elapsedLifeMS > 2000)
            {
                _firePS.IsEmitting = false;
            }

            //Update the velocity multiplier for the fire - spread it out for the first couple of milliseconds.
            if (_elapsedLifeMS < 1000) //Grow the fire for the first second..
            {
                _firePS.FireRingRadius += 2.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else //Shrink it.
            {
                _firePS.FireRingRadius -= 3.0f * _firePS.FireRingRadius * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
