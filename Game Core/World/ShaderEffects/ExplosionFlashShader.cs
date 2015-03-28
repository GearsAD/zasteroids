using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// The class that handles all the lighting.
    /// </summary>
    public class ExplosionFlashShader : ShaderEffect
    {
        /// <summary>
        /// The Location of the light. There is also the option of using the WorldObject/PhysicsWorldObject class here...
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// This is the Red, Green, and Blue values of the light source. We're not going to use an intensity because it'll make the 
        /// calculations more complex. Scale these values between 0 and 1 for both the colour and intensity.
        /// </summary>
        public Vector3 lightColour { get; set; }

        /// <summary>
        /// The ambiend light as a vector of R, G, and B. These must be between 0 and 1.
        /// </summary>
        public Vector3 ambientLight { get; set; }

        /// <summary>
        /// The ambiend light as a vector of R, G, and B. These must be between 0 and 1.
        /// </summary>
        public bool allowOverExposure { get; set; }

        /// <summary>
        /// The light source Location parameter for the pixel shader.
        /// </summary>
        EffectParameter effectParameter_LightSourceLocation;
        /// <summary>
        /// The light source Location parameter for the pixel shader.
        /// </summary>
        EffectParameter effectParameter_AllowOverExposure;
        /// <summary>
        /// The light colour parameter for the pixel shader.
        /// </summary>
        EffectParameter effectParameter_LightColour;
        
        /// <summary>
        /// The ambient light parameter for the pixel shader.
        /// </summary>
        EffectParameter effectParameter_LightAmbient;
        
        /// <summary>
        /// Screen Height.
        /// </summary>
        EffectParameter effectParameter_ScreenHeight;
            
        /// <summary>
        /// Screen Width.
        /// </summary>
        EffectParameter effectParameter_ScreenWidth;
        
        /// <summary>
        /// Create a new lighting class.
        /// </summary>
        public ExplosionFlashShader()
        {
            //The Location will be updated to the player's Location, so make it (0,0) for now.
            //Location = Vector2.Zero;
            //Make the light source white, with an intensity of 60 (at its centre).
            lightColour = Vector3.One * 60.0f;
            //Can add ambient light in here...
            ambientLight = new Vector3(1.0f, 1.0f, 1.0f) * 0.0f;

            //Load the effect.
            this.hlslEffect = WorldContent.effectContent.explosionFlashEffect;

            //Set up the pixel shader parameters...
            effectParameter_LightSourceLocation = hlslEffect.Parameters["lightSourceLocation"];
            effectParameter_LightColour = hlslEffect.Parameters["lightColour"];
            effectParameter_LightAmbient = hlslEffect.Parameters["lightAmbient"];
            effectParameter_AllowOverExposure = hlslEffect.Parameters["allowOverExposure"];

            effectParameter_ScreenHeight = hlslEffect.Parameters["ScreenHeight"];
            effectParameter_ScreenWidth = hlslEffect.Parameters["ScreenWidth"];

            // Make Sure Current Resolution Used
            effectParameter_ScreenHeight.SetValue(WorldContainer.gameClass.GraphicsDevice.Viewport.Height);
            effectParameter_ScreenWidth.SetValue(WorldContainer.gameClass.GraphicsDevice.Viewport.Width);
        }

        /// <summary>
        /// Initialize the Lighting class with the GameClass for the GraphicsDevice and the ContentManager (like we did in the old days...)
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public override void Initialize()
        {
            base.Initialize();

            // Location 
            effectParameter_LightSourceLocation.SetValue(Location);
        }

        /// <summary>
        /// Draw the world (in texture form) onto another texture with the lighting.
        /// </summary>
        /// <param name="originalScene"></param>
        /// <returns></returns>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Parameters : 
            effectParameter_LightSourceLocation.SetValue(Location);
            effectParameter_LightColour.SetValue(lightColour);
            effectParameter_LightAmbient.SetValue(ambientLight);
            effectParameter_AllowOverExposure.SetValue(allowOverExposure);
        }
    }
}
