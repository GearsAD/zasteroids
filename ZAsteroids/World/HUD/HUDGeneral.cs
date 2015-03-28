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
    class HUDGeneral : HUDComponent
    {
        public Texture2D GeneralTexture { get; set; }
        public Texture2D EngineTexture { get; set; }
        public Texture2D WeaponReadyTexture { get; set; }
        public Texture2D WeaponChargeTexture { get; set; }
        public Texture2D XRotationTexture { get; set; }
        public Texture2D YRotationTexture { get; set; }
        public Texture2D ZRotationTexture { get; set; }

        private RelativeTexture GeneralInfo;
        private RelativeTexture WorkingValue;

        private bool enabled;

        public HUDGeneral()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            GeneralTexture = WorldContent.hudContent.general;
            EngineTexture = WorldContent.hudContent.engine;
            WeaponReadyTexture = WorldContent.hudContent.weaponReady;
            WeaponChargeTexture = WorldContent.hudContent.weaponCharging;
            XRotationTexture = WorldContent.hudContent.xRotation;
            YRotationTexture = WorldContent.hudContent.yRotation;
            ZRotationTexture = WorldContent.hudContent.zRotation;

            //XRotation = HUDProperties.XRotationDirection;

            GeneralInfo = new RelativeTexture(GeneralTexture);
            GeneralInfo.Children.Add("Engine", new RelativeTexture(EngineTexture) { Position = new Vector2(174, 171), EnableDraw = true, RotateEffect = true });
            GeneralInfo.Children.Add("WeaponReady", new RelativeTexture(WeaponReadyTexture) { Position = new Vector2(201, 53), EnableDraw = true });
            GeneralInfo.Children.Add("WeaponCharging", new RelativeTexture(WeaponChargeTexture) { Position = new Vector2(57, 196), EnableDraw = true });
            GeneralInfo.Children.Add("XRotation", new RelativeTexture(XRotationTexture) { Position = new Vector2(66, 61), EnableDraw = true, RotateEffect = true });
            GeneralInfo.Children.Add("YRotation", new RelativeTexture(YRotationTexture) { Position = new Vector2(66, 61), EnableDraw = true, RotateEffect = true });
            GeneralInfo.Children.Add("ZRotation", new RelativeTexture(ZRotationTexture) { Position = new Vector2(66, 61), EnableDraw = true, RotateEffect = true });

            GeneralInfo.EnableDraw = true;

            // Retative textures are drawn via origin
            GeneralInfo.Position = new Vector2(HUDDrawSafeArea.Left + (GeneralTexture.Width / 2), HUDDrawSafeArea.Top + (GeneralTexture.Height / 2));

            //safeTopLeft = new Vector2(HUDDrawSafeArea.Left, HUDDrawSafeArea.Top);

            //safePositionCenter = new Vector2(HUDDrawSafeArea.Center.X, HUDDrawSafeArea.Center.Y);

            //crosshairCenter = new Vector2(GeneralTexture.Width / 2, GeneralTexture.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            
            GeneralInfo.Children.TryGetValue("Engine", out WorkingValue);
            WorkingValue.RotationDirection = HUDProperties.ThrustDirection;

            GeneralInfo.Children.TryGetValue("XRotation", out WorkingValue);
            WorkingValue.RotationDirection = HUDProperties.XRotationDirection;

            GeneralInfo.Children.TryGetValue("YRotation", out WorkingValue);
            WorkingValue.RotationDirection = HUDProperties.YRotationDirection;

            GeneralInfo.Children.TryGetValue("ZRotation", out WorkingValue);
            WorkingValue.RotationDirection = HUDProperties.ZRotationDirection;

            GeneralInfo.Children.TryGetValue("WeaponReady", out WorkingValue);
            WorkingValue.EnableDraw = HUDProperties.WeaponStatus;

            GeneralInfo.Children.TryGetValue("WeaponCharging", out WorkingValue);
            WorkingValue.EnableDraw = !HUDProperties.WeaponStatus;

            GeneralInfo.Update(gameTime);
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();
                

                //HUDSpriteBatch.Draw(GeneralTexture, safeTopLeft, Color.White);
                GeneralInfo.Draw(HUDSpriteBatch);

                //HUDSpriteBatch.Draw(GeneralTexture, safePositionCenter, null, Color.White, 0.0f, crosshairCenter, 1.0f, SpriteEffects.None, 0.0f);

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
