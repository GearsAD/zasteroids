using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ZitaAsteria.MenuSystem.World;
using ZitaAsteria.Scene;
using ZitaAsteria;
using ZitaAsteria.MenuSystem.Scene.Shaders;
using ZitaAsteria.MenuSystem.World.Ships;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.World.Effects;
using DPSF;
using ZitaAsteria.World.Effects.ExplosionsSmoke;
using ZAsteroids.World.SPHEREs;
using ZitaAsteria.MenuSystem.World.Satellite;
using ZitaAsteria.World.Effects.Screen_Effects;

namespace ZitaAsteria.MenuSystem
{
    /// <summary>
    /// The core menu system class. Does the drawing and the input handling.
    /// </summary>
    public class MenuSystemScene
    {
        #region Public Properties
        /// <summary>
        /// The current location of the menu scene.
        /// </summary>
        public MenuSceneLocations CurrentLocation {get; set;}
        #endregion

        #region Private Fields
        /// <summary>
        /// The general scene sprite batch.
        /// </summary>
        SpriteBatch sceneSpriteBatch = null;

        /// <summary>
        /// The render targets for the shader calls...
        /// </summary>
        RenderTarget2D shaderRenderTarget;
        /// <summary>
        /// The target texture for the shader calls.
        /// </summary>
        Texture2D shaderTexture;

        BloomShader bloom = new BloomShader();
        #endregion

        public MenuSystemScene()
        {
        }

        /// <summary>
        /// Initialize all the menu system components.
        /// </summary>
        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            MenuContainer.GraphicsDevice = graphicsDevice;
            MenuContainer.ContentManager = contentManager;

            // Perhaps instead of using huge numbers we could scale the objects based on the distance and move the camera less?
            // Would provide the effect of large scale movements? [Alucard - personal note]
            MenuContainer.Camera = new OrbitalCamera(0.5f);// ZAsteroidsCamera(0.5f, 0.9f);
            MenuContainer.Camera.targetCameraLocation = Vector3.One * 15.0f;
            MenuContainer.Camera.farFrustum = 10000000; //Move the frustum out.
            MenuContainer.Camera.fov = 65;

            //// Declare the Particle System Manager to manage the Particle Systems (DPSF)
            MenuContainer.ParticleSystemManager = new ParticleSystemManager();

            MenuContainer.Planet = new Planet();
            MenuContainer.Planet.Initialize();
            MenuContainer.Sun = new Sun();
            MenuContainer.Sun.Initialize();
            MenuContainer.Satellite = new Satellite();
            MenuContainer.Satellite.Initialize();
            MenuContainer.Stars = new Stars();
            MenuContainer.Stars.Initialize();
            MenuContainer.Spheres = new List<SceneObject>();
            MenuContainer.Shield = new Shield();
            MenuContainer.Shield.Initialize();

            //Set up the ships.
            MenuContainer.ShipApproaching = new World.Ships.ApproachShip();
            MenuContainer.ShipApproaching.Initialize();
            MenuContainer.ShipsOrbiting = new List<SceneObject>();
            OrbitingShip
                ship1 = new OrbitingShip(),
                ship2 = new OrbitingShip();
            ship1.Initialize();
            ship2.Initialize();
            MenuContainer.ShipsOrbiting.Add(ship1);
            MenuContainer.ShipsOrbiting.Add(ship2);

            MenuContainer.Asteroids = new List<SceneObject>();
            MenuContainer.AsteroidsNew = new List<Rock>();

            //Put everything in their right places.
            MenuContainer.Sun.Location = Vector3.Zero;
            MenuContainer.Planet.Location = 400000.0f * Vector3.UnitX;
            MenuContainer.Satellite.Location = MenuContainer.Planet.Location + 8000.0f * Vector3.UnitZ + 5000 * Vector3.UnitY + 9000 * Vector3.UnitX;
            MenuContainer.Satellite.ObjectRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.Pi / 6.0f, MathHelper.Pi / 2.0f, MathHelper.Pi / 4.0f);
            MenuContainer.Shield.Location = MenuContainer.Satellite.Location;

            //Put the ships in their right locations.
            MenuContainer.ShipApproaching.Location = MenuContainer.Satellite.Location + 8000.0f * Vector3.UnitZ - 5000 * Vector3.UnitY + 9000 * Vector3.UnitX;
            ship1.Location = MenuContainer.Satellite.Location - 125.0f * Vector3.UnitZ;
            ship2.Location = MenuContainer.Satellite.Location - 75.0f * Vector3.UnitX;

