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

namespace ZAsteroids.World.HUD
{
    public class HUDSphere : HUDComponent
    {
        public SpriteFont Font { get; set; }

        private Vector2 safePositionBottomLeft;

        private string fmt;

        private bool enabled;

        public HUDSphere()
        {
        }

        public override void Initialize()
        {
            // Must initialize base to get safe draw area
            base.Initialize();

            Font = WorldContent.fontAL18pt;
            
            safePositionBottomLeft = new Vector2(HUDDrawSafeArea.Left, HUDDrawSafeArea.Bottom);

            fmt = "00.00";
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

                if (MenuContainer.SpherePlayer.Life > 0) //We're alive...
                {
                    Vector3
                        localLinAcc = Vector3.Transform(MenuContainer.SpherePlayer.Velocity, MenuContainer.SpherePlayer.ObjectRotation),
                        localRotAcc = MenuContainer.SpherePlayer.VelocityRotat;

                    string output =
                        (MenuContainer.SpherePlayer.IsCollidingWith(MenuContainer.Satellite) ? "---COLLISION---\n" : "") +
                        "Velocities (Localbody)\n" +
                        "  Linear   : " + localLinAcc.X.ToString(fmt) + ", " + localLinAcc.Y.ToString(fmt) + ", " + localLinAcc.Z.ToString(fmt) + "\n" +
                        "  Rotation : " + localRotAcc.X.ToString(fmt) + ", " + localRotAcc.Y.ToString(fmt) + ", " + localRotAcc.Z.ToString(fmt) + "\n" +
                        "\n" +
                        "Damping " + (MenuContainer.SpherePlayer.IsUsingDamping ? "Enabled" : "Disabled");

                    safePositionBottomLeft = new Vector2(HUDDrawSafeArea.Left, HUDDrawSafeArea.Bottom - Font.MeasureString(output).Y - 30);

                    HUDSpriteBatch.DrawString(Font, output, safePositionBottomLeft, WorldContent.hudContent.hudTextColor);
                }
                else //We're dead, draw the dead string.
                {
                    string dead = "...SPHERE Comms Signal Lost...";
                    HUDSpriteBatch.DrawString(WorldContent.fontAL28pt, dead, new Vector2(HUDDrawSafeArea.Center.X - WorldContent.fontAL28pt.MeasureString(dead).X / 2, HUDDrawSafeArea.Center.Y), WorldContent.hudContent.hudTextColor);
                }
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
