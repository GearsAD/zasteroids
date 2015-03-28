using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared_Content;
using ZitaAsteria.ContentSections;
using ZitaAsteria;
using SharedContent;

namespace ZitaAsteria
{

    /// <summary>
    /// A static class containing references to all the content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public static class WorldContent
    {
        //Put all textures and stuff here - one global place for everything...

        public static SFXContent sfxContent { get; set; }
        public static MusicContent musicContent { get; set; }
        public static EffectContent effectContent { get; set; }
        public static ItemContent itemContent { get; set; }
        public static MenuSystemContent menuSystemContent { get; set; }

        // HUD stuff
        public static HUDContent hudContent { get; set; }

        #region Fonts

        /// <summary>
        /// The general font for text.
        /// </summary>
        public static SpriteFont fontAL18pt { get; set; }
        /// <summary>
        /// The general font for text.
        /// </summary>
        public static SpriteFont fontAL15pt { get; set; }
        /// <summary>
        /// The small font for text.
        /// </summary>
        public static SpriteFont fontAL12pt { get; set; }

        /// <summary>
        /// Part of XNA 4.0 Pack - Should use QUARTZ
        /// </summary>
        public static SpriteFont fontAL28pt { get; set; }

        /// <summary>
        /// A standard debug font.
        /// </summary>
        public static SpriteFont fontArial10pt { get; set; }

        /// <summary>
        /// A standard debug font.
        /// </summary>
        public static SpriteFont fontArialBold12pt { get; set; }        
        #endregion

        #region Content Managers.
        /// <summary>
        /// The general content manager.
        /// </summary>
        public static ContentManager contentManager { get; set; }

        public static ContentManager menuContentManager { get; set; }

        public static ContentManager baseContentManager { get; set; }

        public static ContentManager sfxContentManager { get; set; }

        public static ContentManager effectContentManager { get; set; }

        #endregion


        public static SpriteFont fontKootenay11pt { get; set; }

        public static void InitializeContent(GraphicsDevice graphicsDevice, ContentInitializationTypes contentTypes)
        {
            Shared_Content.ServiceContainer serviceContainer = new Shared_Content.ServiceContainer();
            GraphicsDeviceService graphicsDeviceService = new GraphicsDeviceService(graphicsDevice);

            serviceContainer.AddService<IGraphicsDeviceService>(graphicsDeviceService);

            ContentManager contentMan = new ContentManager(serviceContainer, Environment.CurrentDirectory + "\\Content");

            if (((int)contentTypes & (int)ContentInitializationTypes.MenuContent) != 0)
                menuContentManager = new ContentManager(serviceContainer, Environment.CurrentDirectory + "\\Menu System Content");

            if (baseContentManager == null)
                baseContentManager = new ContentManager(serviceContainer, Environment.CurrentDirectory + "\\BaseContent");

            if (sfxContentManager == null)
                sfxContentManager = new ContentManager(serviceContainer, Environment.CurrentDirectory + "\\SFXContent");

            if (effectContentManager == null)
                effectContentManager = new ContentManager(serviceContainer, Environment.CurrentDirectory + "\\EffectContent");

            InitializeFromContent(contentMan, contentTypes);

        }

        public static void InitializeContent(ContentManager contentManger, GraphicsDevice deviceService, ContentInitializationTypes contentTypes)
        {
            Shared_Content.ServiceContainer serviceContainer = new Shared_Content.ServiceContainer();
            GraphicsDeviceService graphicsDeviceService = new GraphicsDeviceService(deviceService);
            serviceContainer.AddService<IGraphicsDeviceService>(graphicsDeviceService);

            if (menuContentManager == null)
                if (((int)contentTypes & (int)ContentInitializationTypes.MenuContent) != 0)
                    menuContentManager = new ContentManager(serviceContainer, "Menu System Content");
            // Fucking content
            if (baseContentManager == null)
                baseContentManager = new ContentManager(serviceContainer, "BaseContent");
            if (sfxContentManager == null)
                sfxContentManager = new ContentManager(serviceContainer, "SFXContent");
            if (effectContentManager == null)
                effectContentManager = new ContentManager(serviceContainer, "EffectContent");
            InitializeFromContent(contentManger, contentTypes);
        }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        private static void InitializeFromContent(ContentManager gameContentManager, ContentInitializationTypes contentTypes)
        {
            //Set the content manager.
            contentManager = gameContentManager;

            //Load the generic font for the WorldContainer static class
            fontAL12pt = baseContentManager.Load<SpriteFont>("Fonts\\AlienLeague12pt");
            fontAL15pt = baseContentManager.Load<SpriteFont>("Fonts\\AlienLeague15pt");
            fontAL18pt = baseContentManager.Load<SpriteFont>("Fonts\\AlienLeague18pt");
            fontAL28pt = baseContentManager.Load<SpriteFont>("Fonts\\AlienLeague28pt");
            fontKootenay11pt = baseContentManager.Load<SpriteFont>("Fonts\\Kootenay11pt");
            fontArial10pt = baseContentManager.Load<SpriteFont>("Fonts\\Arial10pt");
            fontArialBold12pt = baseContentManager.Load<SpriteFont>("Fonts\\ArialBold12pt");

            if (((int)contentTypes & (int)ContentInitializationTypes.SfxContent) != 0)
            {
                sfxContent = new SFXContent();
                sfxContent.InitializeFromContent(sfxContentManager);
            }
            if (((int)contentTypes & (int)ContentInitializationTypes.MusicContent) != 0)
            {
                musicContent = new MusicContent();
                WorldContent.musicContent.InitializeFromContent(sfxContentManager);
            }
            if (((int)contentTypes & (int)ContentInitializationTypes.EffectsContent) != 0)
            {
                effectContent = new EffectContent();
                effectContent.InitializeFromContent(effectContentManager);
            }
            if (((int)contentTypes & (int)ContentInitializationTypes.ItemContent) != 0)
            {
                itemContent = new ItemContent();
                itemContent.InitializeFromContent(gameContentManager);
            }
            if (((int)contentTypes & (int)ContentInitializationTypes.MenuContent) != 0)
            {
                menuSystemContent = new MenuSystemContent();
                menuSystemContent.InitializeFromContent(menuContentManager);

                // HUD stuff
                hudContent = new HUDContent();
                hudContent.InitializeFromContent(menuContentManager);
                // Pls Explain the below, no idea how this works [Alucard]
            }
        }
    }
}

