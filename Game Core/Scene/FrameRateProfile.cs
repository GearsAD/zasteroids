using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ZitaAsteria.Scene
{
    /// <summary>
    /// http://blogs.msdn.com/b/shawnhar/archive/2007/06/08/displaying-the-framerate.aspx
    /// </summary>
    public class FrameRateProfile
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        StringBuilder fps = new StringBuilder();
        TimeSpan elapsedTime = TimeSpan.Zero;

        public int Updates { get; set; }

        public int
            loadContentStartMS = 0,
            loadContentEndMS = 0,
            updateStartMS = 0,
            updateEndMS = 0,
            drawTerrainStartMS = 0,
            drawTerrainEndMS = 0,
            drawTwoDStartMS = 0,
            drawTwoDEndMS = 0,
            drawShadersStartMS = 0,
            drawShadersEndMS = 0,
            drawOverlayStartMS = 0,
            drawOverlayEndMS = 0,
            drawDPSFStartMS = 0,
            drawDPSFEndMS = 0;

        public int
            updateProjectilesStartMS = 0,
            updateProjectilesEndMS = 0,
            updateReticlesStartMS = 0,
            updateReticlesEndMS = 0,
            updateItemsStartMS = 0,
            updateItemsEndMS = 0,
            updateExplosionsStartMS = 0,
            updateExplosionsEndMS = 0,
            updateShadersStartMS = 0,
            updateShadersEndMS = 0,
            updateParticleSystems1StartMS = 0,
            updateParticleSystems1EndMS = 0,
            updateBuildingsStartMS = 0,
            updateBuildingsEndMS = 0,
            updateTriggersStartMS = 0,
            updateTriggersEndMS = 0,
            updateSpawnPointsStartMS = 0,
            updateSpawnPointsEndMS = 0,
            updateParticleSystems2StartMS = 0,
            updateParticleSystems2EndMS = 0,
            updateHudControllerStartMS = 0,
            updateHudControllerEndMS = 0,
            updateNPCsStartMS = 0,
            updateNPCsEndMS = 0,
            updateCompoundEffectsStartMS = 0,
            updateCompoundEffectsEndMS = 0,
            updateObjectivesStartMS = 0,
            updateObjectivesEndMS = 0,
            updatePlayerStartMS = 0,
            updatePlayerEndMS = 0,
            updateGameBaseStartMS = 0,
            updateGameBaseEndMS = 0,
            updateMapStartMS = 0,
            updateMapEndMS = 0,
            updateCDStartMS = 0,
            updateCDEndMS = 0;

        public Dictionary<string, ProfileEntry> ProfileEntries = new Dictionary<string, ProfileEntry>();

        string updateProfile;

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);
            spriteFont = WorldContent.contentManager.Load<SpriteFont>("Fonts\\SpriteFont1");
        }

        public void Hello()
        {
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;

                if (fps.Length > 0)
                    fps.Remove(0, fps.Length);

                fps.Append(frameRate.ToString());

                if (WorldContainer.ProfilingEnabled)
                {
                    foreach (ProfileEntry entry in ProfileEntries.Values)
                    {
                        fps.Append(entry.Name); fps.Append(" : "); fps.Append(entry.GetIntervalMS().ToString());
                        fps.AppendLine();
                    }
                }
            }
        }


        public void Draw(GameTime gameTime)
        {
            frameCounter++;

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps.ToString(), new Vector2(400, 10), Color.White);

            //spriteBatch.DrawString(spriteFont, updateProfile, new Vector2(600, 10), Color.White);

            spriteBatch.End();
        }

        public void SetProfileEntry(string key, ProfileEntry profileEntry)
        {
            if (!this.ProfileEntries.ContainsKey(key))
                ProfileEntries.Add(key, profileEntry);
            else
                ProfileEntries[key] = profileEntry;
        }
    }
}
