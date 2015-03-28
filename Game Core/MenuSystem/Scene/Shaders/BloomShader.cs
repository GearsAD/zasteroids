using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.MenuSystem.Scene.ShaderSetting;

namespace ZitaAsteria.MenuSystem.Scene.Shaders
{
    class BloomShader : ShaderEffect
    {
        SpriteBatch spriteBatch;

        // Choose what display settings the bloom should use.
        public BloomSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        BloomSettings settings = BloomSettings.PresetSettings[1];

        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        RenderTarget2D bloomRenderTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        Texture2D screenAfterShader;

        public BloomShader()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);

            bloomExtractEffect = WorldContent.menuSystemContent.BloomExtractEffect;
            bloomCombineEffect = WorldContent.menuSystemContent.BloomCombineEffect;
            gaussianBlurEffect = WorldContent.menuSystemContent.GaussianBlurEffect;

            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = MenuContainer.GraphicsDevice.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for rendering the main scene, prior to applying bloom.
            bloomRenderTarget = new RenderTarget2D(MenuContainer.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            screenAfterShader = new Texture2D(MenuContainer.GraphicsDevice, width, height, false, format);

            // Create two rendertargets for the bloom processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because images are
            // to be blurred in any case.
            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(MenuContainer.GraphicsDevice, width, height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(MenuContainer.GraphicsDevice, width, height, false, format, DepthFormat.None);
        }

        /// <summary>
        /// This is where it all happens.
        /// </summary>
        public override Texture2D DrawSceneWithEffect(Texture2D originalScene)
        {
            // We can set up different Bloom effects an simply change the effect that is used here.
            MenuContainer.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            bloomExtractEffect.Parameters["BloomThreshold"].SetValue(Settings.BloomThreshold);

            DrawFullscreenQuad(originalScene, renderTarget1, bloomExtractEffect);

            // Pass 2: draw from rendertarget 1 into rendertarget 2,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);

            DrawFullscreenQuad(renderTarget1, renderTarget2, gaussianBlurEffect);

            // Pass 3: draw from rendertarget 2 back into rendertarget 1,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);

            DrawFullscreenQuad(renderTarget2, renderTarget1, gaussianBlurEffect);

            // Pass 4: draw both rendertarget 1 and the original scene
            // image back into the main backbuffer, using a shader that
            // combines them to produce the final bloomed result.
            MenuContainer.GraphicsDevice.SetRenderTarget(bloomRenderTarget);
            MenuContainer.GraphicsDevice.Clear(Color.Black);

            EffectParameterCollection parameters = bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);

            // Original Image is placed below blurred image.
            MenuContainer.GraphicsDevice.Textures[1] = originalScene;

            Viewport viewport = MenuContainer.GraphicsDevice.Viewport;

            DrawFullscreenQuad(renderTarget1, viewport.Width, viewport.Height, bloomCombineEffect);
            
            MenuContainer.GraphicsDevice.SetRenderTarget(null);

            screenAfterShader = bloomRenderTarget;

            return screenAfterShader;
        }

        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            MenuContainer.GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect);
        }

        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect)
        {
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

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
            float theta = Settings.BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) * Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
