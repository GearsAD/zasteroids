using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem;
using ZitaAsteria;
using ZAsteroids.World.HUD;
using ZitaAsteria.Scene;
using Microsoft.Xna.Framework.Input;
using ZAsteroids.World.TransitionControllers;
using ZAsteroids.World.Upgrades;
using ZitaAsteria.World;

namespace ZAsteroids.World
{
    /// <summary>
    /// The central game manager.
    /// </summary>
    public class GameManager
    {
        #region Public Properties
        /// <summary>
        /// The current game state.
        /// </summary>
        public GameStateEnum GameState { get; private set; }

        /// <summary>
        /// Time left in the current round.
        /// </summary>
        public float CurrentRoundTimeLeftS { get; private set; }

        public float RoundTimeAsteroids { get; private set; }
        public float RoundTimeSPHEREs { get; private set; }

        /// <summary>
        /// The parent game class.
        /// </summary>
        public ZAsteroidsGameClass GameClass { get; private set; }

        /// <summary>
        /// The asteroid generator.
        /// </summary>
        public AsteroidGenerator AsteroidGenerator { get; private set; }

        private int _score = 0;
        /// <summary>
        /// The zasteroids score.
        /// </summary>
        public int ScoreZAst
        {
            get { return _score; }
            set
            {
                _score = value;
                //Play a score sound.
                WorldContent.sfxContent.AsteroidHit.Play();
            }
        }

        /// <summary>
        /// The current wave.
        /// </summary>
        public int GameWave { get; set; }

        /// <summary>
        /// The number of available bombs.
        /// </summary>
        public int NumberBombs { get; set; }

        /// <summary>
        /// The number of weapon power upgrades that have been done.
        /// </summary>
        public int NumberWeaponPowerUpgrades { get; set; }

        /// <summary>
        /// The number of weapon range upgrades that have been done.
        /// </summary>
        public int NumberWeaponRangeUpgrades { get; set; }

        /// <summary>
        /// The number of weapon range upgrades that have been done.
        /// </summary>
        public int NumberWeaponRateUpgrades { get; set; }
        
        #endregion

        #region Private Fields
        /// <summary>
        /// The current transition controller.
        /// </summary>
        private TransitionController _currentTransition = null;

        /// <summary>
        /// The last key state.
        /// </summary>
        KeyboardState _lastKeyState;
        /// <summary>
        /// The last gamepad state.
        /// </summary>
        GamePadState _lastGamepadState;
        #endregion

        public GameManager(ZAsteroidsGameClass gameClass)
        {
            RoundTimeAsteroids = 90;
            RoundTimeSPHEREs = 60;
            GameClass = gameClass;
            //Force it to not transition from the sphere outro mode.
            SetState(GameStateEnum.Intro, false, true);
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public void Initialize()
        {
            AsteroidGenerator = new AsteroidGenerator();
            AsteroidGenerator.IsActive = false;
        }

        /// <summary>
        /// Update and manage the states.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            
            //Decrease the round time.
            CurrentRoundTimeLeftS -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (CurrentRoundTimeLeftS < 0)
            {
                if (GameState == GameStateEnum.Intro) SetState(GameStateEnum.AsteroidsMode);
                else
                if (GameState == GameStateEnum.AsteroidsMode) SetState(GameStateEnum.SphereMode);
                else
                if (GameState == GameStateEnum.SphereMode) SetState(GameStateEnum.AsteroidsMode);
            }

            //Check if the player presses enter, for the state to change.
            if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
            {
                if(keys.IsKeyDown(Keys.Enter))
                {
                    //If your SPHERE is still alive, add the time points
                    if(MenuContainer.SpherePlayer.Life > 0)
                        ScoreZAst += (int)(CurrentRoundTimeLeftS * 10);
                    SetState(GameStateEnum.AsteroidsMode);
                }
            }

            if (GameState == GameStateEnum.Intro)
            {
                if (keys.IsKeyDown(Keys.Space))
                    HUDProperties.GameManager.SetState(GameStateEnum.AsteroidsMode, true);
            }

            //Including the help check.
            if (keys.IsKeyUp(Keys.F1) && _lastKeyState.IsKeyDown(Keys.F1))
            {
                HUDProperties.IsHelpShown = !HUDProperties.IsHelpShown;
            }

            //Cheat codes!!!
            if (
                keys.IsKeyUp(Keys.M) && _lastKeyState.IsKeyDown(Keys.M) &&
                keys.IsKeyUp(Keys.I) && _lastKeyState.IsKeyDown(Keys.I) &&
                keys.IsKeyUp(Keys.L) && _lastKeyState.IsKeyDown(Keys.L))
            {
                this.ScoreZAst += 10000;
                //Play sound.
                WorldContainer.soundEffectsMgr.PlaySoundEffect(WorldContent.sfxContent.ChaChing);
            }

            //Update the asteroid generator
            AsteroidGenerator.Update(gameTime);

            //Set the properties.
            HUDProperties.CurrendRoundTimeLeftS = CurrentRoundTimeLeftS;
            HUDProperties.GameState = GameState;

            //Update the transition controller if used.
            if (_currentTransition != null)
                _currentTransition.Update(gameTime);

            _lastKeyState = keys;
            _lastGamepadState = gamepad;
        }

