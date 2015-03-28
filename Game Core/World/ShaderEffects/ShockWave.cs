using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// The shockwave shader effect.
    /// </summary>
    public class ShockWave : ShaderEffect
    {
        public Vector2 waveLocation {get; set;}
        public float waveRadius { get; set; }
        public float waveAmplitude { get; set; }
        public float waveWidth { get; set; }
        public float waveSpeedPXS { get; set; }
        public float waveDilationPXS { get; set; }
        public float lifeSpanS { get; set; }
        public double startLifeTimeS { get; set; }

        EffectParameter shaderShockCentre;
        EffectParameter shaderRadius;
        EffectParameter shaderWidth;
        EffectParameter shaderShockAmplitude;

        public static Random rand = new Random();

        /// <summary>
        /// Create a new lighting class.
        /// </summary>
        public ShockWave()
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

            //Load the lighting shader effect...
            hlslEffect = gameClass.Content.Load<Effect>("Effects\\ShockWave");

            waveLocation = new Vector2();
            waveRadius = 20;
            waveAmplitude = 40;
            waveWidth = 160 + (float)(rand.NextDouble() * 40);
            waveSpeedPXS = 800 + (float)(rand.NextDouble() * 500);
            waveDilationPXS = 20;
            lifeSpanS = 2 + (float)(rand.NextDouble() * 1);

            shaderShockCentre = hlslEffect.Parameters["shockCentre"];
            shaderRadius = hlslEffect.Parameters["shockRadius"];
            shaderWidth = hlslEffect.Parameters["shockWidth"];
            shaderShockAmplitude = hlslEffect.Parameters["shockAmplitude"];

            startLifeTimeS = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (startLifeTimeS == 0)
                startLifeTimeS = gameTime.TotalGameTime.TotalSeconds;
            waveRadius += waveSpeedPXS * (float)gameTime.ElapsedGameTime.TotalSeconds;
            waveAmplitude -= waveDilationPXS * (float)gameTime.ElapsedGameTime.TotalSeconds;
            waveWidth += waveDilationPXS * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (gameTime.TotalGameTime.TotalSeconds > startLifeTimeS + lifeSpanS) //Delete this...
            {
                WorldContainer.shaders.Remove(this); //Delete it...
            }
        
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

            //Set the parameters
            shaderShockCentre.SetValue(waveLocation - WorldContainer.gameCamera.GetRawCameraLocation() + WorldContainer.gameCamera.playerLocationOnScreen);
            shaderRadius.SetValue(waveRadius);
            shaderShockAmplitude.SetValue(waveAmplitude);
            shaderWidth.SetValue(waveWidth);

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
