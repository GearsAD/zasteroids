using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem.World.Collision_Objects;

namespace ZitaAsteria.MenuSystem.World.Satellite
{
    public class Satellite : SceneObject
    {
        #region Public Properties
        /// <summary>
        /// Max health
        /// </summary>
        public float LifeMax { get; set; }

        /// <summary>
        /// Get the life percent.
        /// </summary>
        public float LifePercent { get { return (float)Life / (float)LifeMax * 100.0f; } }
        #endregion

        #region Private Fields
        /// <summary>
        /// The targeting ZAPS.
        /// </summary>
        SatelliteTargetingZAPS[] _targetingZAPS = null;
        #endregion

        public Satellite()
        {
            LifeMax = 1000;
            Life = LifeMax;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Load the model.
            this.Model = WorldContent.menuSystemContent.SatelliteModel;

            _targetingZAPS = new SatelliteTargetingZAPS[2]; //Damn DPSF, can't change out the texture on the fly!!!! Easiest way is to make 2 reticles[GearsAD] 
            for (int i = 0; i < 2; i++)
            {
                _targetingZAPS[i] = new SatelliteTargetingZAPS(this, WorldContent.menuSystemContent.SatelliteReticle[i]);
                _targetingZAPS[i].Initialize();
                MenuContainer.ParticleSystemManager.AddParticleSystem(_targetingZAPS[i]);
            }

            //Set up the collision cylinders.
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, 0, -50), 0, 64, 16, Vector3.UnitZ)); //Back Dish 1
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, 0, -34), 0, 50, 8, Vector3.UnitZ)); //Back Dish 1
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, 0, -26), 0, 34, 56, Vector3.UnitZ)); //Column energizer
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, 0, -20), 0, 16, 116, Vector3.UnitZ)); //Main column
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, -98, -14), 0, 11.433f,  196, Vector3.UnitY)); //Lateral spire
            this._cdCollisionContainers.Add(new CollisionCylinder(new Vector3(0, 0, -77), 554, 590, 154, Vector3.UnitZ)); //Docking ring
            this._cdCollisionContainers.Add(new CollisionSphere(new Vector3(484.830f, 0, 0), 30.6f)); //WorldSphere on right.s

        }

        /// <summary>
        /// Set the targeting state of the tracer.
        /// </summary>
        /// <param name="state"></param>
        public void SetTargetingState(SatelliteTargetingStateEnum state)
        {
            //Aaargh, DPSF u monster, make me make two... so sad :( [GearsAD]
            _targetingZAPS[0].Visible = state == SatelliteTargetingStateEnum.NoHit;
            _targetingZAPS[1].Visible = state == SatelliteTargetingStateEnum.WillHit;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            //ObjectRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f * 3.14145926f * 0.025f);
            //ObjectRotation.Normalize();

            for(int i = 0; i < 2; i++)
                _targetingZAPS[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
