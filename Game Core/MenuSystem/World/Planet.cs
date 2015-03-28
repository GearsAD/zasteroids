using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZAsteroids.World.Effects.ExplosionSmoke;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.MenuSystem.World
{
    public class Planet : SceneObject
    {
        public Planet()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            //Load the model.
            this.Model = WorldContent.menuSystemContent.PlanetModel;
        }

        public override void DieEffect(Microsoft.Xna.Framework.Vector3 locationOfCollision)
        {
            base.DieEffect(locationOfCollision);
            IncendiaryExplosionACE expl = new IncendiaryExplosionACE(locationOfCollision, Vector3.Zero);
            expl.Initialize();
            expl.AddChildrenToMenuContainer();
            MenuContainer.CompoundEffects.Add(expl);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
