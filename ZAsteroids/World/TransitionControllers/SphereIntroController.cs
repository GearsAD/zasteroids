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
    /// Intro scene for the sphere controller.
    /// </summary>
    class SphereIntroController : TransitionController
    {
        public SphereIntroController(GameManager gameManager) : base(gameManager)
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
            MenuContainer.SpherePlayer.Location = MenuContainer.Satellite.Location + new Vector3(0, 54, -17);
            MenuContainer.SpherePlayer.ObjectRotation = Quaternion.CreateFromYawPitchRoll(0, 0, 3.141f ); //Face it up and outward
            MenuContainer.SpherePlayer.Velocity = 2.0f * Vector3.UnitY; //Set the speed - need to get to 120m.
            MenuContainer.SpherePlayer.IsCollidable = false;

            //Set the camera offset so that it's looking down the dock.
            (MenuContainer.Camera as ZAsteroidsSPHERECamera).CameraOffsetFromSphere = new Vector3(0, 1.5f, -3);

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

            //If far enough end the scene.
            if (MenuContainer.SpherePlayer.Location.Y >= MenuContainer.Satellite.Location.Y + 110)
                SetupEndOfScene();

            //Force the speed.
            MenuContainer.SpherePlayer.Velocity = 2.0f * Vector3.UnitY; //Set the speed - need to get to 120m.
            MenuContainer.SpherePlayer.VelocityRotat = 0.25f * Vector3.UnitY;
}

        //Setup the SPHERE.
        public override void SetupEndOfScene()
        {
            MenuContainer.SpherePlayer.Location = MenuContainer.Satellite.Location + new Vector3(0, 120, -17);
            MenuContainer.SpherePlayer.Velocity = 2.0f * Vector3.UnitY; //Set the speed - need to get to 120m.
            MenuContainer.SpherePlayer.IsCollidable = true;

            base.SetupEndOfScene();
        }
    }
}
