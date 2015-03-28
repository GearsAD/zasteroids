using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared_Content;
using Microsoft.Xna.Framework.Media;

namespace ZitaAsteria.ContentSections
{

    /// <summary>
    /// A static class containing references to all the music content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public class MusicContent
    {
        public Song mainMenuSong = null;
        public Song selectionMenuSong = null;
        public Song endCreditsSong = null;
        public Song planetBackgroundIce = null;
        public Song planetVolcano = null;
        public Song planetDust = null;
        public Song zaBlueDanubeIntro = null;
        public Song zaBlueDanubeLoop = null;

        /// <summary>
        /// Nothing happens in the constructor.
        /// </summary>
        public MusicContent()
        {
        }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        public void InitializeFromContent(ContentManager gameContentManager)
        {
            mainMenuSong = gameContentManager.Load<Song>("Music\\Game Theme & Menu");
            selectionMenuSong = gameContentManager.Load<Song>("Music\\Selection Menu");
            endCreditsSong = gameContentManager.Load<Song>("Music\\End Credits");
            zaBlueDanubeIntro = gameContentManager.Load<Song>("Music\\ZAsteroids\\Blue Danube Intro");
            zaBlueDanubeLoop = gameContentManager.Load<Song>("Music\\ZAsteroids\\Blue Danube Loop");
        }
    }
}
