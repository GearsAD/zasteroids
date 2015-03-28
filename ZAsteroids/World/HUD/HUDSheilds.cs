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

namespace ZAsteroids.World.HUD
{
    class HUDSheilds : HUDComponent
    {
        public SpriteFont Font { get; set; }

        public Texture2D SheildTexture { get; set; }
        public Texture2D ActiveTexture { get; set; }
        public Texture2D DamageTexture { get; set; }

        private RelativeTexture SheildInfo;
        private RelativeTexture WorkingValue;

        private bool enabled;
        private bool update = false;
        private Vector2 safePositionTopRight;
        private Color color;

        public HUDSheilds()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            SheildTexture = WorldContent.hudContent.sheilds;
            ActiveTexture = WorldContent.hudContent.active;
            DamageTexture = WorldContent.hudContent.damage;

            SheildInfo = new RelativeTexture(SheildTexture);
            SheildInfo.Children.Add("Base01", new RelativeTexture(ActiveTexture) { Position = new Vector2(129, 203), EnableDraw = true });
            SheildInfo.Children.Add("Base02", new RelativeTexture(ActiveTexture) { Position = new Vector2(164.5f, 182.5f), EnableDraw = true });
            SheildInfo.Children.Add("Base03", new RelativeTexture(ActiveTexture) { Position = new Vector2(129.5f, 123), EnableDraw = true });
            SheildInfo.Children.Add("Base04", new RelativeTexture(ActiveTexture) { Position = new Vector2(147, 92.5f), EnableDraw = true });
            SheildInfo.Children.Add("Base05", new RelativeTexture(ActiveTexture) { Position = new Vector2(199.5f, 42), EnableDraw = true });
            SheildInfo.Children.Add("Base06", new RelativeTexture(ActiveTexture) { Position = new Vector2(234.5f, 102), EnableDraw = true });

