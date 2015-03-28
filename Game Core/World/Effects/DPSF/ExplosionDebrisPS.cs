﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ZitaAsteria;
#endregion

namespace DPSF.ParticleSystems
{
    /// <summary>
    /// Create a new Particle System class that inherits from a
    /// Default DPSF Particle System
    /// </summary>
    //[Serializable]
    public class ExplosionDebrisParticleSystem : DefaultTexturedQuadTextureCoordinatesParticleSystem  // DefaultSprite3DBillboardParticleSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExplosionDebrisParticleSystem(Game game) : base(game) 
        {
        }

        //===========================================================
        // Structures and Variables
        //===========================================================

        /// <summary>
        /// The Color of the explosion.
        /// </summary>
        public Color ExplosionColor { get; set; }

        /// <summary>
        /// The Size of the individual Particles.
        /// </summary>
        public int ExplosionParticleSize { get; set; }

        /// <summary>
        /// The Intensity of the explosion.
        /// </summary>
        public int ExplosionIntensity { get; set; }

        /// <summary>
        /// The Epicentre of the explosion.
        /// </summary>
        public Vector3 Location { get; set; }

        /// <summary>
        /// The Initial Velocity of the particles from the explosion.
        /// </summary>
        public Vector3 initialVelocity {get; set; }

        Rectangle _debris1TextureCoordinates = new Rectangle(256, 256, 39, 44);
        Rectangle _debris2TextureCoordinates = new Rectangle(300, 261, 35, 33);
        Rectangle _debris3TextureCoordinates = new Rectangle(344, 263, 38, 30);
        Rectangle _debris4TextureCoordinates = new Rectangle(259, 302, 37, 35);
        Rectangle _debris5TextureCoordinates = new Rectangle(298, 299, 42, 41);
        Rectangle _debris6TextureCoordinates = new Rectangle(342, 306, 40, 32);
        Rectangle _debris7TextureCoordinates = new Rectangle(257, 345, 39, 36);
        Rectangle _debris8TextureCoordinates = new Rectangle(299, 349, 41, 25);
        Rectangle _debris9TextureCoordinates = new Rectangle(343, 342, 36, 40);

        //===========================================================
        // Initialization Functions
        //===========================================================
        public override void AutoInitialize(GraphicsDevice graphicsDevice, ContentManager contentManager, SpriteBatch cSpriteBatch)
        {
            InitializeTexturedQuadParticleSystem(graphicsDevice, contentManager, 200, 500,
                                                UpdateVertexProperties, WorldContent.effectContent.explosionShockwavePSTexture);  // "Textures/ExplosionParticles"
            //InitializeSpriteParticleSystem(graphicsDevice, contentManager, 200, 500,
            //                                        WorldContent.effectContent.explosionShockwavePSTexture);
            Name = "Explosion - Debris";
            LoadEvents();

            this.AutoMemoryManagerSettings.MemoryManagementMode = AutoMemoryManagerModes.Disabled;
            //this.UpdatesPerSecond = 40;
        }

        public void LoadEvents()
        {
            // Specify the particle initialization function
            ParticleInitializationFunction = InitializeParticleExplosion;

            // Setup the behaviors that the particles should have
            ParticleEvents.RemoveAllEvents();
            ParticleEvents.AddEveryTimeEvent(UpdateParticleVelocityUsingExternalForce);
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationAndRotationalVelocityUsingRotationalAcceleration);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndQuickFadeOut, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            //ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera);

            // Setup the emitter
            Vector3 updatedLocation = Location;
            //updatedLocation.Y += WorldContainer.level.GetTerrainHeightAt(new Vector2(updatedLocation.X, updatedLocation.Z));
            Emitter.PositionData.Position = updatedLocation;
            Emitter.ParticlesPerSecond  = 250;
            Emitter.BurstTime           = 0.05f;
            Emitter.EmitParticlesAutomatically = false; // We will call the Explode() function to release a burst of particles instead of always emitting them
            
            // Set the default explosion settings
            ExplosionColor = new Color(255, 160, 0);
            ExplosionParticleSize = 1;
            ExplosionIntensity = 175;

