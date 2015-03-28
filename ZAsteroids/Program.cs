using System;

namespace ZitaAsteria
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                using (ZAsteroidsGameClass game = new ZAsteroidsGameClass())
                {
                    game.Window.Title = "ZAsteroids";

                    game.Run();
                }
            }
            else
            try
            {
                using (ZAsteroidsGameClass game = new ZAsteroidsGameClass())
                {
                    game.Window.Title = "ZAsteroids";

                    game.Run();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "ZAsteroids 0.8 Error\r\n\r\n" +
                    "Message = " + ex.Message + "\r\n" +
                    "Stack trace = \r\n" + ex.StackTrace);
            }
        }
    }
}