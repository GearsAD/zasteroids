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
    /// This draws the crepuscular rays onto the current scene as an additive effect.
    /// </summary>
    public class CrepuscularRaysShader : ShaderEffect
    {
        #region Private Fields
        /// <summary>
        /// The child blackout shader.
        /// </summary>
        BlackoutShader _blackoutShader;
        #endregion

        public CrepuscularRaysShader()
        {
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.menuSystemContent.CrepRaysEffect;

            _blackoutShader = new BlackoutShader();
            _blackoutShader.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.hlslEffect.Parameters["LightScreenPos"].SetValue(GetLightScreenPos());
//            this.hlslEffect.Parameters["elapsedTimeS"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);

            this.hlslEffect.Parameters["Viewport"].SetValue(new Vector2(WorldContainer.graphicsDevice.Viewport.Width, WorldContainer.graphicsDevice.Viewport.Height));
            _blackoutShader.Update(gameTime);
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
            Texture2D blackoutTexture = _blackoutShader.DrawSceneWithEffect(originalScene);

            //2. Now render the lenseflare with the crep rays.
            this.hlslEffect.Parameters["BlackoutTexture"].SetValue(blackoutTexture);
            
            //Finally, pass this to the parent draw.
            //Draw the image.
            return base.DrawSceneWithEffect(originalScene);
        }
    }
}
