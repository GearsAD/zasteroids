using System;
using System.Collections.Generic;
using System.Linq;
using ZitaAsteria.Scene;
using ZitaAsteria.World;
using ZitaAsteria.World.Characters;
using ZitaAsteria.World.Effects;
using ZitaAsteria.World.Effects.Engineering_Mode;
using ZitaAsteria.World.Effects.Explosions;
using ZitaAsteria.World.Effects.Particle_Systems;
//using ZitaAsteria.World.Effects.Particle_Systems.Mercury_Effects;
using ZitaAsteria.World.Level;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.World.TwoDRendering;
using ZitaAsteria.World.Weapons.Projectiles;
using DPSF;
using DPSF.ParticleSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ZitaAsteria.World.Effects.Screen_Effects;
using ZitaAsteria.World.Menu;
using ZitaAsteria.World.AI;
using ZitaAsteria.World.Weapons.Projectiles;
using ZitaAsteria.World.Weapons;
using ZitaAsteria.World.Effects.Particle_Systems;
using System.IO;
//using Game_Core.World.Menu;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the main game class for Pilot Project
    /// </summary>
    public partial class GameClass : Microsoft.Xna.Framework.Game
    {
        //XNA created these variables for you because you're definitely going to need them.
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        //The render targets for the shader calls...
        RenderTarget2D shaderRenderTarget;
        Texture2D shaderTexture;

        //Used to make input smooth
        KeyboardState lastKeysDown;
        GamePadState lastButtonDown;

        //A quick framerate display
        FrameRateProfile frameRateCounter;

        // Static Particle Settings
        float mfStaticParticleTimeStep = 1.0f / 30.0f;

        /****************************************************************************************************************************************/
        // Used Specifically by DPSF
        // The World, View, and Projection matrices
        Matrix msWorldMatrix = Matrix.Identity;
        Matrix msViewMatrix = Matrix.Identity;
        Matrix msProjectionMatrix = Matrix.Identity;

        /****************************************************************************************************************************************/

        NightVision nightVision = null;
        bool nightVisionActive = false;
        bool nightVisionKeyPressed = false;

        PitchBlack pitchBlack = null;
        GrayScale grayScale = null;
        EclipseEffect eclipseLight = null;
        StaticNoiseEffect staticNoise = null;
        FadeInOutEffect fadeInOutEffect = null;

        /****************************************************************************************************************************************/

        SplashScreen gameSplashScreen = null;

        private double elapsedSeconds = 0;
        private bool gameEnded = false;

        public bool IsGameRunning { get; set; }

        public GameClass()
        {
            graphics = new GraphicsDeviceManager(this);
            
            configureGlobalSettings();
            configureGraphicsDevice();

            // Set Artwork Root Directory
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;

            //Set the device size...
            this.graphics.PreferredBackBufferWidth = (int)WorldContainer.gameConfiguration.selectedResolution.X;
            this.graphics.PreferredBackBufferHeight = (int)WorldContainer.gameConfiguration.selectedResolution.Y;
            this.graphics.SynchronizeWithVerticalRetrace = false;  // Disable V-Sync

            //Set the game to run at max framerate...
            base.IsFixedTimeStep = false;

            //Start the splash screen...
            gameSplashScreen = new SplashScreen();
            gameSplashScreen.Show();
        }

        private void configureGlobalSettings()
        {
            GameConfiguration config = new GameConfiguration();

            WorldContainer.gameConfiguration = config;
        }

        void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            GraphicsDevice.VertexSamplerStates[0]   = SamplerState.PointClamp;
            GraphicsDevice.SamplerStates[0]         = SamplerState.PointClamp;
        }
    }
}
