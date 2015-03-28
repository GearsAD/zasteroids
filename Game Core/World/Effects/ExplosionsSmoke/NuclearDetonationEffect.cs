using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects;
using DPSF.ParticleSystems;
using ZitaAsteria;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.Screen_Effects;

namespace ZitaAsteria.World.Effects.Explosions
{
    public class NuclearDetonationEffect : AbstractCompoundEffect
    {

        NuclearDetonationParticleSystem nukeEffect { get; set; }
        
        float lifeSpan = 0;
        float maxLifeSpan = 105.0f;

        public NuclearDetonationEffect()
        {
            this.worldScale = Vector2.One;
        }

        public override void Initialize()
        {
            base.Initialize();

            // 3D Location
            Vector3 explosionLocation3D;
            explosionLocation3D.X = this.Location.X;
            explosionLocation3D.Y = WorldContainer.map.GetTerrainHeightAt(Location);
            explosionLocation3D.Z = this.Location.Y;

            /**********************************************************************************************************/
            // ShockWave Explosion
            nukeEffect = new NuclearDetonationParticleSystem(WorldContainer.gameClass);
            nukeEffect.DrawOrder = 1200;
            nukeEffect.Location = explosionLocation3D;
            nukeEffect.AutoInitialize(WorldContainer.graphicsDevice, WorldContent.contentManager, WorldContainer.gameClass.spriteBatch);

            WorldContainer.particleSystemManager.AddParticleSystem(nukeEffect);

            //addMiscellaneousEffects();

            /*******************************************************************/
            /* Shader based Part of Explosion Effect                           */
            /*******************************************************************/
            //ExplosionFlash flash    = new ExplosionFlash();
            //flash.allowOverExposure = true;
            //flash.effectLocation    = this.GetScreenLocation();

            //flash.Initialize();
            //WorldContainer.compoundEffects.Add(flash);
            /*******************************************************************/

            //Apply the damage physics
            //SplashDamageUtility.ApplySplashDamageToWorldObjects(this, 20000, 250, 10);
            //Apply the camera shake
            //WorldContainer.gameCamera.ApplyCameraShake(2.5);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            lifeSpan += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (lifeSpan > (maxLifeSpan / 3))
            {
                nukeEffect.disableEmitterOnly();
            }
        }
    
        public override void DeleteChildrenItems()
        {
            nukeEffect.Destroy();
            WorldContainer.particleSystemManager.RemoveParticleSystem(nukeEffect);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override bool ShouldDeleteFromWorldContainer()
        {
        
            if (lifeSpan > maxLifeSpan)
            {
                return true;
            }
            else
            {
               return false;
            }  
        }

        public override void triggerDeleteTimer()
        {
            useDeleteTimer = true;
        }

        public override void triggerDeleteTimer(float timeToElapse)
        {
            deleteTimerMS = timeToElapse;
            useDeleteTimer = true;
        }
    }
}
