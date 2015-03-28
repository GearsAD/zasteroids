using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;

namespace ZitaAsteria.World.ShaderEffects
{
    public abstract class ShaderEffect
    {
        #region Public Properties
        /// <summary>
        /// If true then it's enabled and will process the scene.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// If true then the effect is initialized.
        /// </summary>
        [XmlIgnore]
        [ContentSerializerIgnore]
        public bool IsInitialized { get; private set; }
        #endregion

        #region Private Fields
        /// <summary>
        /// The SpriteBatch that we will use to draw the world (in texture form) onto the graphics device.
        /// </summary>
        protected SpriteBatch worldDrawingSpriteBatch = null;

        /// <summary>
        /// The actual effect that we will use - this is the shader.
        /// </summary>
        protected Effect hlslEffect                 = null;

        protected Texture2D screenAfterShader       = null;
        protected RenderTarget2D shaderRenderTarget = null;
        #endregion

        /// <summary>
        /// Default - enable the effect.
        /// </summary>
        public ShaderEffect()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Initialize the ShaderEffect.
        /// </summary>
        public virtual void Initialize()
        {
            //Initialize the SpriteBatch...
            worldDrawingSpriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);
            
            //Set up the render target
            shaderRenderTarget = new RenderTarget2D(WorldContainer.graphicsDevice,
                            WorldContainer.graphicsDevice.PresentationParameters.BackBufferWidth,
                            WorldContainer.graphicsDevice.PresentationParameters.BackBufferHeight,
                            false,
                            WorldContainer.graphicsDevice.PresentationParameters.BackBufferFormat,
                            WorldContainer.graphicsDevice.PresentationParameters.DepthStencilFormat,
                            WorldContainer.graphicsDevice.PresentationParameters.MultiSampleCount,
                            WorldContainer.graphicsDevice.PresentationParameters.RenderTargetUsage);
            
            screenAfterShader = new Texture2D(
                WorldContainer.graphicsDevice, 
                shaderRenderTarget.Width, 
                shaderRenderTarget.Height,
                false, 
                WorldContainer.graphicsDevice.PresentationParameters.BackBufferFormat);

            IsInitialized = true;
        }
        
        /// <summary>
        /// Update the ShaderEffect.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {}

        /// <summary>
        /// Draw the world (in texture form) onto another texture with the lighting.
        /// </summary>
        /// <param name="originalScene"></param>
        /// <returns></returns>
        public virtual Texture2D DrawSceneWithEffect(Texture2D originalScene)
        {
            if (!IsEnabled) return originalScene;

            //Otherwise, process the scene.
            WorldContainer.graphicsDevice.SetRenderTarget(shaderRenderTarget);

            WorldContainer.graphicsDevice.Clear(Color.Black);

            //We will now draw the world texture onto the graphics device while using the pixel shader.
            worldDrawingSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive); //Don't do anything fancy with it.

            //Start up the pixel shader.
            for (int i = 0; i < hlslEffect.CurrentTechnique.Passes.Count; i++)
            {
                hlslEffect.CurrentTechnique.Passes[i].Apply();

                //Draw the world onto the graphics device.
                worldDrawingSpriteBatch.Draw(
                    originalScene, //The texture that contains the world's render
                    Vector2.Zero, //At 0,0
                    Color.White); //With no modulating colour.
            }

            //Finish up with the SpriteBatch drawing.
            worldDrawingSpriteBatch.End();

            //Put the graphics device back to the back buffer...
            WorldContainer.graphicsDevice.SetRenderTarget(null);
            //Get the lit up screen and return it.
            screenAfterShader = shaderRenderTarget;
            return screenAfterShader;
        }
    }
}
