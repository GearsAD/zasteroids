using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.ExplosionsSmoke;

namespace ZitaAsteria.MenuSystem.World.Ships
{
    /// <summary>
    /// The ship class.
    /// </summary>
    public class ApproachShip : SceneObject
    {
        #region Private fields
        /// <summary>
        /// The boosters.
        /// </summary>
//        PortalUpwardZAPS[] _boosters = null;
        #endregion

        public ApproachShip()
        {
        }

        public override void Initialize()
        {
            //Load the model.
            this.Model = WorldContent.menuSystemContent.ShipApproach;

            //Create the two colours...
            Color outerFlare = new Color(20, 20, 255, 64);
            Color innerFlare = new Color(255, 255, 255, 128);
        }

        public override void Update(GameTime gameTime)
        {
            //booster.Location
            //for (int i = 0; i < 3; i++)
            //{
            //    _boosters[i].Location = this.Location + Vector3.UnitY * 4.0f + Vector3.UnitX * 20.0f + (float)(i - 1) * 7.5f * Vector3.UnitZ;
            //    _boosters[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //    _boosters[3+i].Location = this.Location + Vector3.UnitY * 4.0f + Vector3.UnitX * 20.0f + (float)(i - 1) * 7.5f * Vector3.UnitZ;
            //    _boosters[3+i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //}
        }

    }
}
