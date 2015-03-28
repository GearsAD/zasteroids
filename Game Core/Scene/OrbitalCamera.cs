using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// A camera that incorporates a target filter as well. Used in the menu system.
    /// </summary>
    public class OrbitalCamera : SatelliteCamera
    {
        #region Public Properties
        /// <summary>
        /// The target's target.
        /// </summary>
        public Vector3 targetTargetLocation { get; set; }
        #endregion

        public OrbitalCamera(float movementCoefficient)
            : base(movementCoefficient)
        {
        }

        /// <summary>
        /// Incorporate the target filtering as well.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Find how far the camera is from the target.
            Vector3 distanceFromTarget = this.targetTargetLocation - this.targetLocation;

            //Now we use time and the coefficient to only apply a small change. The bigger the coefficient, the faster the camera will get there.
            float timeBetweenUpdates = (float)gameTime.ElapsedGameTime.TotalSeconds;
            distanceFromTarget *= movementCoefficient * timeBetweenUpdates;

            //Apply the small change to the current Location.
            this.targetLocation += distanceFromTarget;

        }
    }
}
