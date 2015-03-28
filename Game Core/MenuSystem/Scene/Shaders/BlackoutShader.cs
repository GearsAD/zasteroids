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
    /// This generates the blackout and the lens flare.
    /// </summary>
    public class BlackoutShader : ShaderEffect
    {
        /// <summary>
        /// The blackout image for the light tracing.
        /// </summary>
        Texture2D blackoutImage = null;
        /// <summary>
        /// The blackout render target.
        /// </summary>
        RenderTarget2D blackoutRenderTarget = null;

        public BlackoutShader()
        {
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.menuSystemContent.CrepBlackoutEffect;
            //Select the lenseflare texture.
            int sun = 2; //I like the blue sun more, mkay [GearsAD]// ZAMathTools.uniformRandomGenerator.Next(WorldContent.menuSystemContent.LenseFlareTextures.Length);

            this.hlslEffect.Parameters["LenseFlareTexture"].SetValue(WorldContent.menuSystemContent.LenseFlareTextures[sun]);

            //Set up the render target
            blackoutRenderTarget = new RenderTarget2D(MenuContainer.GraphicsDevice,
                            MenuContainer.GraphicsDevice.PresentationParameters.BackBufferWidth / 2,
                            MenuContainer.GraphicsDevice.PresentationParameters.BackBufferHeight / 2,
                            false,
                            MenuContainer.GraphicsDevice.PresentationParameters.BackBufferFormat,
                            MenuContainer.GraphicsDevice.PresentationParameters.DepthStencilFormat,
                            MenuContainer.GraphicsDevice.PresentationParameters.MultiSampleCount,
                            MenuContainer.GraphicsDevice.PresentationParameters.RenderTargetUsage);

            //Set the width and height.
//            this.hlslEffect.Parameters["previousImageDecayRateS"].SetValue(PriorFrameBlendFraction);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.hlslEffect.Parameters["LightScreenPos"].SetValue(GetLightScreenPos());
//            this.hlslEffect.Parameters["elapsedTimeS"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Returns the sun's position on the screen in interval [0,1].
        /// </summary>
        /// <returns></returns>
        private Vector2 GetLightScreenPos()
        {
            //Get the particle system's world Location in 3D.
            Vector3 worldLocation = Vector3.Zero;

            //Now convert it to a screen Location.
            Vector3 screenLocation =
                MenuContainer.GraphicsDevice.Viewport.Project(
                    worldLocation,
                    MenuContainer.Camera.GetProjectionMatrix(MenuContainer.GraphicsDevice.Viewport),
                    MenuContainer.Camera.GetViewMatrix(),
                    Matrix.Identity);

            return new Vector2(screenLocation.X / (float)MenuContainer.GraphicsDevice.Viewport.Width, screenLocation.Y / (float)MenuContainer.GraphicsDevice.Viewport.Height);
        }

        public override Texture2D DrawSceneWithEffect(Texture2D originalScene)
        {
            //1. Right, render the scene in completely black...
            MenuContainer.GraphicsDevice.SetRenderTarget(blackoutRenderTarget);
            MenuContainer.GraphicsDevice.Clear(Color.White);
            //Render the important stuff to the scene.
            MenuContainer.Shield.Draw(true);
            MenuContainer.Satellite.Draw(true);
            MenuContainer.Planet.Draw(true);
            MenuContainer.ShipApproaching.Draw(true);
            for (int i = 0; i < MenuContainer.ShipsOrbiting.Count; i++)
                MenuContainer.ShipsOrbiting[i].Draw(true);
            for (int sexyRocksControlledByZombies = 0; sexyRocksControlledByZombies < MenuContainer.Asteroids.Count; sexyRocksControlledByZombies++)
            {
                MenuContainer.Asteroids[sexyRocksControlledByZombies].Draw(true);
            }
            MenuContainer.SpherePlayer.Draw(true); 
            MenuContainer.GraphicsDevice.SetRenderTarget(null);


            //2. Now render the lenseflare.
            this.hlslEffect.Parameters["BlackoutTexture"].SetValue(blackoutRenderTarget as Texture2D);

            //Finally, pass this to the parent draw.

            //Draw the image.
            return base.DrawSceneWithEffect(originalScene);
        }
    }
}