            // Is not pretty, will put in loop later
            double minRad = 1.2f * MenuContainer.Satellite.Model.Meshes[0].BoundingSphere.Radius;
            for (int i = 0; i < 0; i++)
            {
                Rock rock01 = new Rock();
                rock01.Initialize();
                Vector3 offset = new Vector3(
                    (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                    (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                    (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f);
                offset.Normalize();
                offset *= (float)minRad + 2500.0f * (float)ZAMathTools.uniformRandomGenerator.NextDouble();
                rock01.Location = MenuContainer.Satellite.Location + offset;
                MenuContainer.Asteroids.Add(rock01);
            }

            MenuContainer.SpherePlayer = new SPHERE();
            SPHERE sphere = MenuContainer.SpherePlayer;
            sphere.Initialize();
            sphere.Location = MenuContainer.Satellite.Location - 50 * Vector3.UnitX + 50 * Vector3.UnitZ;
            sphere.IsControlledByKeyboard = true;
            sphere.IsUsingDamping = true;
            //Provide a simple setpoint.
            //sphere.SetPositionSetpoint(sphere.Location - Vector3.UnitZ);

            //Set up the shaders.
            MenuContainer.Shaders = new List<ZitaAsteria.World.ShaderEffects.ShaderEffect>();

            ////Create the shader textures
            shaderRenderTarget = new RenderTarget2D(graphicsDevice,
                            graphicsDevice.PresentationParameters.BackBufferWidth,
                            graphicsDevice.PresentationParameters.BackBufferHeight,
                            true,
                            graphicsDevice.PresentationParameters.BackBufferFormat,
                            graphicsDevice.PresentationParameters.DepthStencilFormat,
                            graphicsDevice.PresentationParameters.MultiSampleCount,
                            graphicsDevice.PresentationParameters.RenderTargetUsage);

            shaderTexture = new Texture2D(graphicsDevice, shaderRenderTarget.Width, shaderRenderTarget.Height,
                true, graphicsDevice.PresentationParameters.BackBufferFormat);

            //Set up the ACEs
            MenuContainer.CompoundEffects = new List<AbstractCompoundEffect>();

            //Set up the general shaders.
            sceneSpriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);
            //Add a motion blur
            //MotionBlurShader motion = new MotionBlurShader();
            //motion.Initialize();
            //MenuContainer.Shaders.Add(motion);    

            CrepuscularCombinerShader rays = new CrepuscularCombinerShader();
            rays.Initialize();
            MenuContainer.Shaders.Add(rays);

            // Cant add Bloom like this! [Alucard]
            //BloomShader bloom = new BloomShader();
            bloom.Initialize();
            //MenuContainer.Shaders.Add(bloom);

            //Now set it to intro scene.
            SetMenuSceneLocation(MenuSceneLocations.IntroLocation);
        }

        /// <summary>
        /// Set the menu scene to the specified location.
        /// </summary>
        /// <param name="newLocation"></param>
        public void SetMenuSceneLocation(MenuSceneLocations newLocation)
        {
            CurrentLocation = newLocation;
            if (CurrentLocation == MenuSceneLocations.IntroLocation)
            {
                MenuContainer.Camera.targetLocation = MenuContainer.Camera.targetTargetLocation = MenuContainer.ShipApproaching.Location;
                MenuContainer.Camera.cameraLocation = MenuContainer.ShipApproaching.Location + 500.0f * new Vector3(1.0f, 1.0f, -1.0f);
                MenuContainer.Camera.targetCameraLocation = MenuContainer.ShipApproaching.Location + 60.0f * new Vector3(1.0f, 0.5f, 1.0f);

                //Add a fade-from black filter to the shaders...
                FadeInOutEffect fadein = new FadeInOutEffect();
                fadein.UsedInMenuContainer = true; //IMPORTANT!
                fadein.Initialize();
                fadein.SetPredefinedType(5.0f, PredefinedFadeEffect.BlackFadeIn);
                MenuContainer.CompoundEffects.Add(fadein);
            }
            if (CurrentLocation == MenuSceneLocations.MainMenuLocation)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location - 450.0f * Vector3.UnitZ - 150.0f * Vector3.UnitY;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 650 * Vector3.UnitX - 150.0f * Vector3.UnitY - 190 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
            }
            if (CurrentLocation == MenuSceneLocations.CampaignLocation)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Planet.Location;
                Vector3 satToPlanet = MenuContainer.Planet.Location - MenuContainer.Satellite.Location;
                satToPlanet.Normalize();
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location - satToPlanet * 180.0f;
            }
            if (CurrentLocation == MenuSceneLocations.CharacterSelectionLocation)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.ShipsOrbiting[0].Location;// + Vector3.UnitY * 10.0f + Vector3.UnitZ * 10.0f;
                Vector3 sunToShip = MenuContainer.ShipsOrbiting[0].Location - MenuContainer.Sun.Location;
                sunToShip.Normalize();
                MenuContainer.Camera.targetCameraLocation = 30.0f * sunToShip + MenuContainer.ShipsOrbiting[0].Location + 9.0f * Vector3.UnitY;
            }
            if (CurrentLocation == MenuSceneLocations.LoadingLocation)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location - 95.0f * Vector3.UnitZ - 25.0f * Vector3.UnitY;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 100 * Vector3.UnitX - 45 * Vector3.UnitY - 90 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
            }
            if (CurrentLocation == MenuSceneLocations.OptionsLocation)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location - 95.0f * Vector3.UnitZ - 25.0f * Vector3.UnitY;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 100 * Vector3.UnitX - 45 * Vector3.UnitY - 90 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
            }
            if (CurrentLocation == MenuSceneLocations.InGameMenu)
            {
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location - 95.0f * Vector3.UnitZ - 25.0f * Vector3.UnitY;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 100 * Vector3.UnitX - 45 * Vector3.UnitY - 90 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
            }
            if (CurrentLocation == MenuSceneLocations.ZAsteroidsSatellite)
            {
                Vector3 oldCam = MenuContainer.Camera.cameraLocation;
                Vector3 oldTarget = MenuContainer.Camera.targetLocation;
                MenuContainer.Camera = new ZAsteroidsSatelliteCamera(0.5f, 0.9f);
                MenuContainer.Camera.cameraLocation = oldCam;
                MenuContainer.Camera.targetLocation = oldTarget;
                MenuContainer.Camera.farFrustum = 10000000; //Move the frustum out.
                MenuContainer.Camera.fov = 65;

                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location - 250.0f * Vector3.UnitZ - 25.0f * Vector3.UnitY;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 120 * Vector3.UnitX - 45 * Vector3.UnitY - 120 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
                //MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location + 500.0f * Vector3.UnitZ - 25.0f * Vector3.UnitY;
                //MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 750 * Vector3.UnitX - 45 * Vector3.UnitY - 90 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
            }
            if (CurrentLocation == MenuSceneLocations.ZAsteroidsIntro)
            {
                MenuContainer.Camera.farFrustum = 10000000; //Move the frustum out.
                MenuContainer.Camera.targetTargetLocation = MenuContainer.Satellite.Location;
                MenuContainer.Camera.targetCameraLocation = MenuContainer.Satellite.Location + 140 * Vector3.UnitX -  100 * Vector3.UnitY - 100 * Vector3.UnitZ; //+ 3 * Vector3.UnitY + 10 * Vector3.UnitZ; 
                MenuContainer.Camera.cameraLocation = MenuContainer.Satellite.Location - 130000.0f * Vector3.UnitX - 10000.0f * Vector3.UnitZ + 100 * Vector3.UnitY;
            }
            if (CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres || CurrentLocation == MenuSceneLocations.ZAsteroidsSphereTransition)
            {
                MenuContainer.Camera = new ZAsteroidsSPHERECamera(MenuContainer.Camera, MenuContainer.SpherePlayer);
            }
            if (CurrentLocation == MenuSceneLocations.ControllerInformation)
            {
                //Add a noise effect to bring it back to the space scene.
            }
        }

        public void Update(GameTime gameTime)
        {
            MenuContainer.Camera.Update(gameTime);
            MenuContainer.Satellite.Update(gameTime);
            MenuContainer.Shield.Update(gameTime);
            MenuContainer.Planet.Update(gameTime);
            MenuContainer.ShipApproaching.Update(gameTime);
            for (Int16 i = 0; i < MenuContainer.ShipsOrbiting.Count; i++)
                MenuContainer.ShipsOrbiting[i].Update(gameTime);

            //Update the asteroids.
            for (int sexyRocksControlledByZombies = 0; sexyRocksControlledByZombies < MenuContainer.Asteroids.Count; sexyRocksControlledByZombies++)
            {
                MenuContainer.Asteroids[sexyRocksControlledByZombies].Update(gameTime);
            }
            //Now add the new asteroids to the main ones.
            MenuContainer.Asteroids.AddRange(MenuContainer.AsteroidsNew);
            MenuContainer.AsteroidsNew.Clear();

            //Update the SPHEREs
            for (int i = 0; i < MenuContainer.Spheres.Count; i++)
                MenuContainer.Spheres[i].Update(gameTime);
            MenuContainer.SpherePlayer.Update(gameTime);

            //Run the texture through the current pixel shaders...
            for (int i = 0; i < MenuContainer.Shaders.Count; i++)
                MenuContainer.Shaders[i].Update(gameTime);

            //Update the compound effects...
            for (int i = 0; i < MenuContainer.CompoundEffects.Count; i++)
                MenuContainer.CompoundEffects[i].Update(gameTime);
            // Update All DPSF Particle Systems
            MenuContainer.ParticleSystemManager.UpdateAllParticleSystems((float)gameTime.ElapsedGameTime.TotalSeconds);

            //Now check what needs to be deleted...
            int effectIndex = 0;
            while (effectIndex < MenuContainer.CompoundEffects.Count)
            {
                AbstractCompoundEffect curEffect = MenuContainer.CompoundEffects[effectIndex];
                if (curEffect.ShouldDeleteFromWorldContainer())
                {
                    MenuContainer.CompoundEffects.Remove(curEffect);
                    //Tell it to fire the event.
                    curEffect.Event_TriggerRemovedFromWorldContainer();
                    curEffect.DeleteChildrenFromWorldContainer();
                }
                else
                {
                    effectIndex++;
                }
            }

        }

        public void Draw()
        {
            try
            {
                //Set the render target to the texture so that we can draw the world onto it.
                WorldContainer.graphicsDevice.SetRenderTarget(shaderRenderTarget);

                //Clear the device.
                WorldContainer.graphicsDevice.Clear(Color.Black);
                DepthStencilState depthState = new DepthStencilState();
                depthState.DepthBufferEnable = true;
                WorldContainer.graphicsDevice.DepthStencilState = depthState;

                MenuContainer.Stars.Draw();
                for (int sexyRocksControlledByZombies = 0; sexyRocksControlledByZombies < MenuContainer.Asteroids.Count; sexyRocksControlledByZombies++)
                {
                    MenuContainer.Asteroids[sexyRocksControlledByZombies].Draw();
                }
                MenuContainer.Planet.Draw();
                MenuContainer.Shield.Draw();
                MenuContainer.Satellite.Draw();
                MenuContainer.ShipApproaching.Draw();
                for (int i = 0; i < MenuContainer.ShipsOrbiting.Count; i++)
                    MenuContainer.ShipsOrbiting[i].Draw();
                for (int i = 0; i < MenuContainer.Spheres.Count; i++)
                    MenuContainer.Spheres[i].Draw();
                MenuContainer.SpherePlayer.Draw();
                
                Matrix msViewMatrix = MenuContainer.Camera.GetViewMatrix();
                Matrix msProjectionMatrix = MenuContainer.Camera.GetProjectionMatrix(MenuContainer.GraphicsDevice.Viewport);

                // Set Camera
                MenuContainer.ParticleSystemManager.SetCameraPositionForAllParticleSystems(MenuContainer.Camera.GetRawCameraLocation());

                // Set the World, View, and Projection Matrices for the Particle Systems
                MenuContainer.ParticleSystemManager.SetWorldViewProjectionMatricesForAllParticleSystems(Matrix.Identity, msViewMatrix, msProjectionMatrix);

                // Draw DPSF Particle Systems
                MenuContainer.ParticleSystemManager.DrawAllParticleSystems();

                //Need to reset the render states.
                WorldContainer.graphicsDevice.BlendState = BlendState.AlphaBlend;
                //WorldContainer.graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                //WorldContainer.graphicsDevice.DepthStencilState = WorldContainer.DepthBufferEnabledStencilState;

                WorldContainer.graphicsDevice.SetRenderTarget(null);
                shaderTexture = shaderRenderTarget;
                //Run the texture through the current pixel shaders...
                for (int i = 0; i < MenuContainer.Shaders.Count; i++)
                    shaderTexture = MenuContainer.Shaders[i].DrawSceneWithEffect(shaderTexture);

                shaderTexture = bloom.DrawSceneWithEffect(shaderTexture);

                sceneSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

                ////Render the world onto the screen
                sceneSpriteBatch.Draw(shaderTexture, Vector2.Zero, Color.White);
            }
            catch (Exception ex)
            {
                WorldContainer.graphicsDevice.Clear(Color.Black);
            }
            finally
            {
                try
                {
                    sceneSpriteBatch.End();
                }
                catch (Exception)
                {
                    
                }
                try
                {
                    WorldContainer.graphicsDevice.SetRenderTarget(null);
                }
                catch (Exception) { }
                {
                    
                }
            }
        }
    }
}
