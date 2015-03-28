using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ZitaAsteria.World.Effects;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.Scene;

using DPSF;

namespace ZitaAsteria
{
    /// <summary>
    /// This is the general world class, where all the important information regarding the game is contained. It's static so that we can only have a single
    /// instance of it.
    /// </summary>
    public static class WorldContainer
    {
        // No longer used
        //public static EffectsCacheMgr EffectsCacheMgr { get; set; }

        /// <summary>
        /// The list of all the shaders used in the game.
        /// </summary>
        public static List<ShaderEffect> shaders { get; set; }

        /// <summary>
        /// The list of compound effects - these aren't drawn but they control items in the normal lists.
        /// </summary>
        public static List<AbstractCompoundEffect> compoundEffects { get; set; }

        /// <summary>
        /// The base gameclass.
        /// </summary>
        public static Game gameClass { get; set; }

        /// <summary>
        /// The current game time - this is updated in the GameClass.Update() method so it's always the most recent value. Although we pass gameTime through
        /// in all the update methods, its good to have it available.
        /// </summary>
        public static GameTime currentGameTime { get; set; }

        /// <summary>
        /// The game's camera.
        /// </summary>
        public static ICamera gameCamera { get; set; }

        /// <summary>
        /// Contains Global Game Configuration
        /// </summary>
        public static GameConfiguration gameConfiguration { get; set; }

        /// <summary>
        /// The GraphicsDevice for the game.
        /// </summary>
        public static GraphicsDevice graphicsDevice { get; set; }

        /// <summary>
        /// The GraphicsDeviceManager for the game.
        /// </summary>
        public static IGraphicsDeviceService graphicsDeviceManager { get; set; }

        /// <summary>
        /// The music manager for the game.
        /// </summary>
        public static ZitaAsteria.Scene.MusicManager musicManager { get; set; }

        /// <summary>
        /// Sound Effects Mgr for ALL Sound Effects.
        /// </summary>
        public static SoundEffectsMgr soundEffectsMgr { get; set; }

        /// <summary>
        /// The DPSF Particle System Manager
        /// </summary>
        public static ParticleSystemManager particleSystemManager { get; set; }

        /// <summary>
        /// The Reference to the general-purpose spritebatch.
        /// </summary>
        public static SpriteBatch spriteBatch { get; set; }

        //The list of joined players
        public static Dictionary<int, bool> PlayersJoined { get; set; }

        public static GameConfigurationManager ConfigurationManager { get; set; }

        public static DepthStencilState DepthBufferEnabledStencilState { get; set; }

        public static DepthStencilState DepthBufferDisabledStencilState { get; set; }
        /// <summary>
        /// Toggles the level between desgin mode and game mode. This is used by the the level designer which will set it to true
        /// </summary>
        public static bool DesignMode
        {
            get;
            set;
        }

        /// <summary>
        /// Enables or disables profiling
        /// </summary>
        public static bool ProfilingEnabled { get; set; }

        public static bool ShowFrameRate { get; set; }

        /// <summary>
        /// If true then draw the loading screen.
        /// </summary>
        public static bool IsGameLoading { get; set; }

        static WorldContainer()
        {
        }

        public static object GetWorldContainerListFromType(Type physicsWorldObjectType)
        {
            PropertyInfo[] properties = typeof(WorldContainer).GetProperties();
            object worldContainerList = null;

            var q = from property in properties
                    where property.PropertyType.Name == "List`1"
                    select property;

            foreach (PropertyInfo pInfo in q)
            {
                Type[] genericArgs = pInfo.PropertyType.GetGenericArguments();
                if (genericArgs.Length > 0)
                {
                    Type genericType = genericArgs[0];
                    if (genericType == physicsWorldObjectType || genericType == physicsWorldObjectType.BaseType)
                    {
                        worldContainerList = pInfo.GetValue(null, null);
                        if (worldContainerList == null)
                        {
                            worldContainerList = pInfo.PropertyType.GetConstructor(new Type[] { }).Invoke(null);
                        }
                        break;
                    }
                }
            }

            return worldContainerList;
        }

        public static void InitialiseAllProperties()
        {
            PropertyInfo[] properties = typeof(WorldContainer).GetProperties();

            var q = from property in properties
                    where property.PropertyType.Name == "List`1" || property.PropertyType.Name == "ParticleSystemManager"
                    select property;

            foreach (PropertyInfo pInfo in q)
            {
                pInfo.SetValue(null, pInfo.PropertyType.GetConstructor(new Type[] { }).Invoke(null), null);
            }
        }
    }
}
