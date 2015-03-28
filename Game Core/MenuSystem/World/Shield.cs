using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZAsteroids.World.Effects.ExplosionSmoke;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World
{
    /// <summary>
    /// The shield for the satellite - some overrides to do the animation.
    /// </summary>
    public class Shield : SceneObject
    {
        #region Public Properties
        /// <summary>
        /// Used to manage the volume of the sounds.
        /// </summary>
        public float Radius { get { return this._cdBoundingSphere.Radius; } }
        #endregion

        #region Private Fields
        /// <summary>
        /// The current axial rotations.
        /// </summary>
        Vector3 _rotations = new Vector3();
        /// <summary>
        /// The rotational velocity.
        /// </summary>
        Vector3 _rotationalVelRadS = 0.1f * 3.14159f * Vector3.One;
        #endregion
        public Shield()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Load the model.
            this.Model = WorldContent.menuSystemContent.ShieldModel;
            this.Alpha = 1f;
            this.IsAdditiveBlending = true;
            this.IsZTransparent = true;

            //Set the bounding sphere
            this._cdBoundingSphere = new BoundingSphere(this.Location, this.Model.Meshes[0].BoundingSphere.Radius);

        }

        public override void DieEffect(Microsoft.Xna.Framework.Vector3 locationOfCollision)
        {
            base.DieEffect(locationOfCollision);
            IncendiaryExplosionACE expl = new IncendiaryExplosionACE(locationOfCollision, Vector3.Zero);
            expl.Initialize();
            expl.AddChildrenToMenuContainer();
            MenuContainer.CompoundEffects.Add(expl);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            //Update the rotations
            _rotations += _rotationalVelRadS * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Saving the accuracy.
            while (_rotations.X > 2.0f * Math.PI) _rotations.X -= 2.0f * (float)Math.PI;
            while (_rotations.Y > 2.0f * Math.PI) _rotations.Y -= 2.0f * (float)Math.PI;
            while (_rotations.Z > 2.0f * Math.PI) _rotations.Z -= 2.0f * (float)Math.PI;
        }

        /// <summary>
        /// Override and draw the three components.
        /// </summary>
        /// <param name="inBlack"></param>
        public override void Draw(bool inBlack)
        {
            //Draw it three times with the different axes
            SceneRotation = new Vector3(_rotations.X,0,0);
            base.Draw(inBlack);
            //SceneRotation = new Vector3(_rotations.Y, 0, 0);
            //base.Draw(inBlack);
            //SceneRotation = new Vector3(0,_rotations.X, 0);
            //base.Draw(inBlack);
            //SceneRotation = new Vector3(0,-_rotations.X, 0);
            //base.Draw(inBlack);
            //SceneRotation = new Vector3(0,0,_rotations.Z);
            //base.Draw(inBlack);
        }
    }
}
