using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World.Ships
{
    /// <summary>
    /// The ship class.
    /// </summary>
    public class OrbitingShip : SceneObject
    {
        #region Private fields
        #endregion

        public OrbitingShip()
        {
        }

        public override void Initialize()
        {
            //Load the model.
            this.Model = WorldContent.menuSystemContent.ShipOrbiting;
        }

        public override void Update(GameTime gameTime)
        {
        }

    }
}
