using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.MenuSystem;

namespace ZitaAsteria.MenuSystem.Scene.Shaders
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
            PriorFrameBlendFraction = 1.0f;
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.menuSystemContent.MotionBlurEffect;

            //Set the width and height.
            this.hlslEffect.Parameters["previousImageDecayRateS"].SetValue(PriorFrameBlendFraction);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.hlslEffect.Parameters["previousImageDecayRateS"].SetValue(PriorFrameBlendFraction);
            this.hlslEffect.Parameters["elapsedTimeS"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override Microsoft.Xna.Framework.Graphics.Texture2D DrawSceneWithEffect(Microsoft.Xna.Framework.Graphics.Texture2D originalScene)
        {
            if (priorImage == null) //First call, initialize the texture.
            {
                priorImage = new Texture2D(WorldContainer.graphicsDevice, originalScene.Width, originalScene.Height);
                innerData = new Color[priorImage.Width * priorImage.Height];
            }

            //Now set the parameter.
            this.hlslEffect.Parameters["LastImageTexture"].SetValue(priorImage);
            this.hlslEffect.Parameters["CurrentTexture"].SetValue(originalScene);

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
