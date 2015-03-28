using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World.Collision_Objects
{
    /// <summary>
    /// A sphere collision container.
    /// </summary>
    public class CollisionSphere : CollisionContainer
    {
        #region Public Properties

        /// <summary>
        /// The maximum radius of the object.
        /// </summary>
        public float Radius { get; set; }

        #endregion

        public CollisionSphere(Vector3 origin, float radius)
        {
            this.Origin = origin;
            this.Radius = radius;
        }

        /// <summary>
        /// Do the collision detection.
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool IsCollidingWith(CollisionContainer otherObject)
        {
            //Case 1 - Sphere to sphere collision.
            if (otherObject.GetType() == typeof(CollisionSphere))
            {
                //Calculate the two centers.
                Vector3
                    center1 = this.Location + Vector3.Transform(Origin, Orientation),
                    center2 = otherObject.Location + Vector3.Transform(otherObject.Origin, otherObject.Orientation);

                //Now check their additive distance.
                if ((center1 - center2).LengthSquared() < Math.Pow(this.Radius, 2) + Math.Pow((otherObject as CollisionSphere).Radius, 2))
                    return true;
                return false;
            }
            //Case 2 - Sphere to cylinder collision.
            if (otherObject.GetType() == typeof(CollisionCylinder))
                //Let the cylinder do the collision testing.
                return otherObject.IsCollidingWith(this);
            //Case 3 - 
            throw new Exception("Collision method not supported for CollisionSphere and " + otherObject.GetType().ToString());
        }

    }
}
