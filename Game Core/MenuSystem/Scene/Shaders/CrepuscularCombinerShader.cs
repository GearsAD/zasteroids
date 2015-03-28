using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.Scene;

namespace ZitaAsteria.MenuSystem.Scene.Shaders
{
    /// <summary>
    /// This combines the crep rays with the original scene using a gaussian filter for the rays.
    /// </summary>
    public class CrepuscularCombinerShader : ShaderEffect
    {
        #region Private Fields
        /// <summary>
        /// The child blackout shader.
        /// </summary>
        CrepuscularRaysShader _raysShader;
        #endregion

        public CrepuscularCombinerShader()
        {
        }

        /// <summary>
        /// Initialize the Static Noise effect.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Load the effect.
            this.hlslEffect = WorldContent.menuSystemContent.CrepCombinerEffect;

            //Initialize the gaussian blur.
            SetBlurEffectParameters(0.5f / WorldContainer.graphicsDevice.Viewport.Width, 0.5f / WorldContainer.graphicsDevice.Viewport.Height);

            _raysShader = new CrepuscularRaysShader();
            _raysShader.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

//            this.hlslEffect.Parameters["LightScreenPos"].SetValue(GetLightScreenPos());
//            this.hlslEffect.Parameters["elapsedTimeS"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);

//            this.hlslEffect.Parameters["Viewport"].SetValue(new Vector2(WorldContainer.graphicsDevice.Viewport.Width, WorldContainer.graphicsDevice.Viewport.Height));
            _raysShader.Update(gameTime);
        }

        public override Texture2D DrawSceneWithEffect(Texture2D originalScene)
        {
            Texture2D raysTexture = _raysShader.DrawSceneWithEffect(originalScene);

            //2. Now render the lenseflare with the crep rays.
            this.hlslEffect.Parameters["RaysTexture"].SetValue(raysTexture);
            
            //Finally, pass this to the parent draw.
            //Draw the image.
            //Calculate the dot product to see if we should superimpose the image or not.
            bool shouldDraw = ShouldDrawRays();
            return (shouldDraw ? base.DrawSceneWithEffect(originalScene) : originalScene);
        }

        /// <summary>
        /// Checks if the rays should be drawn.
        /// </summary>
        /// <returns></returns>
        private bool ShouldDrawRays()
        {
            //Two different camera types - normal orbital camera, or a fancy ZA camera.
            if (MenuContainer.Camera as ZAsteroidsCamera == null) //Do the simple test
            {
                if (Vector3.Dot(MenuContainer.Camera.targetLocation - MenuContainer.Camera.cameraLocation, MenuContainer.Sun.Location - MenuContainer.Camera.cameraLocation) > 0f)
                    return true;
            }
            else //Do the fancy test
            {
                Vector3 forward = Vector3.Transform(Vector3.UnitZ, (MenuContainer.Camera as ZAsteroidsCamera).Orientation);
                Vector3 camForward = (MenuContainer.Sun.Location - MenuContainer.Camera.cameraLocation);
                camForward.Normalize();
                if (Vector3.Dot(forward, camForward) > 0.45f)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = hlslEffect.Parameters["SampleWeights"];
            offsetsParameter = hlslEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }

        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeGaussian(float n)
        {
            float theta = 4;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) * Math.Exp(-(n * n) / (2 * theta * theta)));
        }

    }
}
