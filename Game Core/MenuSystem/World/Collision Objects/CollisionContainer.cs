using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World.Collision_Objects
{
    /// <summary>
    /// A simple abstract class for collision detection with spheres and cylinders.
    /// </summary>
    public abstract class CollisionContainer
    {
        #region Public Properties
        /// <summary>
        /// The location of the collisioncontainer.
        /// </summary>
        public Vector3 Location { get; set; }

        /// <summary>
        /// The origin of this collision container.
        /// </summary>
        public Vector3 Origin { get; set; }

        /// <summary>
        /// The orientation of the collision container.
        /// </summary>
        public Quaternion Orientation { get; set; }
        #endregion

        public CollisionContainer()
        {
            Orientation = Quaternion.Identity;
        }

        /// <summary>
        /// Check if it collides.
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public abstract bool IsCollidingWith(CollisionContainer otherObject);
    }
}
