using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZAsteroids.World.Effects.ExplosionSmoke;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem.World;
using ZitaAsteria;

namespace ZAsteroids.World.HUD.UpgradeMarkers
{
    public class UpgradeMarker : SceneObject
    {
        public UpgradeMarker()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Load the model.
            this.Model = WorldContent.hudContent.upgradeMarkerModel;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
