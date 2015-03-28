using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using ZitaAsteria.World.Weapons;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Scene;

namespace ZAsteroids.World.Weapons
{
    /// <summary>
    /// This is a generic weapon class - the character class can carry one of these...
    /// </summary>
    public abstract class ZAstWeapon 
    {
        /// <summary>
        /// The last time the weapon was fired (in milliseconds).
        /// </summary>
        private double firingElapsedMS { get; set; }

        /// <summary>
        /// The time when the weapon reloading was started.
        /// </summary>
        private double reloadingElapsedMS { get; set; }

        /// <summary>
        /// The current ammunition for the gun.
        /// </summary>
        ///*[Browsable(true)]*/
        [Category("Weapon Characteristics")]
        [Description("The firing rate of the weapon (in milliseconds).")]
        public int totalAmmunition { get; set; }

        /// <summary>
        /// The current ammunition in the clip.
        /// </summary>
        //[NonSerialized]
        ///*[Browsable(false)]*/
        public int currentClipAmmunition;

        /// <summary>
        /// If true then the weapon is currently selected.
        /// </summary>
        protected bool IsWeaponSelected { get; private set; }

        /// <summary>
        /// If the round for a single shot weapon has been fired.
        /// </summary>
        bool singleShotFired = false;
        protected WeaponStateEnum weaponState;
        protected bool _isSingleShotWeapon;
        protected double _firingRateMS;
        protected int _ammunitionPerShot;
        protected double _reloadingRateMS;
        protected bool _infiniteAmmo;
        protected int _ammunitionInAClip;
        protected int _maxTotalAmmunition;

        /// <summary>
        /// The weapon reticle.
        /// </summary>
        [ContentSerializerIgnore]
        [XmlIgnore]
        protected Texture2D weaponReticle { get; set; }

        //#region Weapon Events
        ///// <summary>
        ///// Fired when the weapon's trigger is pulled.
        ///// </summary>
        //public event EventHandler<WeaponEventArgs> WeaponTriggerPulled;
        ///// <summary>
        ///// Fired when the weapon's trigger is released.
        ///// </summary>
        //public event EventHandler<WeaponEventArgs> WeaponTriggerReleased;
        ///// <summary>
        ///// Fired when the weapon is actually releasing a shot.
        ///// </summary>
        //public event EventHandler<WeaponEventArgs> WeaponSingleShotFired;
        //#endregion

        public ZAstWeapon()
        {
        }

        /// <summary>
        /// Initialize the gun - no ammo etc.
        /// </summary>
        public virtual void Initialize()
        {

            //Start it idle.
            weaponState = WeaponStateEnum.Idle;
        }

        /// <summary>
        /// Upgrade the damage.
        /// </summary>
        public abstract void UpgradeDamage();
        /// <summary>
        /// Upgrade the rate of fire.
        /// </summary>
        public abstract void UpgradeRateOfFire();
        /// <summary>
        /// Upgrade the weapon range.
        /// </summary>
        public abstract void UpgradeRange();

        /// <summary>
        /// Update the weapon and the firing states.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //Update the weapon's reticle.
            //if (weaponReticle != null)
            //    weaponReticle.Update(gameTime);

            //If the weapon is firing then update the firing cycle
            if (!_isSingleShotWeapon)
            {
                firingElapsedMS += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (weaponState == WeaponStateEnum.Firing)
                {
                    while (firingElapsedMS >= _firingRateMS) //Then fire the weapon again...
                    {
                        if (currentClipAmmunition > 0)
                        {
                            if (Fire()) //Fire the weapon
                            {
                                currentClipAmmunition -= _ammunitionPerShot; //Subtract one from the ammunition.
                                if (currentClipAmmunition < 0) currentClipAmmunition = 0;

                                ////Trigger the reticle...
                                //if (weaponReticle != null)
                                //    weaponReticle.TriggerFired();

                                //Inform event handlers that the weapon was fired.
                                //if (WeaponSingleShotFired != null)
                                //    WeaponSingleShotFired(this, new WeaponEventArgs(this));

                            }//If didn't fire, don't subtract ammo.
                        }
                        else
                        {
                            EmptyFire();
                        }

                        //Subtract the time for the current shot.
                        firingElapsedMS -= _firingRateMS;
                    }

                }
            }

            //If the weapon is reloading then update the reloading cycle
            if (weaponState == WeaponStateEnum.Reloading)
            {
                reloadingElapsedMS += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (reloadingElapsedMS >= _reloadingRateMS) //The weapon is reloaded, set it back to idle.
                {
                    PerformReload();
                    weaponState = WeaponStateEnum.Idle;
                }
            }

            //During the update set the satellite orientation to be that of the camera.
            if(MenuContainer.Camera.GetType() == typeof(ZAsteroidsSatelliteCamera))
                MenuContainer.Satellite.ObjectRotation = (MenuContainer.Camera as ZAsteroidsSatelliteCamera).Orientation;
        }  // end of update(gameTime)


