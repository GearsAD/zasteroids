using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World.Collision_Objects
{
    /// <summary>
    /// A cylinder collision container.
    /// </summary>
    public class CollisionCylinder : CollisionContainer
    {
        #region Public Properties

        /// <summary>
        /// The minimum radius of the object. Set > 0 to make a tube.
        /// </summary>
        public float MinRadius { get; set; }

        /// <summary>
        /// The maximum radius of the object.
        /// </summary>
        public float MaxRadius { get; set; }

        /// <summary>
        /// The pointing direction of the cylinder.
        /// </summary>
        public Vector3 PointingDir { get; set; }

        /// <summary>
        /// The length of the cylinder.
        /// </summary>
        public float Length { get; set; }

        #endregion

        public CollisionCylinder(Vector3 origin, float minradius, float maxradius, float length, Vector3 pointingDirection)
        {
            this.Origin = origin;
            this.MinRadius = minradius;
            this.MaxRadius = maxradius;
            Length = length;
            PointingDir = pointingDirection;
            if (Length == 0) throw new Exception("Cannot create a collision cylinder with a length of zero.");
        }

        /// <summary>
        /// Do the collision detection - this assumes the sphere is small for now.
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool IsCollidingWith(CollisionContainer otherObject)
        {
            //Case 1 - Sphere to sphere collision.
            if (otherObject.GetType() == typeof(CollisionSphere))
            {
                CollisionSphere sphere = otherObject as CollisionSphere;
                //1.1 Generate the cylinder ray.
                Vector3 x0 = this.Location + Vector3.Transform(Origin, Orientation);
                Vector3 x1 = x0 + Length * Vector3.Transform(PointingDir, Orientation);
                Vector3 p = sphere.Location;
                
                //2. Firstly check if it's along the infinite radius cylinder by checking the dot products 
                float
                    minDot = Vector3.Dot(p - x0, x1 - x0),
                    maxDot = Vector3.Dot(p - x1, x0 - x1);
                if (
                    minDot < 0 ||
                    maxDot < 0)
                    return false;
                
                //3. Now calculate the orthogonal distance from the ray to the sphere's center
                //Ref - http://mathworld.wolfram.com/Point-LineDistance3-Dimensional.html
                float d =
                        Vector3.Cross(p - x0, p - x1).Length() / 
                        (x1 - x0).Length();
                //4. If that's inside the cylinder we have a winner (assuming the sphere is far smaller than the cylinder)
                if (
                    d <= MaxRadius &&
                    d >= MinRadius)
                    return true;
                return false;
            }
            //Case 2 - Cylinder to cylinder collision.
            throw new Exception("Collision method not supported for CollisionSphere and " + otherObject.GetType().ToString());            
        }

    }
}
