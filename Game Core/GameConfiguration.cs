using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria
{
    public class GameConfiguration
    {

        public int resIndex = 3;
        public int controllerStyleIndex = 0;
        public int inputControllerIndex = 2;
        public int resIndexMax = 8;
        public int csIndexMax = 2;

        public GameConfiguration()
        {
            // Initialize Defaults
            this.musicVolume = 0.5f;
            this.soundVolume = 0.5f;
            this.selectedResolution = Resolutions[resIndex];
        }

        public GameConfiguration(GameConfiguration newConfig)
        {
            this.selectedResolution = newConfig.selectedResolution;
            this.soundVolume = newConfig.soundVolume;
            this.musicVolume = newConfig.musicVolume;
            this.controllerStyleIndex = newConfig.controllerStyleIndex;
            this.resIndex = newConfig.resIndex;
        }

        /// <summary>
        /// Screen resolution for the entire game
        /// </summary>
        public static Vector2[] Resolutions = new Vector2[]
        {
            new Vector2(640,480),
            new Vector2(800,600),
            new Vector2(1024,768),
            new Vector2(1280,720),
            new Vector2(1600,900),
            new Vector2(1680,1050),
            new Vector2(1680,1050),
            new Vector2(1920,1080),
        };
        public Vector2 selectedResolution { get; set; }

        /// <summary>
        /// Volume Co-Efficient to be used for ALL sound effects globally
        /// </summary>
        public float soundVolume { get; set; }

        /// <summary>
        /// Volume Co-Efficient for ALL in game music
        /// </summary>
        public float musicVolume { get; set; }

        /// <summary>
        /// List of different controller key configurations
        /// </summary>
        public static String[] ControllerStyles= new String[]
        {
            "Style1",
            "Style2"
        };

        /// <summary>
        /// List of different input controller
        /// </summary>
        public enum InputController
        {
            GamePad,
            Keyboard
        };

        public static InputController selectedInputControllers = InputController.Keyboard;
    }
}
