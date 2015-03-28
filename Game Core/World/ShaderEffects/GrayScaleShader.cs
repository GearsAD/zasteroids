using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria;

namespace ZitaAsteria.World.ShaderEffects
{
    public class GrayScaleShader : ShaderEffect
    {
        
        public GrayScaleShader()
        {}

        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.grayScaleEffect;

        }
        
        public override Microsoft.Xna.Framework.Graphics.Texture2D DrawSceneWithEffect(Microsoft.Xna.Framework.Graphics.Texture2D originalScene)
        {
            return base.DrawSceneWithEffect(originalScene);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
