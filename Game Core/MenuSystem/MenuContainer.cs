using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.MenuSystem.World;
using ZitaAsteria.Scene;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.World.ShaderEffects;
using Microsoft.Xna.Framework.Content;
using ZitaAsteria.MenuSystem.World.Ships;
using ZitaAsteria.World.Effects;
using DPSF;
using ZAsteroids.World.SPHEREs;
using ZitaAsteria.MenuSystem.World.Satellite;

namespace ZitaAsteria.MenuSystem
{
    /// <summary>
    /// Like the WorldContainer but specifically for the menu.
    /// </summary>
    public static class MenuContainer
    {
        #region Scene Components
        /// <summary>
        /// The satellite.
        /// </summary>
        public static Satellite Satellite;
        /// <summary>
        /// The planet.
        /// </summary>
        public static Planet Planet;
        /// <summary>
        /// The stars.
        /// </summary>
        public static Stars Stars;
        /// <summary>
        /// The sun.
        /// </summary>
        public static Sun Sun;

        public static Shield Shield;

        public static ApproachShip ShipApproaching;

        public static List<SceneObject> ShipsOrbiting { get; set; }

        /// <summary>
        /// The list of spheres in the game.
        /// </summary>
        public static List<SceneObject> Spheres { get; set; }

        /// <summary>
        /// The player sphere
        /// </summary>
        public static SPHERE SpherePlayer { get; set; }
        /// <summary>
        /// The list of shaders applied to the scene.
        /// </summary>
        public static List<ShaderEffect> Shaders { get; set; }

        /// <summary>
        /// The list of ACE's used in the scene.
        /// </summary>
        public static List<AbstractCompoundEffect> CompoundEffects {get; set;}

        /// <summary>
        /// The DPSF Particle System Manager
        /// </summary>
        public static ParticleSystemManager ParticleSystemManager { get; set; }

        /// <summary>
        /// The pretty asteroids controlled by zombies
        /// </summary>
        public static List<SceneObject> Asteroids { get; set; }

        /// <summary>
        /// The new asteroids created by the death of earlier asteroids (they're added here and then added to the main pool after CD).
        /// </summary>
        public static List<Rock> AsteroidsNew { get; set; }

        /// <summary>
        /// The active menu system scene.
        /// </summary>
        public static MenuSystemScene MenuSystemScene { get; set; }

        #endregion

        #region Game Components
        /// <summary>
        /// The camera.
        /// </summary>
        public static OrbitalCamera Camera;
        /// <summary>
        /// The graphics device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice;
        /// <summary>
        /// The game content manager.
        /// </summary>
        public static ContentManager ContentManager;
        #endregion
    }
}
