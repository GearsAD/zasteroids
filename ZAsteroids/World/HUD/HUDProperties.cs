using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Scene;
using ZAsteroids.World.Upgrades;

namespace ZAsteroids.World.HUD
{
    /// <summary>
    /// A static class that all HUD components look at
    /// </summary>
    public static class HUDProperties
    {
        public static GameManager GameManager
        {
            get;
            set;
        }

        /// <summary>
        /// The current player's score.
        /// </summary>
        public static float Score
        {
            get
            {
                return GameManager.ScoreZAst;
            }
        }

        /// <summary>
        /// The current wave.
        /// </summary>
        public static int Wave
        {
            get { return GameManager.GameWave; }
        }

        // A value from -1 to 1
        public static float XRotationDirection
        {
            get
            {
                float val = 0;
                if(MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
                    val = (MenuContainer.Camera as ZAsteroidsSatelliteCamera).OrientationVelocityAxes[1].CurrentValue * 2.0f * (float)Math.PI;
                else if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
                    val = MenuContainer.SpherePlayer.VelocityRotat.X;
                return val;
            }
        }

        // A value from -1 to 1
        public static float YRotationDirection
        {
            get
            {
                float val = 0;
                if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
                    val = -(MenuContainer.Camera as ZAsteroidsSatelliteCamera).OrientationVelocityAxes[0].CurrentValue * 2.0f * (float)Math.PI;
                else if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
                    val = MenuContainer.SpherePlayer.VelocityRotat.Y;
                return val;
            }
        }

        // A value from -1 to 1
        public static float ZRotationDirection
        {
            get
            {
                float val = 0;
                if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
                    val = (MenuContainer.Camera as ZAsteroidsSatelliteCamera).OrientationVelocityAxes[2].CurrentValue * 2.0f * (float)Math.PI;
                else if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
                    val = MenuContainer.SpherePlayer.VelocityRotat.Z;
                return val;
            }

        }

        // A value from -1 to 1
        private static float thrustDirection = 0;
        public static float ThrustDirection
        {
            get
            {
                return thrustDirection;
            }
            set
            {
                thrustDirection = MathHelper.Clamp(value, -1, 1);
            }
        }

        /// <summary>
        /// Returns the players current health as a percentage.
        /// </summary>
        public static float HealthAmount
        {
            get
            {
                return MenuContainer.Satellite.LifePercent;
            }
        }

        /// <summary>
        /// If true then show the help.
        /// </summary>
        public static bool IsHelpShown { get; set; }

        /// <summary>
        /// Control for whether lines are drawn based on culling.
        /// </summary>
        public static bool[] DrawLines { get; set; }

        /// <summary>
        /// Not bound to the weapon as we can't pass it to the satellite. Updated in game class.
        /// </summary>
        public static bool WeaponStatus {get; set;}

        /// <summary>
        /// Not bound to the weapon as we can't pass it to the satellite. Updated in game class.
        /// </summary>
        public static bool FireHintStatis { get; set; }

        /// <summary>
        /// The current game state.
        /// </summary>
        public static GameStateEnum GameState { get; set; }

        /// <summary>
        /// The time left in the current round.
        /// </summary>
        public static float CurrendRoundTimeLeftS { get; set; }

        /// <summary>
        /// The current upgrade manager.
        /// </summary>
        public static UpgradeManager UpgradeManager { get; set; }
    }
}
