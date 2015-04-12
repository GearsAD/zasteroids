using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZAsteroids.World.Weapons.Laser;
using ZitaAsteria.MenuSystem;
using Microsoft.Xna.Framework;
using ZitaAsteria;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using ZAsteroids.World.Effects.ExplosionSmoke;
using ZitaAsteria.MenuSystem.World.Satellite;
using ZAsteroids.World.HUD;

namespace ZAsteroids.World.Weapons
{
    /// <summary>
    /// Standard laser weapon.
    /// </summary>
    public class LaserWeapon : ZAstWeapon
    {
        #region Public Properties
        /// <summary>
        /// The cached list of intersections to set the targeting without actually doing the calculation twice.
        /// </summary>
        public List<RayIntersectionInfo> CachedIntersectionsList { get; private set; }

        /// <summary>
        /// Is ready to fire.
        /// </summary>
        public bool IsReadyToFire { get { return _fireEmitLeftMS <= 0; } }

        /// <summary>
        /// Will hit an object if fired.
        /// </summary>
        public bool IsObjectHitWhenFired { get { return (CachedIntersectionsList != null ? CachedIntersectionsList.Count > 0 : false); } }

        /// <summary>
        /// The number of periphery rays - related to damage.
        /// </summary>
        public int NumPeripheryRays { get; private set; }

        /// <summary>
        /// The amount of damage applied to an asteroid in range.
        /// </summary>
        public int DamageFactor { get; private set; }

        /// <summary>
        /// Get the firing rate.
        /// </summary>
        public float FiringRateMS { get { return (float)_firingRateMS; } }

        /// <summary>
        /// The max range of the ray, falls off linearly after that.
        /// </summary>
        public float MaxRange { get; private set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The ray effect.
        /// </summary>
        LaserACE _raySystem = null;
        /// <summary>
        /// Lifetime left for the emitter.
        /// </summary>
        float _fireEmitLeftMS = 0;

        /// <summary>
        /// Weapon charge time.
        /// </summary>
        float _ChargeTime = 900;
        public float ChargeTime
        {
            get
            {
                return _ChargeTime;
            }
            set
            {
                _ChargeTime = MathHelper.Clamp(value, 300, 900);
            }
        }
        #endregion

        /// <summary>
        /// Set up the initial properties.
        /// </summary>
        public LaserWeapon()
        {
            this._ammunitionInAClip = 20;
            this.currentClipAmmunition = this._ammunitionInAClip;
            this._ammunitionPerShot = 0;
            this._firingRateMS = 1000f;
            this._infiniteAmmo = true;
            this._isSingleShotWeapon = false;
            this._maxTotalAmmunition = 1000;
            this._reloadingRateMS = 1000;
            this.DamageFactor = 100;
            MaxRange = 600;

            this.NumPeripheryRays = 1;

            _raySystem = new LaserACE(MenuContainer.Satellite.ObjectRotation, NumPeripheryRays);
            _raySystem.Initialize();
            _raySystem.AddChildrenToMenuContainer();
            MenuContainer.CompoundEffects.Add(_raySystem);
            _raySystem.Location3D = MenuContainer.Satellite.Location;
        }

        public override void CheckInput(KeyboardState keyState)
        {
            base.CheckInput(keyState);
            //if space is pressed, fire the weapon.
            if (keyState.IsKeyDown(Keys.Space))
                PullTrigger();
            else
                ReleaseTrigger();
        }

        public override void CheckInput(GamePadState gamePadState)
        {
            base.CheckInput(gamePadState);
            if(gamePadState.IsConnected)
                if (gamePadState.Triggers.Right > 0.5f)
                    PullTrigger();
                else
                    ReleaseTrigger();
        }

        /// <summary>
        /// Fire the laser.
        /// </summary>
        /// <returns></returns>
        protected override bool Fire()
        {
            _raySystem.PointingVector = MenuContainer.Satellite.ObjectRotation;
            _raySystem.Reset();
            _raySystem.EnableSystems();
            _fireEmitLeftMS = 300;
            //Play the sound
            SoundEffectInstance instance = WorldContent.sfxContent.ionCannonShootShort.CreateInstance();
            instance.Play();

            //Do the collision effect.
            if(CachedIntersectionsList != null)
                CollisionDetectionController.ProcessRayIntersectionList(MenuContainer.Satellite.Location, CachedIntersectionsList, DamageFactor, MaxRange);

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //_raySystem.Location = MenuContainer.Satellite.Location;

            //Update the weapon emission.
            _fireEmitLeftMS -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_fireEmitLeftMS < 0)
            {
                _fireEmitLeftMS = 0;
                _raySystem.DisableSystems();
            }

            //Do a collision detection test of this.
            Vector3 direction = Vector3.UnitZ;
            direction = Vector3.Transform(direction, GetOrientationQuaterion());
            direction.Normalize();
            //Cache the list of intersections so that they can be used by the tracer on the hud.
            CachedIntersectionsList = CollisionDetectionController.GetIntersectionRayObjects(new Ray(MenuContainer.Satellite.Location, direction));

            //And set the satellite's targeting state with this.
            MenuContainer.Satellite.SetTargetingState(CachedIntersectionsList.Count > 0 ? SatelliteTargetingStateEnum.WillHit : SatelliteTargetingStateEnum.NoHit);

            //HUD stuff *sigh*
            HUDProperties.WeaponStatus = IsReadyToFire;
            HUDProperties.FireHintStatis = IsObjectHitWhenFired;
        }

        public override void UpgradeDamage()
        {
            //Upgrade the damage factor.
            DamageFactor += 50;

            this.NumPeripheryRays++;

            _raySystem.DeleteChildrenFromWorldContainer();
            //Create a new one.
            _raySystem = new LaserACE(MenuContainer.Satellite.ObjectRotation, NumPeripheryRays);
            _raySystem.Initialize();
            _raySystem.AddChildrenToMenuContainer();
            MenuContainer.CompoundEffects.Add(_raySystem);
            _raySystem.Location3D = MenuContainer.Satellite.Location;

        }

        public override void UpgradeRateOfFire()
        {
            this._firingRateMS -= 100f;
            if (_firingRateMS < 300) _firingRateMS = 300;
        }

        public override void UpgradeRange()
        {
            this.MaxRange += 150;
        }

        /// <summary>
        /// Get the orientation quaternion for the satellite.
        /// </summary>
        /// <returns></returns>
        public Quaternion GetOrientationQuaterion()
        {
            return MenuContainer.Satellite.ObjectRotation;
        }
    }
}