            SheildInfo.Children.Add("Damage01", new RelativeTexture(DamageTexture) { Position = new Vector2(94.5f, 143) });
            SheildInfo.Children.Add("Damage02", new RelativeTexture(DamageTexture) { Position = new Vector2(112, 153) });
            SheildInfo.Children.Add("Damage03", new RelativeTexture(DamageTexture) { Position = new Vector2(112, 133) });
            SheildInfo.Children.Add("Damage04", new RelativeTexture(DamageTexture) { Position = new Vector2(112, 113) });
            SheildInfo.Children.Add("Damage05", new RelativeTexture(DamageTexture) { Position = new Vector2(129.5f, 143) });
            SheildInfo.Children.Add("Damage06", new RelativeTexture(DamageTexture) { Position = new Vector2(129.5f, 103) });
            SheildInfo.Children.Add("Damage07", new RelativeTexture(DamageTexture) { Position = new Vector2(129.5f, 83) });
            SheildInfo.Children.Add("Damage08", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 173) });
            SheildInfo.Children.Add("Damage09", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 153) });
            SheildInfo.Children.Add("Damage10", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 133) });
            SheildInfo.Children.Add("Damage11", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 113) });
            SheildInfo.Children.Add("Damage12", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 73) });
            SheildInfo.Children.Add("Damage13", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 53) });
            SheildInfo.Children.Add("Damage14", new RelativeTexture(DamageTexture) { Position = new Vector2(147, 12) });
            SheildInfo.Children.Add("Damage15", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 162.5f) });
            SheildInfo.Children.Add("Damage16", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 142.5f) });
            SheildInfo.Children.Add("Damage17", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 122.5f) });
            SheildInfo.Children.Add("Damage18", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 102.5f) });
            SheildInfo.Children.Add("Damage19", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 82.5f) });
            SheildInfo.Children.Add("Damage20", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 62.5f) });
            SheildInfo.Children.Add("Damage21", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 42.5f) });
            SheildInfo.Children.Add("Damage22", new RelativeTexture(DamageTexture) { Position = new Vector2(164.5f, 22.5f) });
            SheildInfo.Children.Add("Damage23", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 173) });
            SheildInfo.Children.Add("Damage24", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 153) });
            SheildInfo.Children.Add("Damage25", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 133) });
            SheildInfo.Children.Add("Damage26", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 113) });
            SheildInfo.Children.Add("Damage27", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 73) });
            SheildInfo.Children.Add("Damage28", new RelativeTexture(DamageTexture) { Position = new Vector2(182.5f, 53) });
            SheildInfo.Children.Add("Damage29", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 62) });
            SheildInfo.Children.Add("Damage30", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 82) });
            SheildInfo.Children.Add("Damage31", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 102) });
            SheildInfo.Children.Add("Damage32", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 122) });
            SheildInfo.Children.Add("Damage33", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 142) });
            SheildInfo.Children.Add("Damage34", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 162) });
            SheildInfo.Children.Add("Damage35", new RelativeTexture(DamageTexture) { Position = new Vector2(199.5f, 182) });
            SheildInfo.Children.Add("Damage36", new RelativeTexture(DamageTexture) { Position = new Vector2(217.5f, 112.5f) });
            SheildInfo.Children.Add("Damage37", new RelativeTexture(DamageTexture) { Position = new Vector2(217.5f, 92.5f) });
            SheildInfo.Children.Add("Damage38", new RelativeTexture(DamageTexture) { Position = new Vector2(217.5f, 72.5f) });
            SheildInfo.Children.Add("Damage39", new RelativeTexture(DamageTexture) { Position = new Vector2(235, 122) });

            SheildInfo.EnableDraw = true;

            SheildInfo.Position = new Vector2(HUDDrawSafeArea.Right - (SheildTexture.Width / 2), HUDDrawSafeArea.Top + (SheildTexture.Height / 2));

            safePositionTopRight = new Vector2(HUDDrawSafeArea.Right, HUDDrawSafeArea.Top);

            Font = WorldContent.fontAL15pt;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            // Fuck this sucks, but doing it at work, so will fix later
            //DUUUUUUUUUDE, holy crap! 10 points for effort :) [GearsAD]
            if (update)
            {
                if (HUDProperties.HealthAmount <= 97.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage32", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage32", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 95.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage39", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage39", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 93.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage02", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage02", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 90.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage38", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage38", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                  
                if (HUDProperties.HealthAmount <= 87.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage03", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage03", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 85.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage37", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage37", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 83.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage04", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage04", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 80.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage36", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage36", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 77.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage05", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage05", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 75.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage35", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage35", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 73.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage06", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage06", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 70.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage34", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage34", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 67.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage07", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage07", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 65.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage33", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage33", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 63.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage22", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage22", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 60.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage01", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage01", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 57.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage09", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage09", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 55.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage31", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage31", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 53.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage10", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage10", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 50.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage30", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage30", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 47.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage11", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage11", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 45.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage29", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage29", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 43.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage12", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage12", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 40.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage28", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage28", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 37.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage13", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage13", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 35.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage27", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage27", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 33.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage14", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage14", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 30.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage26", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage26", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 27.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage15", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage15", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 25.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage25", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage25", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 23.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage16", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage16", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 20.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage24", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage24", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 17.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage17", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage17", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 15.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage23", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage23", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 13.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage18", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage18", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                if (HUDProperties.HealthAmount <= 10.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage08", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage08", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }

                    if (HUDProperties.HealthAmount <= 7.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage19", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage19", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 5.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage21", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage21", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
                    if (HUDProperties.HealthAmount <= 0.0f)
                    {
                        SheildInfo.Children.TryGetValue("Damage20", out WorkingValue);
                        WorkingValue.EnableDraw = true;
                    }
                    else
                    {
                        SheildInfo.Children.TryGetValue("Damage20", out WorkingValue);
                        WorkingValue.EnableDraw = false;
                    }
            }
            update = !update;
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                SheildInfo.Draw(HUDSpriteBatch);

                string health = HUDProperties.HealthAmount.ToString();
                if (HUDProperties.HealthAmount <= 80)
                {
                    color = Color.Orange;
                }
                else if (HUDProperties.HealthAmount <= 40)
                {
                    color = Color.Red;
                }
                else
                {
                    color = WorldContent.hudContent.hudTextColor;
                }
                HUDSpriteBatch.DrawString(Font, health, safePositionTopRight + new Vector2(-SheildTexture.Width + 45, 22), color);

                HUDSpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets whether the component should be drawn.
        /// </summary>
        /// <param name="enabled">enable the component</param>
        public void Enable(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}
