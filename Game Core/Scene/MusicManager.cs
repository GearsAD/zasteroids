using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// The different types of background music that can play...
    /// </summary>
    public enum BackgroundMusicTypes
    {
        MainMenu,
        SelectionMenu,
        Loading,
        EndLevel,
        Credits,
        Level,
        Nothing,
        IceLevel,
        ZAsteroidsDockIntro,
        ZAsteroidsDockLoop
    }

    /// <summary>
    /// The different types of foreground music that can play...
    /// </summary>
    public enum ForegroundMusicTypes
    {
        BattleLow,
        BattleMed,
        BattleHigh,
        Tension,
        Nothing
    }

    /// <summary>
    /// A class that manages the music being played...
    /// </summary>
    public class MusicManager
    {
        public BackgroundMusicTypes currentBackgroundType { get; protected set; }
        public ForegroundMusicTypes currentForegroundType { get; protected set; }
        Cue
            battleVolcanoCue = null,
            battleIceCue = null;
            //currentCue = null;

        /// <summary>
        /// The different thresholds for choosing the type of battle music...
        /// </summary>
        static double
            lowBattleNPCThreshold = 45,
            medBattleNPCThreshold = 100,
            highBattleNPCThreshold = 150;

        /// <summary>
        /// The minimum time between music changes.
        /// </summary>
        static double minPeriodBetweenForegroundChangesMS = 3000;
        /// <summary>
        /// The time from the last change.
        /// </summary>
        double elapsedForegroundChangeTimeMS = 0;

        /// <summary>
        /// The bool for whether the foreground music is playing.
        /// </summary>
        bool isForegroundPlaying = false;

        public MusicManager()
        {
            setMusicVolume();
            //currentSong = null;
            MediaPlayer.IsRepeating = true;
        }

        public void setMusicVolume()
        {
            if (WorldContainer.gameConfiguration != null)
            {
                MediaPlayer.Volume = WorldContainer.gameConfiguration.MusicVolumeNormalised;
                if (MediaPlayer.Volume > 1.0f)
                {
                    MediaPlayer.Volume = 1.0f;
                }
            }
            else
            {
                MediaPlayer.Volume = 1.0f;
            }
        }

        public void Initialize()
        {
            //battleVolcanoCue = soundBank.GetCue("Battle Volcano");
        }

        public void Update(GameTime gameTime)
        {
        }

        private int MusicScoreWeight()
        {
            int score = 0;
            //for (int i = 0; i < WorldContainer.npcs.Count; i++)
            //{
            //    score += (WorldContainer.npcs[i].TemplateCharacter as NPCTemplate).MusicIncrementPoints;
            //}
            return score;
        }

        /// <summary>
        /// Set the background music type - this is for stuff that isn't XACT-based!
        /// </summary>
        /// <param name="type"></param>
        public void SetBackgroundToNewType(BackgroundMusicTypes type)
        {
            if (type == BackgroundMusicTypes.Credits)
                MediaPlayer.Play(WorldContent.musicContent.endCreditsSong);
            if (type == BackgroundMusicTypes.MainMenu)
                MediaPlayer.Play(WorldContent.musicContent.mainMenuSong);
            if (type == BackgroundMusicTypes.SelectionMenu)
                MediaPlayer.Play(WorldContent.musicContent.selectionMenuSong);
            if (type == BackgroundMusicTypes.EndLevel)
                MediaPlayer.Play(WorldContent.musicContent.endCreditsSong);
            if (type == BackgroundMusicTypes.IceLevel)
                MediaPlayer.Play(WorldContent.musicContent.planetBackgroundIce);
            if (type == BackgroundMusicTypes.ZAsteroidsDockIntro)
                MediaPlayer.Play(WorldContent.musicContent.zaBlueDanubeIntro);
            if (type == BackgroundMusicTypes.ZAsteroidsDockLoop)
                MediaPlayer.Play(WorldContent.musicContent.zaBlueDanubeLoop);
            if (type == BackgroundMusicTypes.Loading)
                MediaPlayer.Play(WorldContent.musicContent.mainMenuSong);
            if (type == BackgroundMusicTypes.Nothing)
                MediaPlayer.Stop();
        }

        /// <summary>
        /// Start the foreground music.
        /// </summary>
        public void StartForegroundMusic()
        {
            SetForegroundToNewType(ForegroundMusicTypes.Tension);
            isForegroundPlaying = true;
            elapsedForegroundChangeTimeMS = 0;
        }

        /// <summary>
        /// Stop the foreground music.
        /// </summary>
        public void StopForegroundMusic()
        {
            SetForegroundToNewType(ForegroundMusicTypes.Nothing);
            isForegroundPlaying = false;
            elapsedForegroundChangeTimeMS = 0;
        }

        /// <summary>
        /// Set the foreground music type - this is the XACT-based stuff!
        /// </summary>
        /// <param name="type"></param>
        private void SetForegroundToNewType(ForegroundMusicTypes type)
        {
            ////Update the type...
            //ForegroundMusicTypes oldMusicType = currentForegroundType;
            //currentForegroundType = type;

            ////Don't change it if it's asking to be set...
            //if (currentForegroundType == oldMusicType) return;
        }

    }
}
