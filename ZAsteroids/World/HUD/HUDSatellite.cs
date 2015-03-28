using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using ZitaAsteria;
using ZitaAsteria.World;
using ZitaAsteria.MenuSystem;
using ZAsteroids.World.Weapons;

namespace ZAsteroids.World.HUD
{
    public class HUDSatellite : HUDComponent
    {
        public SpriteFont Font { get; set; }

        private Vector2 safePositionBottomLeft;

        private bool enabled;

        public HUDSatellite()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            Font = WorldContent.fontAL18pt;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                string output =
                    "CCP Laser\n" +
                    "  Damage      : " + (HUDProperties.GameManager.GameClass.Weapon as LaserWeapon).DamageFactor + (HUDProperties.GameManager.NumberWeaponPowerUpgrades > 0 ? " (+" + HUDProperties.GameManager.NumberWeaponPowerUpgrades + ")" : "") + "\n" +
                    "  Charge Rate : " + (HUDProperties.GameManager.GameClass.Weapon as LaserWeapon).FiringRateMS + "ms" + (HUDProperties.GameManager.NumberWeaponRateUpgrades > 0 ? " (+" + HUDProperties.GameManager.NumberWeaponRateUpgrades + ")" : "") +"\n" +
                    "  Max Range   : " + (HUDProperties.GameManager.GameClass.Weapon as LaserWeapon).MaxRange + (HUDProperties.GameManager.NumberWeaponRangeUpgrades > 0 ? " (+" + HUDProperties.GameManager.NumberWeaponRangeUpgrades + ")" : "");

                safePositionBottomLeft = new Vector2(HUDDrawSafeArea.Left, HUDDrawSafeArea.Bottom - Font.MeasureString(output).Y - 30);

                HUDSpriteBatch.DrawString(Font, output, safePositionBottomLeft, WorldContent.hudContent.hudTextColor);

                HUDSpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets whether the component should be drawn.
        /// </summary>
        /// <param name="enabled">Enable the component</param>
        public void Enable(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}
