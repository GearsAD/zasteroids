using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World
{
    /// <summary>
    /// The stars class.
    /// </summary>
    public class Stars : SceneObject
    {
        #region Private fields
        #endregion
        
        public Stars()
        {
            IsUsingLighting = false;
        }

        public override void Initialize()
        {
            //Load the model.
            this.Model = WorldContent.menuSystemContent.StarsModel;
        }

        public override void Update(GameTime gameTime)
        {
        }

    }
}
