using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.Common.Controllers;
using ZAsteroids.World.SPHEREs;
using Microsoft.Xna.Framework;

namespace ZAsteroids.World.SPHEREs
{
    /// <summary>
    /// A simple PID-based attitude+position controller for a SPHERE.
    /// </summary>
    public class SphereAttLocController
    {
        #region Public Properties
        /// <summary>
        /// The location controllers.
        /// </summary>
        public PIDController[] LocationControllers { get; private set; }
        /// <summary>
        /// The attitude controllers.
        /// </summary>
        public PIDController[] AttitudeControllers { get; private set; }

        /// <summary>
        /// The sphere under control.
        /// </summary>
        public SPHERE Sphere { get; set; }

        /// <summary>
        /// The intermediate absolute attitude setpoint.
        /// </summary>
        public Vector3 AttitudeSetpointAbs { get; private set; }
        #endregion

        /// <summary>
        /// Initialize and create the controllers.
        /// </summary>
        public SphereAttLocController(SPHERE sphere)
        {
            Sphere = sphere;
            LocationControllers = new PIDController[3];
            AttitudeControllers = new PIDController[3];
            for (int i = 0; i < 3; i++)
            {
                LocationControllers[i] = new PIDController();
                LocationControllers[i].InputLimits = new Vector2(-1f, 1f);
                LocationControllers[i].OutputLimits = new Vector2(-1, 1);
                LocationControllers[i].DerivativeAlpha = 1.5f;
                LocationControllers[i].Kp = 0.1f;//.1f;
                LocationControllers[i].Ki = 0;
                LocationControllers[i].Kd = 1.0f;
                AttitudeControllers[i] = new PIDController();
            }
        }

        /// <summary>
        /// Update the controllers.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Vector3
                locationDutyCycles = Vector3.Zero;
            locationDutyCycles.X = LocationControllers[0].CalculateOutput(gameTime, Sphere.Location.X);
            locationDutyCycles.Y = LocationControllers[1].CalculateOutput(gameTime, Sphere.Location.Y);
            locationDutyCycles.Z = LocationControllers[2].CalculateOutput(gameTime, Sphere.Location.Z);
            
            //Reorient this to the current ship orientation
            locationDutyCycles = Vector3.Transform(locationDutyCycles, Sphere.ObjectRotation);
            //Right, now apply them in sequence
            if (locationDutyCycles.X > 0) 
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegX].DutyCycleNorm = 0;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosX].DutyCycleNorm = locationDutyCycles.X;
            }
            else if (locationDutyCycles.X < 0) 
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegX].DutyCycleNorm = -locationDutyCycles.X;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosX].DutyCycleNorm = 0;
            }
            if (locationDutyCycles.Y > 0)
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegY].DutyCycleNorm = 0;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosY].DutyCycleNorm = locationDutyCycles.Y;
            }
            else if (locationDutyCycles.Y < 0)
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegY].DutyCycleNorm = -locationDutyCycles.Y;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosX].DutyCycleNorm = 0;
            }
            if (locationDutyCycles.Z > 0)
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegZ].DutyCycleNorm = 0;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosZ].DutyCycleNorm = locationDutyCycles.Z;
            }
            else if (locationDutyCycles.Z < 0)
            {
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.NegZ].DutyCycleNorm = -locationDutyCycles.Z;
                Sphere.Actuators[(int)LinearActuatorSequenceEnum.PosZ].DutyCycleNorm = 0;
            }
        }

        /// <summary>
        /// Set the position setpoint.
        /// </summary>
        /// <param name="setpoint"></param>
        public void SetPositionSetpoint(Vector3 setpoint)
        {
            LocationControllers[0].Setpoint = setpoint.X;
            LocationControllers[1].Setpoint = setpoint.Y;
            LocationControllers[2].Setpoint = setpoint.Z;
        }

        /// <summary>
        /// Set the attitude setpoint (in absolute radians [0, 2pi))!
        /// </summary>
        /// <param name="setpoint"></param>
        public void SetAttitudeSetpoint(Vector3 setpoint)
        {
            //This will be transformed later.
            AttitudeSetpointAbs = setpoint;
        }
    }
}