        /// <summary>
        /// Set the game state.
        /// </summary>
        /// <param name="gameState"></param>
        public void SetState(GameStateEnum gameState, bool skipTransition = false, bool skipSound = false)
        {
            GameStateEnum oldState = GameState;
            GameState = gameState;

            //Play a sound.
            if(!skipSound)
                WorldContainer.soundEffectsMgr.PlaySoundEffect(WorldContent.sfxContent.ionCannonLinkNoShoot);

            switch (gameState)
            {
                case (GameStateEnum.Intro): SetIntroMode(oldState); break;
                case (GameStateEnum.AsteroidsMode): if(!skipTransition) SetSPHERESTransitionOutroMode(oldState); else SetSatelliteMode(oldState); break;
                case (GameStateEnum.SphereMode): if (!skipTransition) SetSPHERESTransitionIntroMode(oldState); else SetSPHEREsMode(oldState); break;
            }
        }

        private void SetIntroMode(GameStateEnum oldState)
        {
            //Show intro stuff.
            CurrentRoundTimeLeftS = Int32.MaxValue;
            MenuContainer.MenuSystemScene.SetMenuSceneLocation(MenuSceneLocations.ZAsteroidsIntro);
            //Play the music
            WorldContainer.musicManager.SetBackgroundToNewType(ZitaAsteria.Scene.BackgroundMusicTypes.Loading);
        }

        private void SetSatelliteMode(GameStateEnum oldState)
        {
            Vector3 oldCamStart = Vector3.Zero;
            if(oldState == GameStateEnum.Intro)
            //Turn off intro stuff.
            {
                //Start the asteroids
                AsteroidGenerator.IsActive = true;
            }
            if (oldState == GameStateEnum.SphereMode)
                oldCamStart = MenuContainer.Camera.cameraLocation;

            //Set the scene.
            MenuContainer.MenuSystemScene.SetMenuSceneLocation(MenuSceneLocations.ZAsteroidsSatellite);

            //Set the music.
            WorldContainer.musicManager.SetBackgroundToNewType(ZitaAsteria.Scene.BackgroundMusicTypes.EndLevel);

            //Force the camera back to where the sphere is.
            if (oldState == GameStateEnum.SphereMode)
                MenuContainer.Camera.cameraLocation = oldCamStart;

            //And turn off the shields - muhahaha!
            MenuContainer.Shield.IsDrawing = false;

            CurrentRoundTimeLeftS = RoundTimeAsteroids; //Seconds.

            if (oldState == GameStateEnum.SphereMode) //Round 2
                AsteroidGenerator.SetToNextLevel();

        }

