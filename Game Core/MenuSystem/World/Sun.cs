using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZitaAsteria.MenuSystem.World
{
    public class Sun : SceneObject
    {
        public Sun()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Load the model.
            //this.Model = WorldContent.menuSystemContent.SunModel;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }
    }
}
