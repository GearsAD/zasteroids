using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.Characters;
using ZitaAsteria.World.Effects.Screen_Effects;
using ZitaAsteria.World.Characters.Players;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// The camera now fits in all players in the game.
    /// </summary>
    public class Camera : ICamera
    {
        public Vector3 cameraLocation { get; private set; }
        public Vector3 targetLocation { get; set; }

        /// <summary>
        /// The coefficient of movement - how fast will the camera get to the target Location.
        /// </summary>
        public float movementCoefficient = 0.5f;
        /// <summary>
        /// The minimum height of the camera.
        /// </summary>
        public static float minCameraHeight = 40;
        /// <summary>
        /// The maximum height of the camera.
        /// </summary>
        public static float maxCameraHeight = 100;
        /// <summary>
        /// The pixels around the border that specify the title safe region for the players. They need to be within this region and the camera will attempt to achieve this.
        /// i.e. Safe rectangle = (titleSafeBoundaries.X, titleSafeBoundaries.Y, 800 - titleSafeBoundaries.X, 600 - titleSafeBoundaries.Y) if the viewport is 800x600.
        /// </summary>
        public static Vector2 titleSafeBoundaries = new Vector2(250, 200);

        /// <summary>
        /// Create a new camera shaker.
        /// </summary>
        private CameraShaker cameraShaker = new CameraShaker();

        /// <summary>
        /// The camera shaker compound effect.
        /// </summary>
        private CameraShakerEffect cameraShakerCE = null;

        /// <summary>
        /// Create a new camera...
        /// </summary>
        public Camera(float movementCoefficient)
        {
            targetLocation = new Vector3(0, 0, 0);
            cameraLocation = new Vector3(0, 0, 0);
            this.movementCoefficient = movementCoefficient;
        }

        /// <summary>
        /// This adds a camera shaker CE.
        /// </summary>
        public void InitializeCameraShakerCE()
        {
            cameraShakerCE = new CameraShakerEffect();
            cameraShakerCE.Initialize();
            Vector3 camLoc = new Vector3(cameraLocation.X + cameraShaker.offsetCameraPosition.X, cameraLocation.Y, cameraLocation.Z + cameraShaker.offsetCameraPosition.Y);
            cameraShakerCE.SetCameraParameters(camLoc, cameraShaker.curStdDev);
            //Add it to the compound effects.
            WorldContainer.compoundEffects.Add(cameraShakerCE);
        }

        /// <summary>
        /// Returns the raw camera Location.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRawCameraLocation()
        {
            return cameraLocation;
        }

        public void UpdateBoundingArea(PlayerIndex player, ref Vector3 topLeftBounding, ref Vector3 bottomRightBounding)
        {
            if (ZAUtilities.HasPlayerJoined(player))
            {
                if (WorldContainer.playas[(int)player].Location.X > topLeftBounding.X)
                    topLeftBounding.X = WorldContainer.playas[(int)player].Location.X;
                if (WorldContainer.playas[(int)player].Location.Y > topLeftBounding.Z)
                    topLeftBounding.Z = WorldContainer.playas[(int)player].Location.Y;

                if (WorldContainer.playas[(int)player].Location.X < bottomRightBounding.X)
                    bottomRightBounding.X = WorldContainer.playas[(int)player].Location.X;
                if (WorldContainer.playas[(int)player].Location.Y < bottomRightBounding.Z)
                    bottomRightBounding.Z = WorldContainer.playas[(int)player].Location.Y;

                //Accounting for worst-case playa height...
                float height = 0;
                if (WorldContainer.map != null)
                    height = WorldContainer.map.GetTerrainHeightFor(WorldContainer.playas[(int)player]);

                if (height > topLeftBounding.Y)
                    topLeftBounding.Y = height;
                if (height > bottomRightBounding.Y)
                    bottomRightBounding.Y = height;
            }
        }

        /// <summary>
        /// This method updates the camera target, setting it to be somewhere where all the players are visible.
        /// </summary>
        private void UpdateCameraTarget()
        {
            //The two vectors that are used to find the bounding area of the players.
            Vector3
                topLeftBounding = new Vector3(Single.MinValue, Single.MinValue, Single.MinValue),
                bottomRightBounding = new Vector3(Single.MaxValue, Single.MinValue, Single.MaxValue);
            //The new target Location.
            Vector3
                newTargetLocation = new Vector3(0, 30, 0);


            if (!(ZAUtilities.HasPlayerJoined(PlayerIndex.One) || ZAUtilities.HasPlayerJoined(PlayerIndex.Two) || ZAUtilities.HasPlayerJoined(PlayerIndex.Three) || ZAUtilities.HasPlayerJoined(PlayerIndex.Four)))
                return;

            //Find the bounding area.
            UpdateBoundingArea(PlayerIndex.One, ref topLeftBounding, ref bottomRightBounding);
            UpdateBoundingArea(PlayerIndex.Two, ref topLeftBounding, ref bottomRightBounding);
            UpdateBoundingArea(PlayerIndex.Three, ref topLeftBounding, ref bottomRightBounding);
            UpdateBoundingArea(PlayerIndex.Four, ref topLeftBounding, ref bottomRightBounding);

            //Set the centre of the screen to the middle of this bounding box - do only with the X and Z coordinates. We'll set the Y Location of the camera with a more complex calc.
            newTargetLocation.X = (topLeftBounding.X + bottomRightBounding.X) / 2.0f;
            newTargetLocation.Z = (topLeftBounding.Z + bottomRightBounding.Z) / 2.0f;

            //Find these Locations on the screen...
            Vector3 topLeftScreen = WorldContainer.graphicsDevice.Viewport.Project(
                topLeftBounding,
                GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                GetViewMatrixForTarget(),
                Matrix.Identity);
            Vector3 bottomRightScreen = WorldContainer.graphicsDevice.Viewport.Project(
                bottomRightBounding,
                GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                GetViewMatrixForTarget(),
                Matrix.Identity);

            //Quick solution for this - iterate it rather than be deterministic (bit of a mission to do that and it's getting late).
            Vector2
                topLeftError = new Vector2(topLeftScreen.X - titleSafeBoundaries.X, topLeftScreen.Y - titleSafeBoundaries.Y),
                bottomRightError = new Vector2(
                    (WorldContainer.graphicsDevice.Viewport.Width - titleSafeBoundaries.X) - bottomRightScreen.X,
                    (WorldContainer.graphicsDevice.Viewport.Height - titleSafeBoundaries.Y) - bottomRightScreen.Y);
            float maxError = 0;
            //Check for a negative in any of the components, if they are then zoom out definitely...
            if (
                topLeftError.X < 0 ||
                topLeftError.Y < 0 ||
                bottomRightError.X < 0 ||
                bottomRightError.Y < 0)
            {
                maxError = Math.Min(Math.Min(Math.Min(topLeftError.X, topLeftError.Y), bottomRightError.X), bottomRightError.Y);
            }
            else
            {
                maxError = Math.Max(Math.Max(Math.Max(topLeftError.X, topLeftError.Y), bottomRightError.X), bottomRightError.Y);
            }

            //Add a negative error - if the error is positive we need to zoom in, otherwise we zoom out.
            newTargetLocation.Y = targetLocation.Y - maxError / 100.0f /*ARBITRARY SELECTION!*/;

            //Set the new target.
            if (newTargetLocation.Y > maxCameraHeight)
                newTargetLocation.Y = maxCameraHeight;
            if (newTargetLocation.Y < minCameraHeight)
                newTargetLocation.Y = minCameraHeight;
            targetLocation = newTargetLocation;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update the camera's target.
            //if(targetLocation != Vector3.Zero) //When the game starts, don't do this right away...
            UpdateCameraTarget();

            //Update the camera's shaker.
            cameraShaker.Update(gameTime);

            //Find how far the camera is from the target.
            Vector3 distanceFromTarget = this.targetLocation - this.cameraLocation;

            //Now we use time and the coefficient to only apply a small change. The bigger the coefficient, the faster the camera will get there.
            float timeBetweenUpdates = (float)gameTime.ElapsedGameTime.TotalSeconds;
            distanceFromTarget *= movementCoefficient * timeBetweenUpdates;

            //Apply the small change to the current Location.
            this.cameraLocation += distanceFromTarget;

            //If the camera shaker CE is not null, update it.
            if (cameraShakerCE != null)
            {
                Vector3 camLoc = new Vector3(cameraLocation.X + cameraShaker.offsetCameraPosition.X, cameraLocation.Y, cameraLocation.Z + cameraShaker.offsetCameraPosition.Y);
                cameraShakerCE.SetCameraParameters(camLoc, cameraShaker.curStdDev);
            }

            //PerformCameraBoxChecking();
        }

        /// <summary>
        /// Do the camera box checking.
        /// </summary>
        private void PerformCameraBoxChecking()
        {
            float
                viewWidth = WorldContainer.graphicsDevice.Viewport.Width,
                viewHeight = WorldContainer.graphicsDevice.Viewport.Height;

            Plane plane = new Plane(Vector3.Right, Vector3.Forward, Vector3.Left);

            Ray
                cameraErrorTopLeftErrorRay = new Ray(GetWorldLocation(new Vector3(0, 0, 0)), GetWorldLocation(new Vector3(0, 0, 1)) - GetWorldLocation(new Vector3(0, 0, 0)));
            Ray
                cameraErrorBottomRightErrorRay = new Ray(
                    GetWorldLocation(new Vector3(viewWidth, viewHeight, 0)),
                    GetWorldLocation(new Vector3(viewWidth, viewHeight, 1)) - GetWorldLocation(new Vector3(viewWidth, viewHeight, 0)));

            float?
                distTopLeft = cameraErrorTopLeftErrorRay.Intersects(plane),
                distBottomRight = cameraErrorBottomRightErrorRay.Intersects(plane);

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
        public Matrix GetViewMatrix()
        {
            //Add the camera shake to the camera's position.
            Vector3 camLoc = new Vector3(cameraLocation.X + cameraShaker.offsetCameraPosition.X, cameraLocation.Y, cameraLocation.Z + cameraShaker.offsetCameraPosition.Y);
            Vector3 tarLoc = new Vector3(cameraLocation.X + cameraShaker.offsetTargetPosition.X, cameraLocation.Y, cameraLocation.Z + cameraShaker.offsetTargetPosition.Y) + new Vector3(0, -100, 1f) /*The standard target offset*/;

            //Vector2 forward = WorldContainer.playas[PlayerIndex.One].GetForwardVector();
            //Vector2 loc = WorldContainer.playas[PlayerIndex.One].Location;
            //float height = 0;
            //if (WorldContainer.map != null)
            //    height = WorldContainer.map.GetTerrainHeightFor(WorldContainer.playas[PlayerIndex.One]);
            //Vector3 camLoc = new Vector3(loc.X - forward.X * 10.0f, height + 5, loc.Y - forward.Y * 10.0f);
            //Vector3 tarLoc = new Vector3(loc.X, height, loc.Y);
            return Matrix.CreateLookAt(camLoc, tarLoc, Vector3.Up);
        }

        /// <summary>
        /// Get the view matrix for the camera when it's at it's target Location.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrixForTarget()
        {
            return Matrix.CreateLookAt(targetLocation, targetLocation + new Vector3(0, -100, 1), Vector3.Up);
        }

        /// <summary>
        /// Get the projection matrix for the camera.
        /// </summary>
        /// <param name="viewPort"></param>
        /// <returns></returns>
        public Matrix GetProjectionMatrix(Viewport viewPort)
        {
            return Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(60),
            (float)viewPort.Width / (float)viewPort.Height,
            1.0f, //Near frusutm
            500.0f); //Far frustum
        }

        /// <summary>
        /// Get the projection matrix for the camera.
        /// </summary>
        /// <param name="viewPort"></param>
        /// <returns></returns>
        public Matrix GetOrthographicProjectionMatrix(Viewport viewPort)
        {
            return Matrix.CreateOrthographic(
            (float)256,
            (float)256,
            1.0f, //Near frusutm
            500.0f); //Far frustum
        }

        /// <summary>
        /// Get the view matrix for the shadowmap camera.
        /// </summary>
        /// <returns></returns>
        public Matrix GetShadowMapViewMatrix()
        {
            Vector3 loc = new Vector3(WorldContainer.map.Level.levelWidth / 2.0f, 100.0f, WorldContainer.map.Level.levelHeight / 2.0f);

            //Add the camera shake to the camera's position.
            Vector3 camLoc = loc;
            Vector3 tarLoc = loc + new Vector3(0, -100.0f, 0 + 0.001f);

            return Matrix.CreateLookAt(camLoc, tarLoc, Vector3.Up);
        }
    }
}
