using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// A camera that incorporates the shaker components.
    /// </summary>
    public class SatelliteCamera : ICamera
    {
        public Vector3 cameraLocation { get; set; }
        public Vector3 targetCameraLocation { get; set; }
        public Vector3 targetLocation { get; set; }

        /// <summary>
        /// The far frustum distance.
        /// </summary>
        public float farFrustum { get; set; }

        /// <summary>
        /// The far frustum distance.
        /// </summary>
        public float fov { get; set; }

        /// <summary>
        /// The coefficient of movement - how fast will the camera get to the target Location.
        /// </summary>
        public float movementCoefficient = 0.5f;

        /// <summary>
        /// Create a new camera shaker.
        /// </summary>
        protected CameraShaker cameraShaker = new CameraShaker();

        /// <summary>
        /// Create a new camera...
        /// </summary>
        public SatelliteCamera(float movementCoefficient)
        {
            targetCameraLocation = new Vector3(0, 0, 0);    
            cameraLocation = new Vector3(0, 0, 0);
            this.movementCoefficient = movementCoefficient;
            farFrustum = 500; //Default.
            fov = 60;
        }

        /// <summary>
        /// This adds a camera shaker CE.
        /// </summary>
        public virtual void InitializeCameraShakerCE()
        {
        }

        /// <summary>
        /// Returns the raw camera Location.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRawCameraLocation()
        {
            return cameraLocation;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //Update the camera's shaker.
            cameraShaker.Update(gameTime);

            //Find how far the camera is from the target.
            Vector3 distanceFromTarget = this.targetCameraLocation - this.cameraLocation;

            //Now we use time and the coefficient to only apply a small change. The bigger the coefficient, the faster the camera will get there.
            float timeBetweenUpdates = (float)gameTime.ElapsedGameTime.TotalSeconds;
            distanceFromTarget *= movementCoefficient * timeBetweenUpdates;

            //Apply the small change to the current Location.
            this.cameraLocation += distanceFromTarget;


        }

        /// <summary>
        /// This method converts the world coordinates of this object to screen coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScreenLocation(Vector3 worldLocation)
        {
            //Now convert it to a screen Location.
            Vector3 screenLocation =
                WorldContainer.graphicsDevice.Viewport.Project(
                    worldLocation,
                    WorldContainer.gameCamera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                    WorldContainer.gameCamera.GetViewMatrix(),
                    Matrix.Identity);

            return new Vector2(screenLocation.X, screenLocation.Y);
        }

        /// <summary>
        /// This method converts the world coordinates of this object to screen coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetWorldLocation(Vector3 screenLocation)
        {
            //Now convert it to a screen Location.
            Vector3 worldLocation =
                WorldContainer.graphicsDevice.Viewport.Unproject(
                    screenLocation,
                    WorldContainer.gameCamera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                    WorldContainer.gameCamera.GetViewMatrix(),
                    Matrix.Identity);

            return worldLocation;
        }

        /// <summary>
        /// This method converts the world coordinates of this object to screen coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetWorldLocationWithoutCameraShake(Vector3 screenLocation)
        {
            //Now convert it to a screen Location.
            Vector3 worldLocation =
                WorldContainer.graphicsDevice.Viewport.Unproject(
                    screenLocation,
                    WorldContainer.gameCamera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                    (WorldContainer.gameCamera as SatelliteCamera).GetViewMatrixWithoutShake(),
                    Matrix.Identity);

            return worldLocation;
        }

        /// <summary>
        /// Apply some camera shake, in pixels (well, std. dev. of pixels but whatever..).
        /// </summary>
        /// <param name="shakeAmountPx"></param>
        public void ApplyCameraShake(double shakeAmountPx)
        {
            cameraShaker.curStdDev += shakeAmountPx;
        }

        /// <summary>
        /// Get the view matrix for the camera.
        /// </summary>
        /// <returns></returns>
        public virtual Matrix GetViewMatrix()
        {
            //Add the camera shake to the camera's position.
            Vector3 camLoc = cameraLocation;
            Vector3 tarloc = targetLocation;
            if (cameraShaker != null)
            {
                camLoc += new Vector3(cameraShaker.offsetCameraPosition.X, 0, cameraShaker.offsetCameraPosition.Y);
                tarloc += new Vector3(cameraShaker.offsetCameraPosition.X, 0, cameraShaker.offsetCameraPosition.Y);
            }

            //Vector2 forward = WorldContainer.playas[0].GetForwardVector();
            //Vector2 loc = WorldContainer.playas[0].Location;
            //float height = 0;
            //if (WorldContainer.map != null)
            //    height = WorldContainer.map.GetTerrainHeightFor(WorldContainer.playas[0]);
            //Vector3 camLoc = new Vector3(loc.X - forward.X * 10.0f, height + 10, loc.Y - forward.Y * 10.0f);
            //Vector3 tarloc = new Vector3(loc.X, height + 2, loc.Y);
            return Matrix.CreateLookAt(camLoc, tarloc, Vector3.Up);
        }

        /// <summary>
        /// Get the view matrix for the camera.
        /// </summary>
        /// <returns></returns>
        public virtual Matrix GetViewMatrixWithoutShake()
        {
            return Matrix.CreateLookAt(cameraLocation, targetLocation, Vector3.Up);
        }        
        /// <summary>
        /// Get the view matrix for the camera when it's at it's target Location.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrixForTarget()
        {
            return Matrix.CreateLookAt(targetCameraLocation, targetCameraLocation + new Vector3(0, -100, 1), Vector3.Up);
        }

        /// <summary>
        /// Get the projection matrix for the camera.
        /// </summary>
        /// <param name="viewPort"></param>
        /// <returns></returns>
        public Matrix GetProjectionMatrix(Viewport viewPort)
        {
            return Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(fov),
            (float)viewPort.Width / (float)viewPort.Height,
            1.0f, //Near frusutm
            farFrustum); //Far frustum
        }
        
        // NOT USED IN ZASTEROIDS
        
        public Matrix GetOrthographicProjectionMatrix(Microsoft.Xna.Framework.Graphics.Viewport viewPort) { return Matrix.Identity; }
        public Matrix GetShadowMapViewMatrix() { return Matrix.Identity; }
    }
}
