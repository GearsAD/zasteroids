using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.MenuSystem;
using Microsoft.Xna.Framework;
using ZAsteroids;
using ZAsteroids.World;
using ZAsteroids.World.Weapons;

using ZitaAsteria.MenuSystem;
using ZitaAsteria;
using ZAsteroids.World.HUD;
using Microsoft.Xna.Framework.Input;


namespace ZAsteroids.World.Upgrades
{
    public class UpgradeManager
    {
        #region Public Properties
        /// <summary>
        /// The upgrades.
        /// </summary>
        public UpgradeInfo[] Upgrades { get; private set; }

        #endregion

        #region Static data
        /// <summary>
        /// The upgrade locations on the satellite.
        /// FireRate = 0,
        /// ShipLife,
        /// WeaponDamage,
        /// WeaponUpgrade,
        /// Bomb
        /// </summary>
        private static Vector3[] _upgradeLocationsOnSat = new Vector3[] 
            { 
                new Vector3(0.0f, -33.0f, 67.0f),       // FireRate
                new Vector3(453.0f, 0.0f, 0.0f),        // ShipLife
                new Vector3(0.0f, -100.0f, -20.0f), /*new Vector3(0.0f, 51.0f, 99.0f),        // WeaponDamage*/
                new Vector3(0.0f, 100.0f, -20.0f),      // WeaponRange
                new Vector3(0.0f, -100.0f, -20.0f),     // Bomb
            };

        private KeyboardState _lastKey;
        private GamePadState _lastGamepad;
        #endregion

        #region Private Fields
        GameManager _gameManager = null;
        #endregion

        public UpgradeManager(GameManager gameManager)
        {
            _gameManager = gameManager;

            Upgrades = new UpgradeInfo[4];

            //Set the upgrades
            for (int i = 0; i < Upgrades.Length; i++)
            {
                BoundingSphere sphere = new BoundingSphere();
                sphere.Center = MenuContainer.Satellite.Location + _upgradeLocationsOnSat[i];

                sphere.Radius = 10;
                Upgrades[i] = new UpgradeInfo(sphere, (UpgradesEnum)i, (UpgradesEnum)i + "", 500 * (i + 1));
                Upgrades[i].IsEnabled = true;
                //Get the projection of the hint onto the display.
            }
            //Turn off the bomb 
            //Upgrades[4].IsEnabled = false;
        }

        /// <summary>
        /// Check the upgrades.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Set whether they can be used.
            for (int i = 0; i < Upgrades.Length; i++)
            {
                if (MenuContainer.SpherePlayer.IsCollidingWith(Upgrades[i].BoundingSphere) && Upgrades[i].IsEnabled)
                {
                    Upgrades[i].IsInRange = true;
                }
                else
                    Upgrades[i].IsInRange = false;
            }

            //Handle upgrades.
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gameState = GamePad.GetState(PlayerIndex.One);

            //Check if key pressed.
            if(
                (keyState.IsKeyUp(Keys.Space) && _lastKey.IsKeyDown(Keys.Space)) ||
                (gameState.IsConnected && gameState.IsButtonUp(Buttons.X) && _lastGamepad.IsButtonDown(Buttons.X)))
            {
                for(int i = 0; i < Upgrades.Length; i++)
                {
                    if(Upgrades[i].IsInRange && _gameManager.ScoreZAst >= Upgrades[i].Cost)
                    {
                        //Do the upgrade.
                        switch(i)
                        {
                            case(0): UpgradeWeaponFireRate(); break;
                            case(1): UpgradeShipLife(); break;
                            case(2): UpgradeWeaponDamage(); break;
                            case(3): UpgradeWeaponRange(); break;
                            case(4): UpgradeBomb(); break;
                        }
                        _gameManager.ScoreZAst -= Upgrades[i].Cost;
                        //Play sound.
                        WorldContainer.soundEffectsMgr.PlaySoundEffect(WorldContent.sfxContent.ChaChing);
                    }
                }
            }

            //Set up last states.
            _lastKey = keyState;
            _lastGamepad = gameState;
        }

        public void UpgradeWeaponFireRate()
        {
            _gameManager.GameClass.Weapon.UpgradeRateOfFire();
            //Tell the game manager
            _gameManager.NumberWeaponRateUpgrades++;

        }

        public void UpgradeShipLife()
        {
            MenuContainer.Satellite.LifeMax = MenuContainer.Satellite.LifeMax + 50;
            MenuContainer.Satellite.Life = MenuContainer.Satellite.LifeMax;
        }

        public void UpgradeWeaponDamage()
        {
            _gameManager.GameClass.Weapon.UpgradeDamage();
            //Tell the game manager
            _gameManager.NumberWeaponPowerUpgrades++;
        }

        public void UpgradeWeaponRange()
        {
            _gameManager.GameClass.Weapon.UpgradeRange();
            //Tell the game manager
            _gameManager.NumberWeaponRangeUpgrades++;
        }

        public void UpgradeBomb()
        {
            //_gameManager.GameClass.Weapon.UpgradeRateOfFire();
        }
    }
}
