using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ZitaAsteria.Configuration;

namespace ZitaAsteria
{
    public enum GameInputMode
    {
        Keyboard,
        GamePad
    }

    public enum GamePadStyle
    {
        Normal,
        FlightSim
    }

    public class GameConfiguration
    {
        /// <summary>
        /// Screen resolution for the entire game
        /// </summary>
        public static Dictionary<string, Vector2> Resolutions { get; private set; }

        /// <summary>
        /// Volume Co-Efficient to be used for ALL sound effects globally
        /// </summary>
        public int soundVolume { get; set; }

        /// <summary>
        /// Volume Co-Efficient for ALL in game music
        /// </summary>
        public int musicVolume { get; set; }

        public GameInputMode InputMode { get; set; }

        public GamePadStyle GamePadStyle { get; set; }

        public bool FullScreen { get; set; }

        public string DisplayResolutionS { get; set; }

        /// <summary>
        /// The current display resolution.
        /// </summary>
        // public Vector2 DisplayResolution { get; set; } // Not Used

        public bool VSyncEnabled { get; set; }

        public bool SoundsEnabled { get; set; }

        public bool MusicEnabled { get; set; }

        public float SoundVolumeNormalised
        {
            get { return (float)soundVolume / 100; }
        }

        public float MusicVolumeNormalised
        {
            get { return (float)musicVolume / 100; }
        }

        /// <summary>
        /// The debug configuration.
        /// </summary>
        public GameDebugConfiguration DebugConfiguration { get; set; }

        public void SetDefaults()
        {
            // Initialize Defaults
            this.musicVolume = 50;
            this.soundVolume = 80;
            this.SoundsEnabled = true;
            this.MusicEnabled = true;
            this.DisplayResolutionS = "1280 x 720"; // The real default
            // this.DisplayResolution = new Vector2(1280, 720); // Not Used
            this.VSyncEnabled = true;
#if XBOX
            this.InputMode = GameInputMode.GamePad;
#else
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                this.InputMode = GameInputMode.GamePad;
            }
            else
            {
                this.InputMode = GameInputMode.Keyboard;
            }
#endif
            this.GamePadStyle = ZitaAsteria.GamePadStyle.FlightSim;
            this.FullScreen = false; // False for tests
            DebugConfiguration = new GameDebugConfiguration();
            //DebugConfiguration.IsCollisionBoxesDrawn = true;
        }

        static GameConfiguration()
        {
            Resolutions = new Dictionary<string, Vector2>();

            Resolutions.Add("640 x 480", new Vector2(640, 480));
            Resolutions.Add("800 x 600", new Vector2(800, 600));
            Resolutions.Add("1024 x 768", new Vector2(1024, 768));
            Resolutions.Add("1280 x 720", new Vector2(1280, 720));
            Resolutions.Add("1600 x 900", new Vector2(1600, 900));
            Resolutions.Add("1680 x 1050", new Vector2(1680, 1050));
            Resolutions.Add("1920 x 1080", new Vector2(1920, 1080));

        }

        public static List<object> GetDisplayResolutionKeys()
        {
            List<object> keys = new List<object>();

            foreach (string key in Resolutions.Keys)
            {
                keys.Add(key);
            }

            return keys;
        }

        public GameConfiguration()
        {
            SetDefaults();
        }

        public GameConfiguration(GameConfiguration newConfig)
        {
            this.DisplayResolutionS = newConfig.DisplayResolutionS;
            // this.DisplayResolution = newConfig.DisplayResolution; // Not Used
            this.soundVolume = newConfig.soundVolume;
            this.musicVolume = newConfig.musicVolume;
            this.FullScreen = newConfig.FullScreen;
            this.InputMode = newConfig.InputMode;
            this.GamePadStyle = newConfig.GamePadStyle;
            this.VSyncEnabled = newConfig.VSyncEnabled;
        }

    }
}
