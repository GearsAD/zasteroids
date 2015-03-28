using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared_Content;

namespace ZitaAsteria.ContentSections // SharedContent.ContentSections - Whats going on here? [Alucard]
{
    public class HUDContent
    {
        // HUD textures
        public Texture2D active { get; set; }
        public Texture2D crosshair { get; set; }
        public Texture2D sheilds { get; set; }
        public Texture2D general { get; set; }
        public Texture2D engine { get; set; }
        public Texture2D fire { get; set; }
        public Texture2D weaponChargeRotation { get; set; }
        public Texture2D weaponReady { get; set; }
        public Texture2D weaponCharging { get; set; }
        public Texture2D xRotation { get; set; }
        public Texture2D yRotation { get; set; }
        public Texture2D zRotation { get; set; }
        public Texture2D damage { get; set; }
        public Texture2D damageEffect01 { get; set; }
        public Texture2D damageEffect02 { get; set; }
        public Texture2D damageEffect03 { get; set; }
        public Texture2D upgradeTexture { get; set; }
        public Texture2D upgradeTextureGlow { get; set; }
        public Texture2D upgradeMarker { get; set; }
        public Texture2D upgradeMarkerGlow { get; set; }
        public Texture2D logo { get; set; }
        public Texture2D help { get; set; }

        /// <summary>
        /// The upgrade marker model
        /// </summary>
        public Model upgradeMarkerModel { get; set; }

        // HUD Colors
        public Color hudComponenetsColor { get; set; }
        public Color hudUpgradeLineColor { get; set; }
        public Color hudTextColor { get; set; }

        public HUDContent()
        {
        }

        public void InitializeFromContent(ContentManager gameContentManager)
        {
            active = gameContentManager.Load<Texture2D>("HUD\\Active");
            crosshair = gameContentManager.Load<Texture2D>("HUD\\Crosshair");
            sheilds = gameContentManager.Load<Texture2D>("HUD\\Sheilds");
            general = gameContentManager.Load<Texture2D>("HUD\\General");
            engine = gameContentManager.Load<Texture2D>("HUD\\Engine");
            fire = gameContentManager.Load<Texture2D>("HUD\\FireHint");
            weaponChargeRotation = gameContentManager.Load<Texture2D>("HUD\\WeaponChargeRotation");
            weaponReady = gameContentManager.Load<Texture2D>("HUD\\WeaponReady");
            weaponCharging = gameContentManager.Load<Texture2D>("HUD\\WeaponCharging");
            xRotation = gameContentManager.Load<Texture2D>("HUD\\XRotation");
            yRotation = gameContentManager.Load<Texture2D>("HUD\\YRotation");
            zRotation = gameContentManager.Load<Texture2D>("HUD\\ZRotation");
            damage = gameContentManager.Load<Texture2D>("HUD\\Damage");
            damageEffect01 = gameContentManager.Load<Texture2D>("HUD\\DamageEffect01");
            damageEffect02 = gameContentManager.Load<Texture2D>("HUD\\DamageEffect02");
            damageEffect03 = gameContentManager.Load<Texture2D>("HUD\\DamageEffect03");
            upgradeTexture = gameContentManager.Load<Texture2D>("HUD\\UpgradeTexture");
            upgradeTextureGlow = gameContentManager.Load<Texture2D>("HUD\\UpgradeTextureGlow");
            upgradeMarker = gameContentManager.Load<Texture2D>("HUD\\MarkerInfo01");
            upgradeMarkerGlow = gameContentManager.Load<Texture2D>("HUD\\GlowMarker01");
            upgradeMarkerModel = gameContentManager.Load<Model>("HUD\\UpgradeMarkers\\Marker");
            logo = gameContentManager.Load<Texture2D>("HUD\\Logo");
            help = gameContentManager.Load<Texture2D>("HUD\\HelpTexture");


            hudComponenetsColor = new Color(10, 143, 135);
            hudUpgradeLineColor = new Color(49, 53, 213, 255);
            hudTextColor = new Color(255, 250, 233);
        }
    }
}
