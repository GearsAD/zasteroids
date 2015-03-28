using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.MenuSystem;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.Screen_Effects;
using ZitaAsteria.Scene;

namespace ZAsteroids.World.TransitionControllers
{
    /// <summary>
    /// Outro scene for the sphere controller.
    /// </summary>
    class SphereOutroController : TransitionController
    {
        #region Private fields
        /// <summary>
        /// Facing the sun.
        /// </summary>
        private bool _isDoingFirstOrientation = true;

        private Vector3
            _startOrientation = new Vector3(3.141f /2.0f, 0, 3.141f),
            _endOrientation = new Vector3(0, 0, 3.141f);
        #endregion

        public SphereOutroController(GameManager gameManager) : base(gameManager)
        {
            //No need to call SetupScene - it's done in the base class.

            _isPossibleToSkip = true;
        }

        /// <summary>
        /// Set up the scene.
        /// </summary>
        public override void SetupInitialScene()
        {
            //Put the sphere in the right location + attitude.
            MenuContainer.SpherePlayer.Location = MenuContainer.Satellite.Location + new Vector3(0, 110, -17);
            MenuContainer.SpherePlayer.ObjectRotation = Quaternion.CreateFromYawPitchRoll(_startOrientation.Y, _startOrientation.X, _startOrientation.Z);
            MenuContainer.SpherePlayer.Velocity = Vector3.Zero;
            Vector3 delta = _endOrientation - _startOrientation;
            MenuContainer.SpherePlayer.VelocityRotat = delta / 11.0f;
            MenuContainer.SpherePlayer.IsCollidable = false;

            //Set the camera offset so that it's looking down the dock.
            (MenuContainer.Camera as ZAsteroidsSPHERECamera).CameraOffsetFromSphere = new Vector3(0, -1f, -15);

            //Add a fade-from black filter to the shaders...
            FadeInOutEffect fadein = new FadeInOutEffect();
            fadein.UsedInMenuContainer = true; //IMPORTANT!
            fadein.Initialize();
            fadein.SetPredefinedType(5.0f, PredefinedFadeEffect.BlackFadeIn);
            MenuContainer.CompoundEffects.Add(fadein);
        }

        //Wait until the sphere is out of the dock
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_elapsedTransitionTimeS > 11)
            {
                _isDoingFirstOrientation = false;
                MenuContainer.SpherePlayer.VelocityRotat = 0.25f * Vector3.UnitY;
                MenuContainer.SpherePlayer.Velocity = -2.0f * Vector3.UnitY; //Set the speed - need to get to 120m.
            }
            else
            {
                Vector3 delta = _endOrientation - _startOrientation;
                MenuContainer.SpherePlayer.VelocityRotat = delta / 11.0f;
            }

            //If far enough end the scene.
            if (MenuContainer.SpherePlayer.Location.Y < MenuContainer.Satellite.Location.Y + 74)
                SetupEndOfScene();
}

        /// <summary>
        /// Setup the SPHERE.
        /// </summary>
        public override void SetupEndOfScene()
        {
            MenuContainer.SpherePlayer.Location = MenuContainer.Satellite.Location + new Vector3(0, 0, -17);
            MenuContainer.SpherePlayer.Velocity = -2.0f * Vector3.UnitY; //Set the speed - need to get to 120m.
//            MenuContainer.SpherePlayer.IsCollidable = true;

            base.SetupEndOfScene();
        }
    }
}
