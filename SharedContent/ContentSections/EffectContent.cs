using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Shared_Content;

namespace ZitaAsteria.ContentSections
{

    /// <summary>
    /// A static class containing references to all the effect content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public class EffectContent
    {
        //Effects
        public Effect auraEffect { get; set; }
        public Effect scanlineEffect { get; set; }
        public Effect nightVisionEffect { get; set; }
        public Effect fadeInOutEffect { get; set; }
        public Effect eclipseEffect { get; set; }
        public Effect grayScaleEffect { get; set; }
        public Effect pitchBlackEffect { get; set; }
        public Effect explosionFlashEffect { get; set; }
        public Effect staticNoiseEffect { get; set; }
        public Effect cameraStaticNoiseEffect { get; set; }
        public Effect portalShaderEffect { get; set; }
        public Effect particleFlareEffect { get; set; }
        public Effect ShockwaveEffect { get; set; }
        public Effect backgroundGridEffect { get; set; }
        public Effect motionBlurEffect { get; set; }

        // Particles
        public Texture2D effectExplosion { get; set; }
        public Texture2D cloudParticle { get; set; }
        public Texture2D bloodParticle { get; set; }
        public Texture2D ricochetParticle { get; set; }
        public Texture2D shockwavePSTexture { get; set; }
        public Texture2D whiteWisp { get; set; }
        public Texture2D darkWisp { get; set; }
        public Texture2D explosionShockwavePSTexture { get; set; }
        public Texture2D plumePSTexture { get; set; }
        public Texture2D flareUpwardTexture { get; set; }
        public Texture2D flareSparkTexture { get; set; }
        public Texture2D sandStormPSTexture { get; set; }
        public Texture2D cameraStaticNoiseTexture { get; set; }
        public Texture2D fireParticle { get; set; }
        public Texture2D generalParticle { get; set; }
        public Texture2D sunParticle { get; set; }
        public Texture2D wallDebrisParticle { get; set; }
        public Texture2D particleFlareParticle { get; set; }
        public Texture2D beamFlareParticle { get; set; }

        // Menu textures
        //public Texture2D loadingBackground { get; set; }
        //public Texture2D backgroundDisplay { get; set; }
        public Texture2D menuLogo { get; set; }
        public Texture2D startMenuLogo { get; set; }
        public Texture2D controller { get; set; }
        //public Texture2D tick { get; set; }
        public Texture2D menuSelectionIcon { get; set; }
        public Texture2D playerChosenIcon { get; set; }
        public Texture2D consoleViewBackground { get; set; }

        //Button Textures
        public Texture2D button_back { get; set; }
        public Texture2D button_d_pad { get; set; }
        public Texture2D button_facebutton_a { get; set; }
        public Texture2D button_facebutton_b { get; set; }
        public Texture2D button_facebutton_x { get; set; }
        public Texture2D button_facebutton_y { get; set; }
        public Texture2D button_left_bumper { get; set; }
        public Texture2D button_left_thumbstick { get; set; }
        public Texture2D button_left_trigger { get; set; }
        public Texture2D button_right_bumper { get; set; }
        public Texture2D button_right_thumbstick { get; set; }
        public Texture2D button_right_trigger { get; set; }
        public Texture2D button_start { get; set; }

        //Background movie...
        /// <summary>
        /// The video for the background of the menu.
        /// </summary>
        public Video menuBackgroundVideo { get; set; }

        /// <summary>
        /// The game intro movie
        /// </summary>
        public Video gameIntroMovie { get; set; }

        /// <summary>
        /// The ZA logo movie
        /// </summary>
        public Video logoMovie { get; set; }


        /// <summary>
        /// The AD Logo movie
        /// </summary>
        public Video adLogoMovie { get; set; }

        // Mercury Particle Engine Effects
        //public ParticleEffect MPEexplosionEffect { get; set; }
        //public ParticleEffect rocketTrailEffect { get; set; }
        //public ParticleEffect miniRocketTrailEffect { get; set; }

        //Font for HUD
        public SpriteFont hudFont { get; set; }

        public Texture2D controllerMap { get; set; }

        public EffectContent()
        {
        }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        public void InitializeFromContent(ContentManager gameContentManager)
        {
            //Load the effects

            // Load Particle Effect Content
            generalParticle = gameContentManager.Load<Texture2D>("Effects\\Particle Systems\\generalParticle");
            cloudParticle = gameContentManager.Load<Texture2D>("Effects\\Particle Systems\\Mercury Particle Systems\\Textures\\Cloud001");
            explosionShockwavePSTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/ExplosionParticles");
            shockwavePSTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Shockwave");

            plumePSTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Smoke");
            flareUpwardTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Flare");
            flareSparkTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Spark");
            sandStormPSTexture = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Cloud");
            fireParticle = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/Fire");
            bloodParticle = gameContentManager.Load<Texture2D>("Effects/Particle Systems/BloodParticle");
            ricochetParticle = gameContentManager.Load<Texture2D>("Effects/Particle Systems/Ricochet");
            wallDebrisParticle = gameContentManager.Load<Texture2D>("Effects/Particle Systems/WallDebris");
            sunParticle = gameContentManager.Load<Texture2D>("Effects\\DPSF\\Textures\\Sun");
            particleFlareParticle = gameContentManager.Load<Texture2D>("Effects/DPSF/Textures/ParticleGunProjectile");
            beamFlareParticle = gameContentManager.Load<Texture2D>("Effects/Particle Systems/Beam");

            // Screen Effect Content
            auraEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\AuraEffect");
            scanlineEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\VideoNoise");
            nightVisionEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\NightVision");
            pitchBlackEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\PitchBlack");
            grayScaleEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\GrayScale");
            explosionFlashEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\ExplosionFlash");
            eclipseEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\EclipseEffect");
            staticNoiseEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\StaticNoise");
            cameraStaticNoiseEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\CameraStaticNoise");
            cameraStaticNoiseTexture = gameContentManager.Load<Texture2D>("Effects\\Screen Effects\\CameraNoise");
            fadeInOutEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\FadeInOut");
            portalShaderEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\Portal");
            particleFlareEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\ParticleFlareEffect");
            backgroundGridEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\BackgroundGridEffect");
            motionBlurEffect = gameContentManager.Load<Effect>("Effects\\Screen Effects\\MotionBlurEffect");

            //Load the shockwave
            ShockwaveEffect = gameContentManager.Load<Effect>("Effects\\Weapons\\ShockWave");

            //Loads HUD font
            //hudFont = gameContentManager.Load<SpriteFont>("Fonts\\HudFont");

            // DPSF splatters
            darkWisp = gameContentManager.Load<Texture2D>("Effects\\DPSF\\Textures\\DarkWisp");
            whiteWisp = gameContentManager.Load<Texture2D>("Effects\\DPSF\\Textures\\WhiteWisp");

            //Buttons
            button_back = gameContentManager.Load<Texture2D>("Menu\\Buttons\\back");
            button_d_pad = gameContentManager.Load<Texture2D>("Menu\\Buttons\\d-pad");
            button_facebutton_a = gameContentManager.Load<Texture2D>("Menu\\Buttons\\facebutton_a");
            button_facebutton_b = gameContentManager.Load<Texture2D>("Menu\\Buttons\\facebutton_b");
            button_facebutton_x = gameContentManager.Load<Texture2D>("Menu\\Buttons\\facebutton_x");
            button_facebutton_y = gameContentManager.Load<Texture2D>("Menu\\Buttons\\facebutton_y");
            button_left_bumper = gameContentManager.Load<Texture2D>("Menu\\Buttons\\left_bumper");
            button_left_thumbstick = gameContentManager.Load<Texture2D>("Menu\\Buttons\\left_thumbstick");
            button_left_trigger = gameContentManager.Load<Texture2D>("Menu\\Buttons\\left_trigger");
            button_right_bumper = gameContentManager.Load<Texture2D>("Menu\\Buttons\\right_bumper");
            button_right_thumbstick = gameContentManager.Load<Texture2D>("Menu\\Buttons\\right_thumbstick");
            button_right_trigger = gameContentManager.Load<Texture2D>("Menu\\Buttons\\right_trigger");
            button_start = gameContentManager.Load<Texture2D>("Menu\\Buttons\\start");

            controllerMap = gameContentManager.Load<Texture2D>("Menu\\controller map");
        }
    }
}
