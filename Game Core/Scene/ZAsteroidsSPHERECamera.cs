using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Common.Filters;
using Microsoft.Xna.Framework.Input;
using ZAsteroids.World.SPHEREs;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// A camera that incorporates the shaker components.
    /// </summary>
    public class ZAsteroidsSPHERECamera : ZAsteroidsCamera
    {
        #region Public Variables

        /// <summary>
        /// The target sphere.
        /// </summary>
        public SPHERE Sphere { get; protected set; }

        /// <summary>
        /// The camera offset from the SPHERE.
        /// </summary>
        public Vector3 CameraOffsetFromSphere { get; set; }

        /// <summary>
        /// If true then using the camera is bound to the sphere's orientation.
        /// </summary>
        public bool IsUsingFilters { get; set; }

        #endregion

        #region Private Fields
        #endregion

        /// <summary>
        /// Create a new camera...
        /// </summary>
        public ZAsteroidsSPHERECamera(OrbitalCamera baseCamera, SPHERE targetSphere)
            : base(5.0f)
        {
            CameraOffsetFromSphere = new Vector3(0, 3, -5);
            Orientation = targetSphere.ObjectRotation;
            Sphere = targetSphere;
            this.farFrustum = baseCamera.farFrustum;
            this.cameraLocation = baseCamera.cameraLocation;
            this.targetLocation = baseCamera.targetLocation;
            //Try copy out the orientation.
            if(baseCamera as ZAsteroidsSatelliteCamera != null)
                this.Orientation = (baseCamera as ZAsteroidsSatelliteCamera).Orientation;
            if (baseCamera as ZAsteroidsSPHERECamera != null)
                this.Orientation = (baseCamera as ZAsteroidsSPHERECamera).Orientation;

            //Test
            IsUsingFilters = false;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this.targetTargetLocation = Sphere.Location;
            this.targetCameraLocation = Sphere.Location; 

            base.Update(gameTime);

            this.Orientation = Sphere.ObjectRotation;
        }

        /// <summary>
        /// Get the view matrix for the camera.
        /// </summary>
        /// <returns></returns>
        public override Matrix GetViewMatrix()
        {
            //Add the camera shake to the camera's position.
            if (IsUsingFilters)
            {
                Vector3 tarloc = this.targetTargetLocation; //Need to multiply this out so that the rotations don't look jerky... interesting... [GearsAD]
                Vector3 camloc = Vector3.Transform(CameraOffsetFromSphere, Matrix.CreateFromQuaternion(Orientation));
                return Matrix.CreateLookAt(this.cameraLocation + camloc, tarloc, Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(Orientation)));
            }
            else
            {
                //Just use the position but don't use the camera's orientation.
                Vector3
                    forward = Vector3.Transform(CameraOffsetFromSphere, MenuContainer.SpherePlayer.ObjectRotation),
                    up = Vector3.Transform(10.0f * Vector3.UnitY, MenuContainer.SpherePlayer.ObjectRotation);
                return Matrix.CreateLookAt(this.cameraLocation + forward, this.cameraLocation - 3.0f * forward, up);
            }
        }

    }
}
