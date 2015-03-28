using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZitaAsteria.Configuration
{
    /// <summary>
    /// The game debugging flags.
    /// </summary>
    public class GameDebugConfiguration
    {
        #region Public Properties
        /// <summary>
        /// If true then collision boxes are drawn.
        /// </summary>
        public bool IsCollisionBoxesDrawn { get; set; }
        /// <summary>
        /// If true then the game draws the collision buckets.
        /// </summary>
        public bool IsDrawingCollisionBuckets { get; set; }
        /// <summary>
        /// If true then the game performs collision detection.
        /// </summary>
        public bool IsDoingCollisionDetection { get; set; }

        /// <summary>
        /// Character healths will be shown above the character.
        /// </summary>
        public bool IsCharacterHealthShown { get; set; }
        /// <summary>
        /// If true then ANN debug information is shown.
        /// </summary>
        public bool IsANNDebugShown { get; set; }

        /// <summary>
        /// If true then the console window is visible.
        /// </summary>
        public bool IsConsoleWindowVisible { get; set; }
        /// <summary>
        /// If true then the profiling window is visible.
        /// </summary>
        public bool IsProfilingWindowVisible { get; set; }
        #endregion

        public GameDebugConfiguration()
        {
            IsDoingCollisionDetection = true;
            IsCharacterHealthShown = false;
            IsCollisionBoxesDrawn = false;
            IsANNDebugShown = false;
            IsConsoleWindowVisible = false;
            IsProfilingWindowVisible = false;
        }
    }
}
