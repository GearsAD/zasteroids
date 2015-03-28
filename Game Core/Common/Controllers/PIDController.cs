using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.Common.Controllers
{
    /// <summary>
    /// A straightfoward PID controller. [GearsAD]
    /// </summary>
    public class PIDController
    {
        #region Public Properties
        /// <summary>
        /// The proportional coefficient.
        /// </summary>
        public float Kp { get; set; }
        /// <summary>
        /// The integral coefficient.
        /// </summary>
        public float Ki { get; set; }
        /// <summary>
        /// The derivative coefficient.
        /// </summary>
        public float Kd { get; set; }

        /// <summary>
        /// The high integration saturation limit on the controller, in normalized integration.
        /// </summary>
        public float SaturationHighLimit { get; set; }
        /// <summary>
        /// The low integration saturation limit on the controller, in normalized integration.
        /// </summary>
        public float SaturationLowLimit { get; set; }

        /// <summary>
        /// The limits of the input - X is low and Y is high.
        /// </summary>
        public Vector2 InputLimits { get; set; }
        /// <summary>
        /// The limits of the output - X is low and Y is high.
        /// </summary>
        public Vector2 OutputLimits { get; set; }

        /// <summary>
        /// The current input.
        /// </summary>
        public float CurrentInput { get; private set; }
        /// <summary>
        /// The current error.
        /// </summary>
        public float CurrentError { get; private set; }
        /// <summary>
        /// The current setpoint / reference / demand -> effectively where you want to end up. In the same scale as CurrentInput.
        /// </summary>
        public float Setpoint { get; set; }
        /// <summary>
        /// The current output.
        /// </summary>
        public float CurrentOutput { get; private set; }

        /// <summary>
        /// The coefficient of the first order filter on the error derivative, because the controller moves sporadically through time (no fixed sampling period),
        /// this is an adapted filter of the form x(t) = x(t-1) + DerivativeTau * (x(t) - x(t-1)) * dt, so it's normalized by time, Suggested approximate range of [0; 1].
        /// </summary>
        public float DerivativeAlpha { get; set; }

        /// <summary>
        /// If true then the input is limited to [-1; 1] after scaling.
        /// </summary>
        public bool IsInputLimited { get; set; }

        /// <summary>
        /// If true then the output is limited to [-1; 1] before scaling.
        /// </summary>
        public bool IsOutputLimited { get; set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The rolling integration term.
        /// </summary>
        private float _integral = 0;
        /// <summary>
        /// The last error for the derivative component.
        /// </summary>
        private float _lastError = 0;
        /// <summary>
        /// The current filtered derivative, initializes if equal to -1000, so please don't start there lol. [GearsAD].
        /// </summary>
        private float _derivativeFiltered = -1000;
        #endregion

        public PIDController()
        {
            Kp = 1;
            Ki = 0;
            Kd = 0;
            SaturationHighLimit = 100;
            SaturationLowLimit = -100;
            InputLimits = OutputLimits = Vector2.UnitY;
            DerivativeAlpha = 1;
            IsInputLimited = true;
            IsOutputLimited = true;
        }

        /// <summary>
        /// Reset the integration in the controller.
        /// </summary>
        public void ResetIntegration()
        {
            _integral = 0;
        }

        /// <summary>
        /// Calculate the output (
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public float CalculateOutput(GameTime gameTime, float input)
        {
            //Provide the diagnostic values.
            CurrentInput = input;
            CurrentError = Setpoint - CurrentInput;

            float error = ScaleInput(Setpoint - input, InputLimits) - 0.5f;

            //Calculate the error.

            //Calculate the proportional component.
            float proportional = Kp * error;

            //Calculate the integral component.
            _integral += (float)(error * gameTime.ElapsedGameTime.TotalSeconds);
            if (_integral > SaturationHighLimit) _integral = SaturationHighLimit;
            if (_integral < SaturationLowLimit) _integral = SaturationLowLimit;

            //Calculate the derivative component.
            if(_derivativeFiltered == -1000) //Initializing
                _lastError = error;
            float derivative = (float)((error - _lastError) / gameTime.ElapsedGameTime.TotalSeconds);
            //if(_derivativeFiltered == -1000) //Initializing
           // {
                _derivativeFiltered = derivative;
            //}else
            //{
            //    float possibleDerivativeFiltered = _derivativeFiltered + (float)((derivative - _derivativeFiltered) * DerivativeAlpha * gameTime.ElapsedGameTime.TotalSeconds);
            //    if (!float.IsNaN(possibleDerivativeFiltered) && !float.IsInfinity(possibleDerivativeFiltered))
            //        _derivativeFiltered = possibleDerivativeFiltered;
            //}
            if (float.IsNaN(_derivativeFiltered)) _derivativeFiltered = 0; 

            //So now, just add the components...
            float output = proportional + Ki * _integral + Kd * _derivativeFiltered;
            CurrentOutput = UnscaleOutput(output, OutputLimits);

            return CurrentOutput;
        }

        /// <summary>
        /// Return a scaled value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="limits"></param>
        /// <returns></returns>
        private float ScaleInput(float val, Vector2 limits)
        {
            float scaled = (val - limits.X) / (limits.Y - limits.X);
            if(IsInputLimited)
            {
                if(scaled > 1.0) scaled = 1.0f;
                if(scaled < -1.0) scaled = -1.0f;
            }
            return scaled;
        }

        /// <summary>
        /// Return an unscaled value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="limits"></param>
        /// <returns></returns>
        private float UnscaleOutput(float val, Vector2 limits)
        {
            float unscaled = val;
            if (unscaled < -1) unscaled = -1;
            if (unscaled > 1) unscaled = 1;
            unscaled = unscaled / 2.0f;
            return unscaled * (limits.Y - limits.X) /*+ limits.X*/;
        }
    }
}
