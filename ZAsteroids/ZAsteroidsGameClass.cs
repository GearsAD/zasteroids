using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZitaAsteria.Scene;
using ZAsteroids.World.Weapons;
using ZAsteroids.World.HUD;
using ZAsteroids.World;
using ZAsteroids.World.Upgrades;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the main game class for Pilot Project
    /// </summary>
    public partial class ZAsteroidsGameClass : Microsoft.Xna.Framework.Game
    {

        #region Public Properties
        //XNA created these variables for you because you're definitely going to need them.
        public GraphicsDeviceManager graphics;

        public ZAstWeapon Weapon { get; private set; }
        #endregion

        //public SpriteBatch spriteBatch;

        //The render targets for the shader calls...
        RenderTarget2D shaderRenderTarget;
        Texture2D shaderTexture;

        //Used to make input smooth
        KeyboardState lastKeysDown;
        GamePadState lastButtonDown;

        //A quick framerate display
        FrameRateProfile frameRateCounter;

        //private ObjectPoolProfile objectPoolProfile;

        /****************************************************************************************************************************************/
        // Used Specifically by DPSF
        // The World, View, and Projection matrices
        Matrix msWorldMatrix = Matrix.Identity;
        Matrix msViewMatrix = Matrix.Identity;
        Matrix msProjectionMatrix = Matrix.Identity;

        private double elapsedSeconds = 0;
        private bool gameEnded = false;

        float fAspectRatio;

        /// <summary>
        /// Test for whether to show the menu system.
        /// </summary>
        public bool IsGameRunning { get; set; }
        
        /// <summary>
        /// If true then updates are stopped.
        /// </summary>
        public bool IsGamePaused { get; set; }

        /// <summary>
        /// HUD stuff
        /// </summary>
        private HUDScore _score;

        private HUDGameState _hudModeInfo;

        private HUDCrosshair _crosshair;

        private HUDGeneral _general;

        private HUDUpgrade _upgrade;

        private HUDSheilds _shields;

        private HUDSphere _hudSphere;

        private UpgradeManager _upgradeManager;

        private HUDSatellite _hudSatellite;

        private HUDIntroScreen _hudIntroScreen;

        private HUDControls _hudControls;

        private HUDDamage _hudDamage;

        /// <summary>
        /// The game manager.
        /// </summary>
        private GameManager _gameManager;

        public ZAsteroidsGameClass()
        {
            graphics = new GraphicsDeviceManager(this);

            // Set Artwork Root Directory
            Content.RootDirectory = "Content";

            WorldContainer.gameClass = this;

            configureGlobalSettings();
            configureGraphicsDevice();

#if XBOX
            graphics.IsFullScreen = true;
           // WorldContainer.gameConfiguration.selectedResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
#else
            //if (!graphics.IsFullScreen != WorldContainer.gameConfiguration.FullScreen)
            //{
            //    graphics.ToggleFullScreen();
            //}
#endif

            //Set the device size...
            this.graphics.PreferredBackBufferWidth = (int)GameConfiguration.Resolutions[WorldContainer.gameConfiguration.DisplayResolutionS].X;
            this.graphics.PreferredBackBufferHeight = (int)GameConfiguration.Resolutions[WorldContainer.gameConfiguration.DisplayResolutionS].Y;
            this.graphics.SynchronizeWithVerticalRetrace = WorldContainer.gameConfiguration.VSyncEnabled;  // Disable V-Sync

            //Set the game to run at max framerate...
            base.IsFixedTimeStep = false;
            //base.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 10);

            this.IsMouseVisible = true;

        }

        private void configureGlobalSettings()
        {
            WorldContainer.ConfigurationManager = new GameConfigurationManager();
            WorldContainer.ConfigurationManager.LoadConfiguration();
        }

        void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            GraphicsDevice.VertexSamplerStates[0]   = SamplerState.PointClamp;
            GraphicsDevice.SamplerStates[0]         = SamplerState.PointClamp;
        }

        
    }
}
