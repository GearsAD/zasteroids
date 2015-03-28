using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects;
using ZitaAsteria;
using ZitaAsteria.World.ShaderEffects;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem;

namespace ZitaAsteria.World.Effects.Screen_Effects
{
    /// <summary>
    /// The different types of fade effects
    /// </summary>
    public enum PredefinedFadeEffect { Undefined, BlackFadeIn, RedFadeIn, WhiteFadeIn, BlackFadeOut, RedFadeOut, WhiteFadeOut };

    [Obsolete]
    public class FadeInOutEffect : AbstractCompoundEffect
    {
        #region Public Properties
        /// <summary>
        /// The actual shader.
        /// </summary>
        public FadeInOutShader FaderShader { get; set; }
        /// <summary>
        /// The maximum life span.
        /// </summary>
        public float MaxLifeSpan { get; set; }
        /// <summary>
        /// If true then added to and removed from the menu system container - be careful when using this, only in the menu system!
        /// </summary>
        public bool UsedInMenuContainer { get; set; }
        #endregion

        #region Private Fields
        float lifeSpan = 0.0f;
        PredefinedFadeEffect predefinedFadeEffect = PredefinedFadeEffect.Undefined;
        #endregion

        public FadeInOutEffect()
        {
            MaxLifeSpan = 5.0f;
        }

        public override void DeleteChildrenFromWorldContainer()
        {
            if (!UsedInMenuContainer)
                WorldContainer.shaders.Remove(FaderShader);
            else
                MenuContainer.Shaders.Remove(FaderShader);
        }

        public override void Initialize()
        {
            base.Initialize();

            //Initialize the shader.
            FaderShader = new FadeInOutShader();
            FaderShader.Initialize();

            //Add it to the world containers...
            if (!UsedInMenuContainer)
                WorldContainer.shaders.Add(FaderShader);
            else
                MenuContainer.Shaders.Add(FaderShader);
            
        }
        
        /// <summary>
        /// Set the fader to a specific type.
        /// </summary>
        /// <param name="totalTimeS"></param>
        /// <param name="fadeEffect"></param>
        public void SetPredefinedType(float totalTimeS, PredefinedFadeEffect fadeEffect)
        {
            predefinedFadeEffect = fadeEffect;

            switch (fadeEffect)
            {
                case(PredefinedFadeEffect.BlackFadeIn): SetBlackFadeInEffect(totalTimeS); break;
                case(PredefinedFadeEffect.BlackFadeOut): SetBlackFadeInEffect(totalTimeS); break;
                case(PredefinedFadeEffect.RedFadeIn): SetRedFadeInEffect(totalTimeS); break;
                case(PredefinedFadeEffect.RedFadeOut): SetRedFadeOutEffect(totalTimeS); break;
                case(PredefinedFadeEffect.WhiteFadeIn): SetWhiteFadeInEffect(totalTimeS); break;
                case(PredefinedFadeEffect.WhiteFadeOut): SetWhiteFadeOutEffect(totalTimeS); break;
            }
        }

        private void SetRedFadeInEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.CurrentFactors = new Vector3(1.2f, -1.2f, -1.2f);
            FaderShader.DeltaFactorPerS = new Vector3(-1.2f / MaxLifeSpan, 1.2f / MaxLifeSpan, 1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed = true;
            FaderShader.IsIncreasingGreen = true;
            FaderShader.IsIncreasingBlue = true;
        }

        private void SetBlackFadeInEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.CurrentFactors = new Vector3(-1.2f, -1.2f, -1.2f); 
            FaderShader.DeltaFactorPerS = new Vector3(1.2f / MaxLifeSpan, 1.2f / MaxLifeSpan, 1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed = true;
            FaderShader.IsIncreasingGreen = true;
            FaderShader.IsIncreasingBlue = true;
        }

        private void SetWhiteFadeInEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.CurrentFactors = new Vector3(1.2f, 1.2f, 1.2f); 
            FaderShader.DeltaFactorPerS = new Vector3(-1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed = true;
            FaderShader.IsIncreasingGreen = true;
            FaderShader.IsIncreasingBlue = true;
        }

        private void SetRedFadeOutEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.DeltaFactorPerS = new Vector3(1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed         = true;
            FaderShader.IsIncreasingGreen       = true;
            FaderShader.IsIncreasingBlue        = true;
        }

        private void SetBlackFadeOutEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.DeltaFactorPerS = new Vector3(-1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed         = true;
            FaderShader.IsIncreasingGreen       = true;
            FaderShader.IsIncreasingBlue        = true;
        }

        private void SetWhiteFadeOutEffect(float totalTimeS)
        {
            MaxLifeSpan = totalTimeS;
            FaderShader.DeltaFactorPerS = new Vector3(-1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan, -1.2f / MaxLifeSpan);

            FaderShader.IsIncreasingRed         = true;
            FaderShader.IsIncreasingGreen       = true;
            FaderShader.IsIncreasingBlue        = true;
        }

        public override void AddChildrenToWorldContainer()
        {
            throw new NotImplementedException();
        }

        public override bool ShouldDeleteFromWorldContainer()
        {
            if(lifeSpan > MaxLifeSpan)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            
            lifeSpan += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
