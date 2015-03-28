using Microsoft.Xna.Framework;

using ZitaAsteria.MenuSystem;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the main game class for Pilot Project
    /// </summary>
    public partial class ZAsteroidsGameClass : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Used for different drawing methods in derived game classes
        /// </summary>
        /// <param name="gameTime"></param>
        protected void DrawBase(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Clear the device...
            GraphicsDevice.Clear(Color.Black);

            MenuContainer.MenuSystemScene.Draw();

            //Hmm, maybe do this in the game manager [GearsAD]
            // lol, Should do in the game manager. Will move in a bit. [Alucard]
            if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
            {
                _hudModeInfo.Draw();
                _upgrade.Draw();
                _score.Draw();
                _hudSphere.Draw();
                _hudControls.Draw();
            }
            if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
            {
                _hudDamage.Draw();
                _hudModeInfo.Draw();
                _shields.Draw();
                _general.Draw();
                _crosshair.Draw();
                _score.Draw();
                _hudSatellite.Draw();
                _hudControls.Draw();
            }
            if (MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsIntro)
            {
                _hudIntroScreen.Draw();
                _hudControls.Draw();
            }

            // Why is there Profiling code here?
            // I know that my RelativeTexture class is expensive since i dont cache anything in it.
            // Will fix that. [Alucard]
            //flameshield.Draw();
            //Debugging time profile
            if (WorldContainer.ProfilingEnabled)
            {
                frameRateCounter.drawTwoDEndMS = System.Environment.TickCount;

                frameRateCounter.drawOverlayEndMS = System.Environment.TickCount;
            }

            if (WorldContainer.ProfilingEnabled || WorldContainer.ShowFrameRate)
                //Draw the framerate...
                frameRateCounter.Draw(gameTime);
        }
    }
}
