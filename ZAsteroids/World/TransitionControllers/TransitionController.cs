using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects.Screen_Effects;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ZAsteroids.World.TransitionControllers
{
    /// <summary>
    /// A simple transition controller class - very basic for a simple game.
    /// </summary>
    abstract class TransitionController
    {
        #region Protected Fields
        /// <summary>
        /// The game manager.
        /// </summary>
        protected GameManager _gameManager = null;

        /// <summary>
        /// If true then can skip to end.
        /// </summary>
        protected bool _isPossibleToSkip = false;

        /// <summary>
        /// The elapsed transition time.
        /// </summary>
        protected float _elapsedTransitionTimeS = 0;

        /// <summary>
        /// Fired when the transition is completed.
        /// </summary>
        public event EventHandler IsCompleted;
        #endregion

        #region Private Fields
        KeyboardState _prevKState;
        GamePadState _prevGState;
        #endregion

        public TransitionController(GameManager gameManager)
        {
            this._gameManager = gameManager;

            SetupInitialScene();
        }

        /// <summary>
        /// Set up the initial scene.
        /// </summary>
        public abstract void SetupInitialScene();

        /// <summary>
        /// Check that the skip scene hasn't been pressed.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            _elapsedTransitionTimeS += (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState kstate = Keyboard.GetState();
            GamePadState gstate = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

            if(_elapsedTransitionTimeS > 1.0f)
                if (_isPossibleToSkip &&
                    (
                        (gstate.IsConnected && (gstate.IsButtonUp(Buttons.Start) && _prevGState.IsButtonDown(Buttons.Start))
                        ||
                        (kstate.IsKeyUp(Keys.Space) && _prevKState.IsKeyDown(Keys.Space)))))
                {
                    SetupEndOfScene();
                }

            _prevKState = kstate;
            _prevGState = gstate;
        }

        /// <summary>
        /// Set up the end scene and or change the menu system state.
        /// </summary>
        public virtual void SetupEndOfScene()
        {
            if (IsCompleted != null)
                IsCompleted(this, new EventArgs());
        }

    }
}
