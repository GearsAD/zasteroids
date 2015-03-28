using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZAsteroids.World.SPHEREs;
using Microsoft.Xna.Framework;

namespace ZAsteroids.World.SPHEREs
{
    public class SPHERE_Controlled : SPHERE
    {
        #region Public Properties
        /// <summary>
        /// The SPHERE controller.
        /// </summary>
        SphereAttLocController _controller = null;
        #endregion

        public SPHERE_Controlled()
            : base()
        {
            _controller = new SphereAttLocController(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Update the controller.
            _controller.Update(gameTime);
        }

        /// <summary>
        /// Set the position setpoint.
        /// </summary>
        /// <param name="target"></param>
        public void SetPositionSetpoint(Vector3 target)
        {
            _controller.SetPositionSetpoint(target);
        }

        /// <summary>
        /// Set the absolute attitude setpoint (in radians [0, 2pi))!
        /// </summary>
        /// <param name="target"></param>
        public void SetAttitudeSetpoint(Vector3 target)
        {
            _controller.SetAttitudeSetpoint(target);
        }
    }
}
