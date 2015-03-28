using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ZitaAsteria;
using ZitaAsteria.MenuSystem;

namespace ZAsteroids.World.SPHEREs
{
    /// <summary>
    /// A single pneumatic actuator.
    /// </summary>
    public class PneumaticActuator
    {
        #region Public Properties
        /// <summary>
        /// The actuator offset.
        /// </summary>
        public Vector3 ActuatorOffset { get; private set; }

        /// <summary>
        /// The linear force (in newtons) applied when the actuator is active - this is not a general force/torque calculation, 
        /// simplified to just assume torque is applied orthogonally and on one axis, and this lateral force is applied toward or 
        /// away from the COG.
        /// </summary>
        public Vector3 LateralForceN { get; private set; }

        /// <summary>
        /// The linear force at present according to the pulse generator.
        /// </summary>
        public Vector3 LateralForceN_Current { get; private set; }

        /// <summary>
        /// This is the rotational force (in newtons) when the actuator is active, applied along the normal plane to ActuatorOffset. 
        /// Note had to be along that normal surface, because the lateral force component isn't taken into account.
        /// </summary>
        public Vector3 RotationalForceN { get; private set; }

        /// <summary>
        /// The rotational force at present according to the pulse generator.
        /// </summary>
        public Vector3 RotationalForceN_Current { get; private set; }

        /// <summary>
        /// Gets the actuator state - if actively applying a force.
        /// </summary>
        public bool IsActuatorCurrentlyActive { get; private set; }
        
        /// <summary>
        /// The frequency of the actuator's pulse train.
        /// </summary>
        public float FrequencyHz { get; private set; }

        /// <summary>
        /// The duty cycle (normalized to [0 ,1]) of the actuator. 
        /// </summary>
        public float DutyCycleNorm { get; set; }

        /// <summary>
        /// The Sphere associated with this actuator.
        /// </summary>
        public SPHERE Sphere { get; private set; }

        /// <summary>
        /// If true then it behaves like a true actuator, pulsed with the fundamental frequency and duty cycle.
        /// </summary>
        public bool IsPulseActuator { get; set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The elapsed time for the driver for the pulse train that affects the forces.
        /// </summary>
        double elapsedTimeS = 0;

        /// <summary>
        /// The actuation sound.
        /// </summary>
        SoundEffectInstance _sfxActuate = null;

        List<ActuatorZAPS> _actuatorZAPS = new List<ActuatorZAPS>();
        #endregion

        public PneumaticActuator(SPHERE sphere, Vector3 actuatorOffset, Vector3 lateralForceN, Vector3 rotationalForceN, float frequencyHz)
        {
            ActuatorOffset = actuatorOffset;
            LateralForceN = lateralForceN;
            RotationalForceN = rotationalForceN;
            FrequencyHz = 5;
            DutyCycleNorm = 0;
            Sphere = sphere;
            IsPulseActuator = true;

            _sfxActuate = WorldContent.sfxContent.SPHEREActuate.CreateInstance();
            _sfxActuate.IsLooped = true;

            //Create the actuators...
            if (lateralForceN != Vector3.Zero) //We have a lateral actuator
            {
                ActuatorZAPS actuator = new ActuatorZAPS(-LateralForceN);
                _actuatorZAPS.Add(actuator);
            }//Otherwise we have a rotational actuator
            {
                ActuatorZAPS
                    actuator1 = new ActuatorZAPS(RotationalForceN),
                    actuator2 = new ActuatorZAPS(-RotationalForceN);
                _actuatorZAPS.Add(actuator1);
                _actuatorZAPS.Add(actuator2);
            }
            //Initialize them.
            for (int i = 0; i < _actuatorZAPS.Count; i++)
            {
                _actuatorZAPS[i].Initialize();
                MenuContainer.ParticleSystemManager.AddParticleSystem(_actuatorZAPS[i]);
            }

        }

        /// <summary>
        /// Update the actuator and apply the force to the system.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            elapsedTimeS += gameTime.ElapsedGameTime.TotalSeconds;
            //Modulo for the fundamental period.
            elapsedTimeS = elapsedTimeS % (1.0 / FrequencyHz);
            if (IsPulseActuator)
            {
                //Calculate whether the actuator is on.
                if (elapsedTimeS * FrequencyHz /*elapsed / periodS*/ < DutyCycleNorm)
                {
                    IsActuatorCurrentlyActive = true;
                    RotationalForceN_Current = CalculateRotationalAcceleration(1.0f);
                    LateralForceN_Current = LateralForceN;
                    //If the sound isn't playing, play it.
                    _sfxActuate.Volume = DutyCycleNorm;
                    if (_sfxActuate.State != SoundState.Playing)
                        _sfxActuate.Play();
                }
                else
                {
                    IsActuatorCurrentlyActive = false;
                    RotationalForceN_Current = Vector3.Zero;
                    LateralForceN_Current = Vector3.Zero;
                    if (_sfxActuate.State != SoundState.Paused)
                        _sfxActuate.Pause();
                }
            }
            else
            {
                IsActuatorCurrentlyActive = DutyCycleNorm != 0;
                RotationalForceN_Current = CalculateRotationalAcceleration(1.0f) * DutyCycleNorm;
                LateralForceN_Current = LateralForceN * DutyCycleNorm;
            }

            //Update the actuators
            for (int i = 0; i < _actuatorZAPS.Count; i++)
            {
                //Emitting state
                _actuatorZAPS[i].IsEmitting = IsActuatorCurrentlyActive;
                _actuatorZAPS[i].Emitter.PositionData.Position = 
                    Sphere.Location + 
                    Vector3.Transform(
                        (float)Math.Pow(-1, i) * ActuatorOffset, 
                        Sphere.ObjectRotation);
                _actuatorZAPS[i].Emitter.OrientationData.Orientation = Sphere.ObjectRotation;
            }
        }

        /// <summary>
        /// Calculate the rotational acceleration using a force scaling term to account for portions of the duty cycle that we use (partially on, etc.)
        /// </summary>
        /// <param name="forceN"></param>
        /// <returns></returns>
        private Vector3 CalculateRotationalAcceleration(float forceScaling)
        {
            return Vector3.Cross(RotationalForceN * forceScaling, ActuatorOffset) / Sphere.Inertia;
        }
    }
}
