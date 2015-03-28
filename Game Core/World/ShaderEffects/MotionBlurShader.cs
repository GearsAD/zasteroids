using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using Microsoft.Xna.Framework.Graphics;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// This draws the prior scene together with the current, causing a motion blur and some smoothing.
    /// </summary>
    public class MotionBlurShader : ShaderEffect
    {
        /// <summary>
        /// The fraction of the prior frame that is blended into the current.
        /// </summary>
        public float PriorFrameBlendFraction { get; set; }

        /// <summary>
        /// The prior screen texture.
        /// </summary>
        Texture2D priorImage = null;

        Color[] innerData = null;

        public MotionBlurShader()
        {
            PriorFrameBlendFraction = 0.3f;
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.motionBlurEffect;

            //Set the width and height.
            this.hlslEffect.Parameters["blendFraction"].SetValue(PriorFrameBlendFraction);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.hlslEffect.Parameters["blendFraction"].SetValue(PriorFrameBlendFraction);
        }

        public override Microsoft.Xna.Framework.Graphics.Texture2D DrawSceneWithEffect(Microsoft.Xna.Framework.Graphics.Texture2D originalScene)
        {
            if (priorImage == null) //First call, initialize the texture.
            {
                priorImage = new Texture2D(WorldContainer.graphicsDevice, originalScene.Width, originalScene.Height);
                innerData = new Color[priorImage.Width * priorImage.Height];
            }

            //Now set the parameter.
            this.hlslEffect.Parameters["PriorScreenTexture"].SetValue(priorImage);

            //Draw the image.
            Texture2D thisImage = base.DrawSceneWithEffect(originalScene);

            //Unset the textures before unlocking.
            WorldContainer.graphicsDevice.Textures[0] = null;
            WorldContainer.graphicsDevice.Textures[1] = null;
            WorldContainer.graphicsDevice.Textures[2] = null;

            //Save as last image - do as a copy for now.
            thisImage.GetData<Color>(innerData);
            priorImage.SetData<Color>(innerData);

            return thisImage;
        }
    }
}
