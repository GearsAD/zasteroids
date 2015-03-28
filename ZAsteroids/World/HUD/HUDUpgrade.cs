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
using ZAsteroids.World.Upgrades;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Scene;
using ZAsteroids.World.HUD.UpgradeMarkers;
//using ZitaAsteria.ContentSections;

namespace ZAsteroids.World.HUD
{
    class HUDUpgrade : HUDComponent
    {
        public Texture2D UpgradeTexture { get; set; }
        public Texture2D UpgradeTextureGlow { get; set; }
        /// <summary>
        /// A simple line util for draw the lines.
        /// </summary>
        private HUDSimpleLineUtil _hudSimpleLineUtil;

        private RelativeTexture UpgradeInfoTex;

        /// <summary>
        /// An upgrade marker.
        /// </summary>
        private UpgradeMarker _upgradeMarkerModel = null;

        private bool enabled;

        public HUDUpgrade()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            UpgradeTexture = WorldContent.hudContent.upgradeTexture;
            UpgradeTextureGlow = WorldContent.hudContent.upgradeTextureGlow;

            UpgradeInfoTex = new RelativeTexture(UpgradeTextureGlow);
            UpgradeInfoTex.Children.Add("MainTexture", new RelativeTexture(UpgradeTexture) { Position = new Vector2(64, 64), EnableDraw = true });

            //Load the upgrade marker model.
            _upgradeMarkerModel = new UpgradeMarker();
            _upgradeMarkerModel.Initialize();

            //Initialize the line
            _hudSimpleLineUtil = new HUDSimpleLineUtil();
            _hudSimpleLineUtil.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        /// <summary>
        /// Special method for the upgrades - draw the 3D components of the markers.
        /// Really a special case but the easiest way to handle it.
        /// </summary>
        public void Draw3D()
        {
            if (enabled)
            {
                //Draw the upgrade marker
                for (int i = 0; i < HUDProperties.UpgradeManager.Upgrades.Length; i++)
                {
                    UpgradeInfo info = HUDProperties.UpgradeManager.Upgrades[i];
                    //If active...
                    if (info.IsEnabled)
                    {
                        //Backface culling.
                        Vector3 toHint = info.BoundingSphere.Center - MenuContainer.Camera.cameraLocation;
                        Vector3 cameraForward = Vector3.Transform(Vector3.UnitZ, (MenuContainer.Camera as ZAsteroidsSPHERECamera).Orientation);
                        if (Vector3.Dot(toHint, cameraForward) > 0)
                        {
                            //Draw the marker sphere model.
                            _upgradeMarkerModel.Location = info.BoundingSphere.Center;
                            _upgradeMarkerModel.Scale = info.BoundingSphere.Radius * Vector3.One;
                            _upgradeMarkerModel.Draw();
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            if (enabled)
            {
                HUDSpriteBatch.Begin();

                //Draw the upgrade info's
                for(int i = 0; i < HUDProperties.UpgradeManager.Upgrades.Length; i++)
                {
                    UpgradeInfo info = HUDProperties.UpgradeManager.Upgrades[i];
                    //If active...
                    if (info.IsEnabled)
                    {
                        Vector2 uiPos = new Vector2(
                            HUDDrawSafeArea.Right - 64,
                            HUDDrawSafeArea.Top + (i + 1) * 96);

                        Vector2 hintPos = GetScreenLocation(info.BoundingSphere.Center);
                        Texture2D hint = ZitaAsteria.WorldContent.hudContent.upgradeMarker;

                        //Backface culling.
                        Vector3 toHint = info.BoundingSphere.Center - MenuContainer.Camera.cameraLocation;
                        toHint.Normalize();
                        Vector3 cameraForward = Vector3.Transform(Vector3.UnitZ, (MenuContainer.Camera as ZAsteroidsSPHERECamera).Orientation);
                        if (Vector3.Dot(toHint, cameraForward) > 0.25f) //Make it relatively stringent
                        {
                            //Draw the line.
                            // Line stuff
                            //Calculate the cross product
                            Vector2
                                lineCross = uiPos - hintPos;
                            lineCross = new Vector2(-lineCross.Y, lineCross.X);
                            lineCross.Normalize();
                            Vector2
                                lineFromTopLeft = uiPos - new Vector2(16, 0),
                                lineFromBottomLeft = lineFromTopLeft + new Vector2(0, 32),
                                lineTo1 = hintPos + 36.0f * lineCross,
                                lineTo2 = hintPos - 36.0f * lineCross;
                            _hudSimpleLineUtil.DrawLine(WorldContent.hudContent.hudUpgradeLineColor, 1, lineFromBottomLeft, lineTo1);
                            _hudSimpleLineUtil.DrawLine(WorldContent.hudContent.hudUpgradeLineColor, 1, lineFromTopLeft, lineTo2);
                            
                            //Draw the text.
                            UpgradeInfoTex.Position = new Vector2(uiPos.X + 32, uiPos.Y + 16);
                            UpgradeInfoTex.Draw(HUDSpriteBatch);
                            HUDSpriteBatch.DrawString(
                                WorldContent.fontArial10pt,
                                info.UpgradeName + "\r\n" +
                                info.Cost.ToString(),
                                uiPos,
                                Color.White);

                            HUDSpriteBatch.Draw(hint, hintPos, null, Color.White, 0, new Vector2(hint.Width / 2, hint.Height / 2), 1.0f, SpriteEffects.None, 0);
                            string distance = (info.BoundingSphere.Center - MenuContainer.SpherePlayer.Location).Length().ToString() + "m";
                            HUDSpriteBatch.DrawString(
                                WorldContent.fontArial10pt,
                                distance,
                                hintPos + new Vector2(68, -102),
                                WorldContent.hudContent.hudUpgradeLineColor);
                        }

                        //Draw the text for the upgrade if you're in range.
                        if (info.IsInRange)
                        {
                            string toWrite = "";
                            if (HUDProperties.Score >= info.Cost)
                                toWrite = "Press [Space] to Upgrade " + info.UpgradeName + " for " + info.Cost + " Points!";
                            else
                                toWrite = "Require Additional " + (info.Cost - HUDProperties.Score).ToString() + " Points to Upgrade " + info.UpgradeName + "!";
                            HUDSpriteBatch.DrawString(
                                WorldContent.fontAL28pt,
                                toWrite,
                                new Vector2(this.HUDDrawSafeArea.Center.X - WorldContent.fontAL28pt.MeasureString(toWrite).X / 2.0f, this.HUDDrawSafeArea.Center.Y),
                                WorldContent.hudContent.hudTextColor);
                        }
                    }
                }

                HUDSpriteBatch.End();
            }
        }

        /// <summary>
        /// This method converts the world coordinates of this object to screen coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScreenLocation(Vector3 loc)
        {
            //Now convert it to a screen Location.
            Vector3 screenLocation =
                WorldContainer.graphicsDevice.Viewport.Project(
                    loc,
                    MenuContainer.Camera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport),
                    MenuContainer.Camera.GetViewMatrix(),
                    Matrix.Identity);

            return new Vector2(screenLocation.X, screenLocation.Y);
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