        /// <summary>
        /// Set to transition.
        /// </summary>
        /// <param name="oldState"></param>
        private void SetSPHERESTransitionIntroMode(GameStateEnum oldState)
        {
            CurrentRoundTimeLeftS = 10000;

            MenuContainer.MenuSystemScene.SetMenuSceneLocation(MenuSceneLocations.ZAsteroidsSphereTransition);

            //Set the music.
            WorldContainer.musicManager.SetBackgroundToNewType(ZitaAsteria.Scene.BackgroundMusicTypes.ZAsteroidsDockIntro);

            //Move the sphere back to location.
            MenuContainer.SpherePlayer.Velocity = Vector3.Zero;
            MenuContainer.SpherePlayer.VelocityRotat = Vector3.Zero;
            MenuContainer.SpherePlayer.AccelerationLinear = Vector3.Zero;
            MenuContainer.SpherePlayer.AccelerationRotat = Vector3.Zero;
            MenuContainer.SpherePlayer.Life = 100; //Arbitrary number > 0;
            MenuContainer.SpherePlayer.IsDrawing = true;

            //Put the satellite back into position, facing the sun.
            MenuContainer.Satellite.ObjectRotation = Quaternion.Identity;

            _currentTransition = new SphereIntroController(this);
            _currentTransition.IsCompleted += new EventHandler(_currentTransition_IsCompleted);

            //Clear out intersecting rocks.
            CollisionDetectionController.OnShieldChange_CleanupAsteroids();
            //And turn on the shields - muhahaha!
            MenuContainer.Shield.IsDrawing = true;
        }


        /// <summary>
        /// Set to transition.
        /// </summary>
        /// <param name="oldState"></param>
        private void SetSPHERESTransitionOutroMode(GameStateEnum oldState)
        {
            //Only do this if the sphere is still alive.
            if (MenuContainer.SpherePlayer.Life <= 0) //Skip this transition if you're dead.
            {
                SetState(GameStateEnum.AsteroidsMode, true, false);
                return;
            }

            CurrentRoundTimeLeftS = 10000;

            MenuContainer.MenuSystemScene.SetMenuSceneLocation(MenuSceneLocations.ZAsteroidsSphereTransition);

            //Set the music.
            WorldContainer.musicManager.SetBackgroundToNewType(ZitaAsteria.Scene.BackgroundMusicTypes.ZAsteroidsDockIntro);

            //Move the sphere back to location.
            MenuContainer.SpherePlayer.Velocity = Vector3.Zero;
            MenuContainer.SpherePlayer.VelocityRotat = Vector3.Zero;
            MenuContainer.SpherePlayer.AccelerationLinear = Vector3.Zero;
            MenuContainer.SpherePlayer.AccelerationRotat = Vector3.Zero;
            MenuContainer.SpherePlayer.Life = 100; //Arbitrary number > 0;
            MenuContainer.SpherePlayer.IsDrawing = true;

            _currentTransition = new SphereOutroController(this);
            _currentTransition.IsCompleted += new EventHandler(_currentTransition_IsCompleted);
        }


        private void SetSPHEREsMode(GameStateEnum oldState)
        {
            MenuContainer.MenuSystemScene.SetMenuSceneLocation(MenuSceneLocations.ZAsteroidsSpheres);

            //Set the music.
            WorldContainer.musicManager.SetBackgroundToNewType(ZitaAsteria.Scene.BackgroundMusicTypes.ZAsteroidsDockLoop);

            CurrentRoundTimeLeftS = RoundTimeSPHEREs; //Seconds.
        }

        /// <summary>
        /// Remove the transition reference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _currentTransition_IsCompleted(object sender, EventArgs e)
        {
            if(_currentTransition != null)
                _currentTransition.IsCompleted -= _currentTransition_IsCompleted;
            _currentTransition = null;

            //And set sphere's mode
            if(sender.GetType() == typeof(SphereIntroController))
                SetState(GameStateEnum.SphereMode, true, false);
            if (sender.GetType() == typeof(SphereOutroController))
                SetState(GameStateEnum.AsteroidsMode, true, false);
        }
    }
}