        /// <summary>
        /// The weapon's trigger is pulled - set the gun to firing mode if it's not reloading.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public virtual void PullTrigger()
        {
            if (weaponState == WeaponStateEnum.Idle)
            {
                weaponState = WeaponStateEnum.Firing;
                //Fire the TriggerPulled Event.
                if (currentClipAmmunition > 0)
                    //if (WeaponTriggerPulled != null)
                    //    WeaponTriggerPulled(this, new WeaponEventArgs(this));

                if (firingElapsedMS > _firingRateMS)
                    firingElapsedMS = _firingRateMS;

                //And start the timer.
                //firingElapsedMS = firingRateMS;
                if (_isSingleShotWeapon && !singleShotFired)
                {
                    if (currentClipAmmunition > 0)
                    {
                        if (Fire()) //Fire the weapon
                        {
                            currentClipAmmunition -= _ammunitionPerShot; //Subtract one from the ammunition.
                            if (currentClipAmmunition < 0) currentClipAmmunition = 0;

                            ////Trigger the reticle...
                            //if (weaponReticle != null)
                            //    weaponReticle.TriggerFired();

                            //if (WeaponSingleShotFired != null)
                            //    WeaponSingleShotFired(this, new WeaponEventArgs(this));
                        }
                    }
                    else  //If didn't fire, don't subtract ammo.
                    {
                        EmptyFire();
                    }

                    singleShotFired = true;
                }
            }
        }

        /// <summary>
        /// The weapon's trigger is released - set the gun to stop firing if it's not reloading.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void ReleaseTrigger()
        {
            if (weaponState == WeaponStateEnum.Firing)
            {
                weaponState = WeaponStateEnum.Idle;

                //if (WeaponTriggerReleased != null)
                //    WeaponTriggerReleased(this, new WeaponEventArgs(this));
            }

            singleShotFired = false;
        }


        /// <summary>
        /// This requests that the weapon be reloaded.
        /// </summary>
        public void RequestReload(bool playSound)
        {

            //If the gun is currently firing, stop it.
            if (weaponState == WeaponStateEnum.Firing)
            {
                ReleaseTrigger();
            }

            if (totalAmmunition <= 0 && !_infiniteAmmo)
            {
                //Play an out of ammo sound...
                //if (playSound)
                //{
                //    SoundEffectInstance outOfAmmoInstance = WorldContent.sfxContent.charOutOfAmmo.CreateInstance();
                //    outOfAmmoInstance.Play();
                //}
                return;
            }

            //If the gun is currently reloading, then don't do anything.
            if (weaponState == WeaponStateEnum.Reloading)
            {
                return;
            }

            //Play the sound effect of reloading...
            if (playSound)
            {
                //SoundEffectInstance instance = WorldContent.sfxContent.reload.CreateInstance();
                //instance.Play();
            }

            //Set the weapon to the reloading state and start the timer.
            weaponState = WeaponStateEnum.Reloading;
            reloadingElapsedMS = 0;

        }

        /// <summary>
        /// This is the actual event that takes place when the weapon is reloaded. This can be overridden by children turretWeapons.
        /// </summary>
        protected virtual void PerformReload()
        {
            int _ammoNeededForClip = _ammunitionInAClip - currentClipAmmunition;

            if (totalAmmunition >= _ammoNeededForClip || _infiniteAmmo)
            {
                currentClipAmmunition = _ammunitionInAClip;
                if (!_infiniteAmmo)
                    totalAmmunition -= _ammoNeededForClip;
            }
            else //Haven't got a full clip!
            {
                currentClipAmmunition = totalAmmunition;
                totalAmmunition = 0;
            }
        }

        /// <summary>
        /// Add a clip to this weapon, can be overridden. Returns true if successful.
        /// </summary>
        public virtual bool AddAmmoFromAmmoItem()
        {
            //Add a single clip for now.
            if (totalAmmunition >= _maxTotalAmmunition) return false;

            //If infinite ammo
            if (_infiniteAmmo) return false;

            //Otherwise add a clip...
            totalAmmunition += _ammunitionInAClip;
            if (totalAmmunition > _maxTotalAmmunition)
                totalAmmunition = _maxTotalAmmunition;
            return true;
        }

        /// <summary>
        /// Actually fire the gun, this is called if the trigger is pulled and enough time has passed. This must be overridden by a child class.
        /// </summary>
        /// <param name="inheritedVelocity">The inherited velocity of the projectile if the weapon is fired</param>
        protected virtual bool Fire()
        {
            return true;
        }

        /// <summary>
        /// What happens when the gun is fired and is out of ammunition (the current clip).
        /// </summary>
        protected virtual void EmptyFire()
        {
            //Play the sound effect of reloading...
            //SoundEffectInstance instance = WorldContent.sfxContent.emptyGunFire.CreateInstance();
            //instance.Play();
        }

        /// <summary>
        /// The Player class will pass this down to the weapon. This is where the gamepad input for the weapon is checked.
        /// </summary>
        /// <param name="keyboardstate"></param>
        public virtual void CheckInput(GamePadState gamePadState)
        {
        }

        /// <summary>
        /// The Player class will pass this down to the weapon. This is where the keyboard input for the weapon is checked.
        /// </summary>
        /// <param name="keyboardstate"></param>
        public virtual void CheckInput(KeyboardState keyState)
        {
        }

        /// <summary>
        /// Draw the reticle.
        /// </summary>
        public virtual void Draw2D()
        {
            if (weaponReticle != null)
            {
                //Todo
                //weaponReticle.Draw2D();
            }
        }

        /// <summary>
        /// Return the name of the weapon - used in the critter tester.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }

    }
}
