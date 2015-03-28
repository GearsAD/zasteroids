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
    /// A static class containing references to all the menu system content. Not necessarily a good idea, can be done better (like with a sorted list), but this is 
    /// both simple and effective.
    /// </summary>
    public class MenuSystemContent
    {
        #region Public Properties
        public Model SatelliteModel { get; private set; }
        public Model PlanetModel { get; private set; }
        public Model StarsModel { get; private set; }
        public Model SPHEREModel { get; private set; }
        public Model RockModel { get; private set; }
        public Model ShieldModel { get; private set; }

        /// <summary>
        /// The ship that is approaching the satellite.
        /// </summary>
        public Model ShipApproach { get; private set; }
        /// <summary>
        /// The ships that are orbiting the station.
        /// </summary>
        public Model ShipOrbiting { get; private set; }

        //Shader Effects
        public Effect BloomCombineEffect { get; private set; }
        public Effect BloomExtractEffect { get; private set; }
        public Effect GaussianBlurEffect { get; private set; }

        public Effect MotionBlurEffect { get; private set; }
        public Effect CrepBlackoutEffect { get; private set; }
        public Effect CrepRaysEffect { get; private set; }
        public Effect CrepCombinerEffect { get; private set; }
        public Texture2D[] LenseFlareTextures { get; private set; }
//        public Model ShipModel { get; private set; }

        /// <summary>
        /// The satellite reticle textures.
        /// </summary>
        public Texture2D[] SatelliteReticle { get; private set; }
        #endregion

        /// <summary>
        /// Nothing happens in the constructor.
        /// </summary>
        public MenuSystemContent()
        {
        }

        /// <summary>
        /// Load and set all the content.
        /// </summary>
        /// <param name="contentManager">The Content property (ContentManager) from GameClass</param>
        public void InitializeFromContent(ContentManager gameContentManager)
        {
            SatelliteModel = gameContentManager.Load<Model>("Satellite\\Main_Menu_Ship_2");
//            ShipModel = gameContentManager.Load<Model>("Ship\\Ship");
            PlanetModel = gameContentManager.Load<Model>("Planet\\Planet");
            StarsModel = gameContentManager.Load<Model>("Stars\\Star_Dome");
            ShieldModel = gameContentManager.Load<Model>("Shield\\SHIELD");

            //The ships.
            ShipApproach = gameContentManager.Load<Model>("Ships\\ApproachShip");
            ShipOrbiting = gameContentManager.Load<Model>("Ships\\OrbitingShip");
            SPHEREModel = gameContentManager.Load<Model>("SPHEREs\\SPHERES02");

            //The most amazing rocks ever
            RockModel = gameContentManager.Load<Model>("Rock\\Big_Rock");

            // Bloom Effects
            BloomCombineEffect = gameContentManager.Load<Effect>("Effects\\BloomCombine");
            BloomExtractEffect = gameContentManager.Load<Effect>("Effects\\BloomExtract");
            GaussianBlurEffect = gameContentManager.Load<Effect>("Effects\\GaussianBlur");

            //Shader Effects
            MotionBlurEffect = gameContentManager.Load<Effect>("Effects\\MotionBlurEffect");
            //Crepuscular effects for compound god rays...
            CrepBlackoutEffect = gameContentManager.Load<Effect>("Effects\\CrepBlackoutLight");
            CrepRaysEffect = gameContentManager.Load<Effect>("Effects\\CrepuscularRays");
            CrepCombinerEffect = gameContentManager.Load<Effect>("Effects\\RaysCombiner");
            
            LenseFlareTextures = new Texture2D[] {
                gameContentManager.Load<Texture2D>("Sun\\Red_Sun"),
                gameContentManager.Load<Texture2D>("Sun\\Yellow_Sun"),
                gameContentManager.Load<Texture2D>("Sun\\Blue_Sun")};

            SatelliteReticle = new Texture2D[2];
            SatelliteReticle[0] = gameContentManager.Load<Texture2D>("Satellite\\TargetPassive");
            SatelliteReticle[1] = gameContentManager.Load<Texture2D>("Satellite\\TargetActive");
        }
    }
}
