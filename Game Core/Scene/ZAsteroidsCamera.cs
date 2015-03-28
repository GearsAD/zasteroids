using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.Screen_Effects;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Common.Filters;
using Microsoft.Xna.Framework.Input;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// Standard camera for ZAsteroids, just incorporates the Orientation quaternion.
    /// </summary>
    public class ZAsteroidsCamera : OrbitalCamera
    {
        #region Public Variables
        /// <summary>
        /// The orientation of the camera - may drift (just for asteroids at the moment).
        /// </summary>
        public Quaternion Orientation { get; set; }

        #endregion

        /// <summary>
        /// Create a new camera...
        /// </summary>
        public ZAsteroidsCamera(float movementCoefficient) : base(movementCoefficient)
        {
            Orientation = Quaternion.Identity;
        }

    }
}
