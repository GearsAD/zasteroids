﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Common.Filters;
using Microsoft.Xna.Framework.Input;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// A camera that incorporates the shaker components.
    /// </summary>
    public class ZAsteroidsSatelliteCamera : ZAsteroidsCamera
    {
        #region Public Variables

        /// <summary>
        /// The orientation velocity of the camera - pressing a key fires the value.
        /// </summary>
        public FirstOrderFilter[] OrientationVelocityAxes { get; protected set; }

        #endregion

        /// <summary>
        /// Create a new camera...
        /// </summary>
        public ZAsteroidsSatelliteCamera(float movementCoefficient, float orientationCoefficient) : base(movementCoefficient)
        {
            Orientation = Quaternion.Identity;
            OrientationVelocityAxes = new FirstOrderFilter[3];
            for (int i = 0; i < OrientationVelocityAxes.Length; i++) OrientationVelocityAxes[i] = new FirstOrderFilter(orientationCoefficient, 0, 0);
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Update the orientation filters.
            for (int i = 0; i < OrientationVelocityAxes.Length; i++) OrientationVelocityAxes[i].Update(gameTime);

            //Check the keyboard and mouse.
            MouseState state = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            //Get the differentials of the movement.
            float dx = ((float)state.X - (float)(MenuContainer.GraphicsDevice.Viewport.Width / 2.0f)) / (float)MenuContainer.GraphicsDevice.Viewport.Width ;
            float dy = ((float)state.Y - (float)(MenuContainer.GraphicsDevice.Viewport.Height / 2.0f)) / (float)MenuContainer.GraphicsDevice.Viewport.Height ;

            if (Math.Abs(dx) <= 0.01f) dx = 0;
            if (Math.Abs(dy) <= 0.01f) dy = 0;

            dx /= 5.0f;
            dy /= 5.0f;
            //Set the camera rotational velocities.
            //First zero them.
            for (int i = 0; i < OrientationVelocityAxes.Length; i++) OrientationVelocityAxes[i].TargetValue = 0;
            //if (keyState.IsKeyDown(Keys.Space))
            //{
                if (state.RightButton == ButtonState.Released)
                {
                    OrientationVelocityAxes[0].TargetValue = dy; //X axis rotation is from y movement.
                    OrientationVelocityAxes[1].TargetValue = dx; //Y axis rotation is from x movement.
                    OrientationVelocityAxes[2].TargetValue = 0; //0
                }
                else
                {
                    OrientationVelocityAxes[2].TargetValue = dx; //Z axis rotation is from x movement.
                    OrientationVelocityAxes[1].TargetValue = 0; //0
                }
            //}

            //Now apply the rotations.
            Orientation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, OrientationVelocityAxes[0].CurrentValue);
            Orientation.Normalize();
            Orientation *= Quaternion.CreateFromAxisAngle(-Vector3.UnitY, OrientationVelocityAxes[1].CurrentValue);
            Orientation.Normalize();
            Orientation *= Quaternion.CreateFromAxisAngle(Vector3.UnitZ, OrientationVelocityAxes[2].CurrentValue);
            Orientation.Normalize();
        }

        /// <summary>
        /// Get the view matrix for the camera.
        /// </summary>
        /// <returns></returns>
        public override Matrix GetViewMatrix()
        {
            //Add the camera shake to the camera's position.
            Vector3 camLoc = cameraLocation;
            Vector3 tarloc = 10 * Vector3.UnitZ; //Need to multiply this out so that the rotations don't look jerky... interesting... [GearsAD]
            tarloc = Vector3.Transform(tarloc, Matrix.CreateFromQuaternion(Orientation));
            if (cameraShaker != null)
            {
                camLoc += new Vector3(cameraShaker.offsetCameraPosition.X, 0, cameraShaker.offsetCameraPosition.Y);
                tarloc += new Vector3(cameraShaker.offsetCameraPosition.X, 0, cameraShaker.offsetCameraPosition.Y);
            }

            return Matrix.CreateLookAt(camLoc, camLoc + tarloc, Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(Orientation)));
        }

    }
}
