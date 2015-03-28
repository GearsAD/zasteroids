using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// The class that manages the camera shake.
    /// </summary>
    public class CameraShaker
    {
        /// <summary>
        /// The current standard deviation of the camera's shake velocity.
        /// </summary>
        public double curStdDev { get; set; }

        /// <summary>
        /// The ration between the standard deviation and the rumble...
        /// </summary>
        private double rumbleToSDRatio = 0.75;

        /// <summary>
        /// The damping rate of the standard deviation (this is a multiple over time).
        /// </summary>
        private double stdDevDampingRate = 3f;

        /// <summary>
        /// The proportional controller coefficient, which tries to return the camera back to it's original position. Should probably use a full PID, but hell, this is a game!
        /// </summary>
        private double pControllerCoefficient = 3.0;
       
        /// <summary>
        /// The camera's offsets.
        /// </summary>
        public Vector2
            offsetCameraPosition {get; private set;}
        private Vector2
            offsetCameraVelocity;

        /// <summary>
        /// The target position's offsets.
        /// </summary>
        public Vector2
            offsetTargetPosition { get; private set; }
        private Vector2
            offsetTargetVelocity;

        /// <summary>
        /// Create a new camera shaker.
        /// </summary>
        public CameraShaker()
        {
        }

        /// <summary>
        /// Use the current camera's 
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //Get the elapsed game time.
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update the camera's offset position given its velocity.
            offsetCameraPosition += offsetCameraVelocity * elapsedGameTime;
            offsetTargetPosition += offsetTargetVelocity * elapsedGameTime;

            //Update the camera's velocity by applying some gaussian noise with the given standard deviation.
            if (curStdDev > 0.01) //Otherwise don't waste the cpuage...
            {
                offsetTargetVelocity.X = (float)ZAMathTools.GetGaussianRandom(0, curStdDev);
                offsetTargetVelocity.Y = (float)ZAMathTools.GetGaussianRandom(0, curStdDev);
                offsetCameraVelocity.X = (float)ZAMathTools.GetGaussianRandom(0, curStdDev);
                offsetCameraVelocity.Y = (float)ZAMathTools.GetGaussianRandom(0, curStdDev);

                //Set the rumble on the gamepads...
                float rumble = (float)((curStdDev > 1 ? 1 : curStdDev) / rumbleToSDRatio);
                if (WorldContainer.PlayersJoined[(int)PlayerIndex.One])
                    GamePad.SetVibration(PlayerIndex.One, rumble, rumble);
                else
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                if (WorldContainer.PlayersJoined[(int)PlayerIndex.Two])
                    GamePad.SetVibration(PlayerIndex.Two, rumble, rumble);
                else
                    GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                if (WorldContainer.PlayersJoined[(int)PlayerIndex.Three])
                    GamePad.SetVibration(PlayerIndex.Three, rumble, rumble);
                else
                    GamePad.SetVibration(PlayerIndex.Three, 0, 0);
                if (WorldContainer.PlayersJoined[(int)PlayerIndex.Four])
                    GamePad.SetVibration(PlayerIndex.Four, rumble, rumble);
                else
                    GamePad.SetVibration(PlayerIndex.Four, 0, 0);
            }
            else
            {
                offsetTargetVelocity = Vector2.Zero;
                offsetCameraVelocity = Vector2.Zero;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                GamePad.SetVibration(PlayerIndex.Three, 0, 0);
                GamePad.SetVibration(PlayerIndex.Four, 0, 0);
            }
            //Dampen the standard deviation.
            curStdDev -= curStdDev * stdDevDampingRate * elapsedGameTime;

            //Let the controller (well, two controller's technically) return the camera back to it's centre position.
            //Add the controller's contribution to the camera's velocity. Assume that we're using velocity to affect position.
            Vector2
                cameraControllerSignal = new Vector2( 
                    (float)(pControllerCoefficient * -(offsetCameraPosition.X) * elapsedGameTime), //C_x = c*V_x*dt;
                    (float)(pControllerCoefficient * -(offsetCameraPosition.Y) * elapsedGameTime)), //C_y = c*V_y*dt;
                targetControllerSignal = new Vector2(
                    (float)(pControllerCoefficient * -(offsetTargetPosition.X) * elapsedGameTime), //C_x = c*V_x*dt;
                    (float)(pControllerCoefficient * -(offsetTargetPosition.Y) * elapsedGameTime)); //C_y = c*V_y*dt;

            //Update the positions.
            offsetCameraPosition += cameraControllerSignal;
            offsetTargetPosition += targetControllerSignal;

        }

    }
}
