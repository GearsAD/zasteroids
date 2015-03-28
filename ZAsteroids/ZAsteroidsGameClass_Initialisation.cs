using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.Scene;

using ZitaAsteria.MenuSystem;
using ZAsteroids.World.Weapons;

using DPSF;
using ZAsteroids.World.HUD;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the main game class for Pilot Project
    /// </summary>
    public partial class ZAsteroidsGameClass : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
            WorldContainer.ProfilingEnabled = false;
            WorldContainer.ShowFrameRate = false;

            WorldContainer.DepthBufferDisabledStencilState = new DepthStencilState();
            WorldContainer.DepthBufferDisabledStencilState.DepthBufferEnable = false;

            WorldContainer.DepthBufferEnabledStencilState = new DepthStencilState();
            WorldContainer.DepthBufferEnabledStencilState.DepthBufferEnable = true;

            WorldContainer.graphicsDevice = this.GraphicsDevice;
            this.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(GraphicsDevice_DeviceReset);
            WorldContainer.graphicsDeviceManager = graphics;
            //WorldContainer.gameClass = this;

            //// Declare the Particle System Manager to manage the Particle Systems (DPSF)
            WorldContainer.particleSystemManager = new ParticleSystemManager();

            elapsedSeconds = 0;
            gameEnded = false;

            // Create a new SpriteBatch, which can be used to draw textures.
            WorldContainer.spriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);

            DateTime preContent = DateTime.Now;

            //Initialize the static world content class
            WorldContent.InitializeContent(Content, WorldContainer.graphicsDevice, 
                SharedContent.ContentInitializationTypes.MenuContent | 
                SharedContent.ContentInitializationTypes.MusicContent | 
                SharedContent.ContentInitializationTypes.SfxContent | 
                SharedContent.ContentInitializationTypes.EffectsContent);

            if (WorldContainer.ProfilingEnabled || WorldContainer.ShowFrameRate)
            {
                //Initialize the framerate counter
                frameRateCounter.Initialize();

                if (WorldContainer.ProfilingEnabled)
                {
                    frameRateCounter.ProfileEntries.Add("Content", new ProfileEntry("Content", preContent));

                    frameRateCounter.ProfileEntries["Content"].EndTime = DateTime.Now;

                    frameRateCounter.ProfileEntries.Add("ObjMan", new ProfileEntry("ObjMan", DateTime.Now));
                }
            }

            //Initialise the ObjectManager
            ObjectManager.Initialize_ForZAsteroids();

            if (WorldContainer.ProfilingEnabled)
            {
                frameRateCounter.ProfileEntries["ObjMan"].EndTime = DateTime.Now;

                frameRateCounter.ProfileEntries.Add("General", new ProfileEntry("General", DateTime.Now));
            }

            // General Sound Effect Manager : Must be done after sfxContent Loaded
            WorldContainer.soundEffectsMgr = new SoundEffectsMgr();

            //Set up the music manager.
            WorldContainer.musicManager = new ZitaAsteria.Scene.MusicManager();
            WorldContainer.musicManager.Initialize();
            WorldContainer.musicManager.setMusicVolume();

            ////Create the shader textures
            shaderRenderTarget = new RenderTarget2D(graphics.GraphicsDevice,
                            graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                            graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                            true,
                            graphics.GraphicsDevice.PresentationParameters.BackBufferFormat,
                            graphics.GraphicsDevice.PresentationParameters.DepthStencilFormat,
                            graphics.GraphicsDevice.PresentationParameters.MultiSampleCount,
                            graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage);

            shaderTexture = new Texture2D(graphics.GraphicsDevice, shaderRenderTarget.Width, shaderRenderTarget.Height,
                true, graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);

            //Create the menu controller
            MenuContainer.MenuSystemScene = new MenuSystemScene();
            MenuContainer.MenuSystemScene.Initialize(GraphicsDevice, WorldContent.menuContentManager);
            MenuContainer.Satellite.SceneRotation = new Vector3(0, 0, 0);

            //And give the ship a weapon.
            Weapon = new LaserWeapon();
            Weapon.Initialize();

            HUDProperties.DrawLines = new bool[]
            {
                false,
                false,
                false,
                false,
                false
            };

            // HUD stuff
            _score = new ZAsteroids.World.HUD.HUDScore();
            _score.Initialize();
            _score.Enable(true);

            _hudModeInfo = new ZAsteroids.World.HUD.HUDGameState();
            _hudModeInfo.Initialize();
            _hudModeInfo.Enable(true);

            _upgrade = new ZAsteroids.World.HUD.HUDUpgrade();
            _upgrade.Initialize();
            _upgrade.Enable(true);

            _crosshair = new ZAsteroids.World.HUD.HUDCrosshair();
            _crosshair.Initialize();
            _crosshair.Enable(true);

            _general = new ZAsteroids.World.HUD.HUDGeneral();
            _general.Initialize();
            _general.Enable(true);

            _shields = new ZAsteroids.World.HUD.HUDSheilds();
            _shields.Initialize();
            _shields.Enable(true);

            _hudSphere = new ZAsteroids.World.HUD.HUDSphere();
            _hudSphere.Initialize();
            _hudSphere.Enable(true);

            _hudSatellite = new HUDSatellite();
            _hudSatellite.Initialize();
            _hudSatellite.Enable(true);

            _hudIntroScreen = new HUDIntroScreen();
            _hudIntroScreen.Initialize();

            _hudControls = new HUDControls();
            _hudControls.Initialize();

            _hudDamage = new HUDDamage();
            _hudDamage.Initialize();
            _hudDamage.Enable(true);

            // Compute the Aspect Ratio of the window
            fAspectRatio = (float)WorldContainer.graphicsDevice.Viewport.Width / (float)WorldContainer.graphicsDevice.Viewport.Height;

            _gameManager = new ZAsteroids.World.GameManager(this);
            HUDProperties.GameManager = _gameManager;
            _gameManager.Initialize();
            //Initialize the upgrades
            _upgradeManager = new ZAsteroids.World.Upgrades.UpgradeManager(_gameManager);
            HUDProperties.UpgradeManager = _upgradeManager;

            //Do a garbage collection after initialization
            GC.Collect();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Create the framerate counter.
            //frameRateCounter = null;

            // Declare the Particle System Manager to manage the Particle Systems (DPSF)
            WorldContainer.particleSystemManager.DestroyAndRemoveAllParticleSystems();
        }

        public void restart()
        {
            UnloadContent();
            Initialize();
            LoadContent();
        }

        public void unloadGameData()
        {
            UnloadContent();
            base.Initialize();
        }

        public void configureGraphicsDevice()
        {
            //DISABLE Anti-Aliasing
            graphics.PreferMultiSampling = false;

            //Set the device size...
            this.graphics.PreferredBackBufferWidth = (int)GameConfiguration.Resolutions[WorldContainer.gameConfiguration.DisplayResolutionS].X;
            this.graphics.PreferredBackBufferHeight = (int)GameConfiguration.Resolutions[WorldContainer.gameConfiguration.DisplayResolutionS].Y;
            this.graphics.SynchronizeWithVerticalRetrace = WorldContainer.gameConfiguration.VSyncEnabled;  // Disable V-Sync

#if XBOX
            graphics.IsFullScreen = true;
           // WorldContainer.gameConfiguration.selectedResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
#else
            if (graphics.IsFullScreen != WorldContainer.gameConfiguration.FullScreen)
            {
                graphics.ToggleFullScreen();
            }
#endif

            this.graphics.ApplyChanges();
            //LoadContent();
        }

        public void configurePeripheralSettings()
        {
//            WorldContainer.soundEffectsMgr.setSoundFXVolume();
            WorldContainer.musicManager.setMusicVolume();
        }

    } // END of Partial class GameClass_Initialisation
}

