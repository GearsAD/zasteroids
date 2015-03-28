using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared_Content;
using Microsoft.Xna.Framework.Audio;

namespace ZitaAsteria.ContentSections
{

    /// <summary>
    /// A static class containing references to all the SFX content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public class SFXContent
    {
        // Effect for NightVision Visual Effect
        public SoundEffect NightVisionSoundEffect { get; private set; }

        // Submarine Alarm
        public SoundEffect submarineAlarmSoundEffect { get; private set; }

        // AssaultRifle Shot
        public SoundEffect AssaultRifleShot { get; set; }

        // AssaultRifle Shot
        public SoundEffect PistolShot { get; set; }

        // Uzi Shot
        public SoundEffect MachinegunShot { get; set; }

        // Minigun windup Shot
        public SoundEffect MinigunWindup { get; set; }

        // Missile Shot
        public SoundEffect MissileShot { get; set; }

        // Cocking the gun
        public SoundEffect GunCock { get; set; }

        // Debris Explosion
        public SoundEffect DebrisExplosion { get; set; }

        // MiniGun Shot
        public SoundEffect MiniGunShot { get; set; }

        // MiniGun Shot
        public SoundEffect MiniGunShot2 { get; set; }

        // MiniGun Shot
        public SoundEffect MiniGunWindup { get; set; }

        // MiniGun Shot
        public SoundEffect RocketMiniGunWindup { get; set; }
        
        // Ricochet1 Effect
        public SoundEffect Ricochet1 { get; set; }

        // HUD ItemChange Effect
        public SoundEffect HUDItemChange { get; set; }

        // HUD Open Effect
        public SoundEffect AsteroidHit { get; set; }

        // HUD Select Effect
        public SoundEffect HUDSelect { get; set; }

        // Construction - Building Complete Effect
        public SoundEffect BuildingComplete { get; set; }

        // Construction - Building Placement Effect
        public SoundEffect BuildingPlacement { get; set; }

        // Turret Move Effect
        public SoundEffect TurretMove { get; set; }

        // Map Failure Effect
        public SoundEffect MapFailure { get; set; }

        // Map Failure Effect
        public SoundEffect ManDying { get; set; }

        // A scuttlebug dying...
        public SoundEffect ScuttlebugDie { get; set; }

        // A scuttlebug spawning...
        public SoundEffect ScuttlebugSpawn { get; set; }

        // A big scuttlebug dying...
        public SoundEffect BigScuttlebugDie { get; set; }

        // A big scuttlebug spawning...
        public SoundEffect BigScuttlebugSpawn { get; set; }

        // General Chatter Effect
        public SoundEffect GeneralChatter { get; set; }

        // Gravel footsteps effect
        public SoundEffect GravelFootSteps { get; set; }

        // Panic Scream effect
        public SoundEffect PanicScream { get; set; }

        // Laser Select effect
        public SoundEffect LaserSelect { get; set; }

        // LaserShoot effect
        public SoundEffect LaserShoot { get; set; }

        // Railgun effect
        public SoundEffect RailgunShoot { get; set; }

        // Particlegun effect
        public SoundEffect ParticleGunShoot { get; set; }

        // PainterSelect effect
        public SoundEffect PainterSelect { get; set; }

        // MenuClick effect
        public SoundEffect MenuClick { get; set; }
        // MenuClick effect
        public SoundEffect SPHEREActuate { get; set; }

        //ShockExplosion
        public SoundEffect ShockExplosion { get; set; }

        //ShockExplosion
        public SoundEffect ShockExplosionInSpace { get; set; }

        //IncendiaryExplosion
        public SoundEffect IncendiaryExplosion { get; set; }

        //IncendiaryExplosion
        public SoundEffect IncendiaryExplosionInSpace { get; set; }

        /// <summary>
        /// Cash sound effect - http://soundbible.com/1997-Cha-Ching-Register.html
        /// 
        /// </summary>
        public SoundEffect ChaChing { get; set; }

        //RPG Launch
        public SoundEffect RocketLaunch { get; set; }

        //Reload Launch
        public SoundEffect reload { get; set; }

        //ShotGun Fire
        public SoundEffect ShotgunFire { get; set; }

        //ShotGun Fire
        public SoundEffect emptyGunFire { get; set; }

        //Char Voices
        public SoundEffect charOutOfAmmo { get; set; }

        //Kreest
        public SoundEffect KreestRoar { get; set; }

        //Shockwave
        public SoundEffect ShockWave { get; set; }

        //IonCannonLink shoot
        public SoundEffect ionCannonLinkShoot { get; set; }

        //IonCannonLink no shoot
        public SoundEffect ionCannonLinkNoShoot { get; set; }

        //IonCannon shoot
        public SoundEffect ionCannonShoot { get; set; }

        //IonCannon shoot (used in ZAsteroids)
        public SoundEffect ionCannonShootShort { get; set; }


        //IonCannonLink
        public SoundEffect mineShoot { get; set; }
        public SoundEffect mineActivate { get; set; }
        public SoundEffect mineDeactivate { get; set; }

        //IonCannonLink
        public SoundEffect grenadeShoot { get; set; }

        //IonCannonLink
        public SoundEffect electricSound { get; set; }

        public SoundEffect GodModePlayer { get; set; }

        public SFXContent()
        { }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        public void InitializeFromContent(ContentManager gameContentManager)
        {
            /*************************************************************************************************************/
            //Explosions
            ShockExplosion = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\ShockExplosion");
            ShockExplosionInSpace = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\ShockExplosionInSpace");
            DebrisExplosion = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\DebrisExplosion");
            IncendiaryExplosion = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\IncendiaryExplosion");
            IncendiaryExplosionInSpace = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\IncendiaryExplosionInSpace");

            ionCannonLinkNoShoot = gameContentManager.Load<SoundEffect>("SoundEffects\\Weapons\\IonCannonLinkNoShoot");
            ionCannonShootShort = gameContentManager.Load<SoundEffect>("SoundEffects\\Weapons\\IonCannonShootShort");

            AsteroidHit = gameContentManager.Load<SoundEffect>("SoundEffects\\Explosions\\AsteroidHit");

            // Menu FX
            MenuClick = gameContentManager.Load<SoundEffect>("SoundEffects\\MenuSoundFX\\MenuClick");
            //SPHERE steam
            SPHEREActuate = gameContentManager.Load<SoundEffect>("SoundEffects\\MenuSoundFX\\SphereSteam");
        }
    }
}
