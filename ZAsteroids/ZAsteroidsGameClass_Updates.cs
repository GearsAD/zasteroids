using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ZitaAsteria.MenuSystem;
using ZAsteroids.World;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the main game class for Pilot Project
    /// </summary>
    public partial class ZAsteroidsGameClass : Microsoft.Xna.Framework.Game
    {
        private int effectIndex = 0;
        KeyboardState _lastKeyState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Global - if the game is not active (focused, pause the whole system).
            if (!this.IsActive)
            {
                return;
            }
            KeyboardState kbState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);

            //Update the menu
            MenuContainer.MenuSystemScene.Update(gameTime);

            // Update Music Manager
            WorldContainer.musicManager.Update(gameTime);

            //Update the weapon.
            if (Weapon != null && MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
            {
                Weapon.Update(gameTime);
                Weapon.CheckInput(kbState);
                Weapon.CheckInput(gamepadState);
            }

            //General collision detection
            CollisionDetectionController.ProcessGeneralCollisionDetection();

            // HUD Stuff
            _general.Update(gameTime);
            _score.Update(gameTime);
            //_state.Update(
            _crosshair.Update(gameTime);
            
            //Update the game state.
            _gameManager.Update(gameTime);
            
            //Update the upgrades.
            _upgradeManager.Update(gameTime);

            //Update the shields and life
            _shields.Update(gameTime);

            _lastKeyState = kbState;
        }

    }
}
