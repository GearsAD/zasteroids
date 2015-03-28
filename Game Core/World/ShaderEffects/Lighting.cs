using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ADFieldTraining.World.ShaderEffects
{
    /// <summary>
    /// The class that handles all the lighting.
    /// </summary>
    public class Lighting : ShaderEffect
    {
        /// <summary>
        /// The light source.
        /// </summary>
        public Light lightSource { get; set; }

        /// <summary>
        /// The ambiend light as a vector of R, G, and B. These must be between 0 and 1.
        /// </summary>
        public Vector3 ambientLight { get; set; }

        EffectParameter effectParameter_LightSourceLocation;
        EffectParameter effectParameter_LightColour;
        EffectParameter effectParameter_LightAmbient;
        
        /// <summary>
        /// Create a new lighting class.
        /// </summary>
        public Lighting()
        {
            //Good reading = http://msdn.microsoft.com/en-us/library/bb509638(VS.85).aspx
        }

        /// <summary>
        /// Initialize the Lighting class with the GameClass for the GraphicsDevice and the ContentManager (like we did in the old days...)
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public override void Initialize(GameClass gameClass)
        {
            base.Initialize(gameClass);

            //Create a new list of lights...
            lightSource = new Light(Vector2.Zero, Vector3.One * 120);

            //Set the ambient light source to be a small amount of white...
            ambientLight = new Vector3(0.2f, 0.2f, 0.2f);

            //Load the lighting shader effect...
            hlslEffect = gameClass.Content.Load<Effect>("Effects\\Lighting");

            //Set up the pixel shader parameters...
            effectParameter_LightSourceLocation = hlslEffect.Parameters["lightSourceLocation"];
            effectParameter_LightColour = hlslEffect.Parameters["lightColour"];
            effectParameter_LightAmbient = hlslEffect.Parameters["lightAmbient"];
        }

        /// <summary>
        /// Draw the world (in texture form) onto another texture with the lighting.
        /// </summary>
        /// <param name="originalScene"></param>
        /// <returns></returns>
        public override Texture2D DrawSceneWithEffect(Texture2D originalScene, GameClass gameClass)
        {
            //Store the current render target....
            RenderTarget2D originalRenderTarget = (RenderTarget2D)gameClass.GraphicsDevice.GetRenderTarget(0);
            gameClass.GraphicsDevice.SetRenderTarget(0, shaderRenderTarget);

            gameClass.GraphicsDevice.Clear(Color.Black);

            //We will now draw the world texture onto the graphics device while using the pixel shader.
            worldDrawingSpriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None); //Don't do anything fancy with it.
            
            //Set up the parameters for the pixel shader
            Vector2 screenLightLocation = lightSource.location - WorldContainer.gameCamera.GetRawCameraLocation() + WorldContainer.gameCamera.playerLocationOnScreen;
            effectParameter_LightSourceLocation.SetValue(screenLightLocation);
            effectParameter_LightColour.SetValue(lightSource.lightColour);
            effectParameter_LightAmbient.SetValue(ambientLight);

            //Start up the pixel shader.
            hlslEffect.Begin();
            hlslEffect.CurrentTechnique.Passes[0].Begin();

            //Draw the world onto the graphics device.
            worldDrawingSpriteBatch.Draw(
                originalScene, //The texture that contains the world's render
                Vector2.Zero, //At 0,0
                Color.White); //With no modulating colour.

            //Finish up with the SpriteBatch drawing.
            worldDrawingSpriteBatch.End();

            //Finish up the pixel shader.
            hlslEffect.CurrentTechnique.Passes[0].End();
            hlslEffect.End();

            //Put the graphics device back to the way it first was...
            gameClass.GraphicsDevice.SetRenderTarget(0, originalRenderTarget);
            //Get the lit up screen and return it.
            screenAfterShader = shaderRenderTarget.GetTexture();
            return screenAfterShader;
        }
    }
}
