using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared_Content;

namespace ZitaAsteria.ContentSections
{

    /// <summary>
    /// A static class containing references to all the item content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public class ItemContent
    {
        //Items
        public Texture2D airStrike { get; protected set; }
        public Texture2D ammo { get; protected set; }
        public Texture2D armourLarge { get; protected set; }
        public Texture2D armourSmall { get; protected set; }
        public Texture2D bioHazard { get; protected set; }
        public Texture2D destructor { get; protected set; }
        public Texture2D flamer { get; protected set; }
        public Texture2D gluonGun { get; protected set; }
        public Texture2D railGun { get; protected set; }
        public Texture2D grenade { get; protected set; }
        public Texture2D healthLarge { get; protected set; }
        public Texture2D healthMedium { get; protected set; }
        public Texture2D healthSmall { get; protected set; }
        public Texture2D ionCannon { get; protected set; }
        public Texture2D laser { get; protected set; }
        public Texture2D machineGun { get; protected set; }
        public Texture2D mine { get; protected set; }
        public Texture2D miniGun { get; protected set; }
        public Texture2D nuke { get; protected set; }
        public Texture2D painter { get; protected set; }
        public Texture2D particleGun { get; protected set; }
        public Texture2D pistol { get; protected set; }
        public Texture2D rifle { get; protected set; }
        public Texture2D rpg { get; protected set; }
        public Texture2D shotGun { get; protected set; }
        public Texture2D smartDestructor { get; protected set; }
        public Texture2D smartMinigun { get; protected set; }
        public Texture2D smartRifle { get; protected set; }
        public Texture2D smartRPG { get; protected set; }
        public Texture2D smartShotGun { get; protected set; }
        public Texture2D zar100k { get; protected set; }
        public Texture2D zar10k { get; protected set; }
        public Texture2D zar1M { get; protected set; }
        public Texture2D zar20k { get; protected set; }
        public Texture2D zar50k { get; protected set; }

        /// <summary>
        /// The ammo crate.
        /// </summary>
        public Model ammoCrate { get; protected set; }

        public ItemContent()
        {
        }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        public void InitializeFromContent(ContentManager gameContentManager)
        {
            //Load the items
            airStrike = gameContentManager.Load<Texture2D>("Weapons\\Items\\Airstrike");
            ammo = gameContentManager.Load<Texture2D>("Weapons\\Items\\Ammo");
            armourLarge = gameContentManager.Load<Texture2D>("Weapons\\Items\\Armour Large");
            armourSmall = gameContentManager.Load<Texture2D>("Weapons\\Items\\Armour Small");
            bioHazard = gameContentManager.Load<Texture2D>("Weapons\\Items\\Biohazard");
            destructor = gameContentManager.Load<Texture2D>("Weapons\\Items\\Destructor");
            flamer = gameContentManager.Load<Texture2D>("Weapons\\Items\\Flamer");
            gluonGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\Gluon Gun");
            railGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\Railgun");
            grenade = gameContentManager.Load<Texture2D>("Weapons\\Items\\Grenade");
            healthLarge = gameContentManager.Load<Texture2D>("Weapons\\Items\\Health Large");
            healthMedium = gameContentManager.Load<Texture2D>("Weapons\\Items\\Health Medium");
            healthSmall = gameContentManager.Load<Texture2D>("Weapons\\Items\\Health Small");
            ionCannon = gameContentManager.Load<Texture2D>("Weapons\\Items\\ION Cannon");
            laser = gameContentManager.Load<Texture2D>("Weapons\\Items\\Laser");
            machineGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\machinegun");
            mine = gameContentManager.Load<Texture2D>("Weapons\\Items\\Mine");
            miniGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\Minigun");
            nuke = gameContentManager.Load<Texture2D>("Weapons\\Items\\Nuke");
            painter = gameContentManager.Load<Texture2D>("Weapons\\Items\\Painter");
            particleGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\Particle Gun");
            pistol = gameContentManager.Load<Texture2D>("Weapons\\Items\\Pistol");
            rifle = gameContentManager.Load<Texture2D>("Weapons\\Items\\Rifle");
            rpg = gameContentManager.Load<Texture2D>("Weapons\\Items\\RPG");
            shotGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\Shotgun");
            smartDestructor = gameContentManager.Load<Texture2D>("Weapons\\Items\\SMART Destructor");
            smartMinigun = gameContentManager.Load<Texture2D>("Weapons\\Items\\SMART Minigun");
            smartRifle = gameContentManager.Load<Texture2D>("Weapons\\Items\\SMART Rifle");
            smartRPG = gameContentManager.Load<Texture2D>("Weapons\\Items\\SMART RPG");
            smartShotGun = gameContentManager.Load<Texture2D>("Weapons\\Items\\SMART Shotgun");
            zar100k = gameContentManager.Load<Texture2D>("Weapons\\Items\\ZAR 100k");
            zar10k = gameContentManager.Load<Texture2D>("Weapons\\Items\\ZAR 10k");
            zar1M = gameContentManager.Load<Texture2D>("Weapons\\Items\\ZAR 1M");
            zar20k = gameContentManager.Load<Texture2D>("Weapons\\Items\\ZAR 20k");
            zar50k = gameContentManager.Load<Texture2D>("Weapons\\Items\\ZAR 50k");

            ammoCrate = gameContentManager.Load<Model>("Weapons\\Items\\Crate_ammo_kw");
        }
    }
}