            //initialVelocity = ; 
        }

        public void SetupToAutoExplodeEveryInterval(float intervalInSeconds)
        {
            // Set the Particle System's Emitter to release a burst of particles after a set interval
            ParticleSystemEvents.RemoveAllEventsInGroup(1);
            ParticleSystemEvents.LifetimeData.EndOfLifeOption = CParticleSystemEvents.EParticleSystemEndOfLifeOptions.Destroy;
            ParticleSystemEvents.LifetimeData.Lifetime = intervalInSeconds;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemToExplode, 0, 1);
        }

        //public void InitializeParticleExplosion(DefaultSprite3DBillboardParticle particle)
        public void InitializeParticleExplosion(DefaultTextureQuadTextureCoordinatesParticle particle)
        {
            particle.Lifetime = RandomNumber.Between(0.2f, 1.25f);
            particle.Color = particle.StartColor = ExplosionColor;
            particle.EndColor = Color.DarkGray;
            particle.Position = Emitter.PositionData.Position;
            particle.ExternalForce = new Vector3(0, -15, 0);
            particle.RotationalVelocity = new Vector3(0, 0, RandomNumber.Between(-MathHelper.PiOver4, MathHelper.PiOver4));
            //particle.RotationalVelocity = RandomNumber.Between(-MathHelper.PiOver4, MathHelper.PiOver4);
            particle.EndSize = ExplosionParticleSize;
            particle.Velocity = DPSFHelper.RandomNormalizedVector() * 0.25f;   // Calculate the direction the particle will travel in

            // We want the debris to travel upwards more often than downward, so if it's travelling downward switch it to travel upward 50% of the time
            if (particle.Velocity.Y < 0 && RandomNumber.Next(0, 2) == 0)
                particle.Velocity.Y *= -1;

            // Set the Particle's Speed
            particle.Velocity *= RandomNumber.Next(25, 50);
            //particle.Velocity *= RandomNumber.Next(100, 150);

            // Randomly pick which texture coordinates to use for this particle
            Rectangle textureCoordinates;
            switch (RandomNumber.Next(0, 9))
            {
                default:
                case 0: textureCoordinates = _debris1TextureCoordinates; break;
                case 1: textureCoordinates = _debris2TextureCoordinates; break;
                case 2: textureCoordinates = _debris3TextureCoordinates; break;
                case 3: textureCoordinates = _debris4TextureCoordinates; break;
                case 4: textureCoordinates = _debris5TextureCoordinates; break;
                case 5: textureCoordinates = _debris6TextureCoordinates; break;
                case 6: textureCoordinates = _debris7TextureCoordinates; break;
                case 7: textureCoordinates = _debris8TextureCoordinates; break;
                case 8: textureCoordinates = _debris9TextureCoordinates; break;
            }

            particle.SetTextureCoordinates(textureCoordinates, Texture.Width, Texture.Height);
            //particle.SetTextureCoordinates(textureCoordinates.Left, textureCoordinates.Top, textureCoordinates.Right, textureCoordinates.Bottom);

            // Set the Width to Heigh ratio so the image isn't skewed when we scale it
            particle.Width  = textureCoordinates.Width;
            particle.Height = textureCoordinates.Height;

            // Set the particle to the specified size, give or take 25%
            particle.ScaleToWidth(ExplosionParticleSize * RandomNumber.Between(0.35f, 0.75f));
        }

        //===========================================================
        // Particle Update Functions
        //===========================================================

        //===========================================================
        // Particle System Update Functions
        //===========================================================
        protected void UpdateParticleSystemToExplode(float elapsedTimeInSeconds)
        {
            Explode();
        }

        //===========================================================
        // Other Particle System Functions
        //===========================================================

        /// <summary>
        /// Start the explosion.
        /// </summary>
        public void Explode()
        {
            this.Emitter.BurstParticles = this.ExplosionIntensity;
        }

        /// <summary>
        /// Change the color of the explosion to a random color.
        /// </summary>
        public void ChangeExplosionColor()
        {
            ExplosionColor = DPSFHelper.RandomColor();
        }

        /// <summary>
        /// Change the color of the explosion to the given color.
        /// </summary>
        /// <param name="color">The color the explosion should be.</param>
        public void ChangeExplosionColor(Color color)
        {
            ExplosionColor = color;
        }
    }
}
