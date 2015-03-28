using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.MenuSystem.World;

namespace ZitaAsteria.World
{
    /// <summary>
    /// The asteroid generator - creates the asteroid and maintains the game environment.
    /// </summary>
    public class AsteroidGenerator
    {
        #region Public Properties
        /// <summary>
        /// The location that the asteroids are generated from.
        /// </summary>
        public Vector3 Location { get; set; }

        /// <summary>
        /// The start direction of the asteroids.
        /// </summary>
        public Vector3 StartDirection { get; set; }

        /// <summary>
        /// The radius of the generator.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// The period between generations.
        /// </summary>
        public float AsteroidPeriodS { get; set; }

        /// <summary>
        /// The minimum velocity.
        /// </summary>
        public float VelocityMinMS { get; set; }
        /// <summary>
        /// The maxmimum velocity.
        /// </summary>
        public float VelocityMaxMS { get; set; }

        /// <summary>
        /// If true then is generating asteroids.
        /// </summary>
        public bool IsActive { get; set; }
        #endregion

        #region Private Fields
        float _accumulatedTimeS = 0;
        #endregion

        public AsteroidGenerator()
        {
            //Default numbers.
            //Set the location to 97.5% of dist from sat to sun.
            Location = 0.975f * (MenuContainer.Satellite.Location - MenuContainer.Sun.Location);
            //Head to the satellite.
            StartDirection = Vector3.Normalize(Location);
            //Arbitrary field size.
            Radius = 250;
            //Start period.
            AsteroidPeriodS = 5f;
            VelocityMinMS = 100f;
            VelocityMaxMS = 300f;
        }

        /// <summary>
        /// Update the generator.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                _accumulatedTimeS += (float)gameTime.ElapsedGameTime.TotalSeconds;

                while (_accumulatedTimeS > AsteroidPeriodS)
                {
                    GenerateAsteroid();
                    _accumulatedTimeS -= AsteroidPeriodS;
                }
            }
        }

        /// <summary>
        /// Generate an asteroid with the given parameters.
        /// </summary>
        private void GenerateAsteroid()
        {
            Rock rock = ObjectManager.GetObject<Rock>();
            rock.IsLifetimeLimited = true;
            rock.Initialize(); 

            //Generate the location.
            Vector3 pointer = new Vector3(
                (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f);
            pointer.Normalize();
            pointer *= Radius; //That gives the point of the spherical generation radius.
            rock.Location = Location + pointer;

            //Generate the velocity.
            rock.Velocity = StartDirection * (float)(VelocityMinMS + (VelocityMaxMS - VelocityMinMS) * ZAMathTools.uniformRandomGenerator.NextDouble());

            MenuContainer.Asteroids.Add(rock);
        }

        /// <summary>
        /// Update to next level.
        /// </summary>
        public void SetToNextLevel()
        {
            AsteroidPeriodS *= 0.9f;
            Radius *= 1.1f;
            VelocityMinMS *= 1.05f;
            VelocityMaxMS *= 1.05f;
        }
    }
}
