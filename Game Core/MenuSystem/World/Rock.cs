using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZAsteroids.World.Effects.ExplosionSmoke;
using Microsoft.Xna.Framework.Audio;
using ZitaAsteria.MenuSystem.World.Collision_Objects;

namespace ZitaAsteria.MenuSystem.World
{
    public class Rock : SceneObject
    {
        /// <summary>
        /// If true then will be drawn to the satellite.
        /// </summary>
        public static bool ISGRAVITATIONALLYATTRACTED_TOSATELLITE = false;

        /// <summary>
        /// If true then the lifespan is limited to 120 seconds.
        /// </summary>
        public bool IsLifetimeLimited { get; set; }

        /// <summary>
        /// The number of times this can break before it disappears.
        /// </summary>
        public int SizesTillBreak { get; set; }

        #region Private Fields
        /// <summary>
        /// The rotational velocity.
        /// </summary>
        Vector3 _rotVel = 0.05f * new Vector3(
            (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
            (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
            (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f);

        int _originalLife = 100;

        /// <summary>
        /// The asteroids lifetime.
        /// </summary>
        float _lifetime = 0;
        #endregion

        public Rock()
        {
            SizesTillBreak = 3;
        }

        public Rock(int sizesTillBreak)
        {
            SizesTillBreak = sizesTillBreak;
        }

        public override void Initialize()
        {
             base.Initialize();

            //Load the model.
            this.Model = WorldContent.menuSystemContent.RockModel;
            float scaleFactor = (float)ZAMathTools.uniformRandomGenerator.NextDouble();
            this.Scale = scaleFactor * Scale;

            //Get the radius from the model.
            float _radius = this.Model.Meshes[0].BoundingSphere.Radius * 0.7f * scaleFactor; //Scale it down a little to be conservative.

            //Create one bounding sphere to go with the normal bounding sphere.
            this._cdBoundingSphere = new BoundingSphere(Vector3.Zero, _radius);
            //Add a parallel sphere for the satellite to rock collision
            this._cdCollisionContainers.Add(new CollisionSphere(Vector3.Zero, _radius));
            _cdIsInitialized = true; //Force the collision sphere to not update in the SceneObject method.

        }

        public override void DieEffect(Vector3 locationOfCollision)
        {
            base.DieEffect(locationOfCollision);
            //Create an explosion.
            IncendiaryExplosionACE expl = ObjectManager.GetObject<IncendiaryExplosionACE>();
            expl.Location3D = this.Location;
            expl.Velocity3D = this.Velocity;
            expl.Reset();
            expl.AddChildrenToMenuContainer();
            MenuContainer.CompoundEffects.Add(expl);

            //Play the sound effect.
            WorldContainer.soundEffectsMgr.PlaySoundEffectInZAsteroids(WorldContent.sfxContent.ShockExplosionInSpace, this.Location);

            //Spawn some children if done
            if (SizesTillBreak > 0)
            {
                SizesTillBreak--;
                int numberFragments = 5;// ZAMathTools.uniformRandomGenerator.Next(0, 10);
                for (int i = 0; i < numberFragments; i++)
                {
                    Rock rock = new Rock();
                    //Move the rocks around.
                    rock.Location = this.Location;// + 
                        //this._cdBoundingSphere.Radius * 
                        //(1.0f * new Vector3(
                        //        (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                        //        (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f,
                        //        (float)ZAMathTools.uniformRandomGenerator.NextDouble() - 0.5f));
                    rock.Initialize();
                    rock.Location = this.Location;
                    rock.Life = _originalLife /=numberFragments;
                    float maxVel = this.Velocity.Length();
                    rock.Velocity =
                        this.Velocity +
                        (float)ZAMathTools.uniformRandomGenerator.NextDouble() * (ZAMathTools.uniformRandomGenerator.NextDouble() > 0.5 ? Vector3.Cross(this.Velocity, Vector3.Up) : Vector3.Cross(Vector3.Up, this.Velocity)) +
                        (float)ZAMathTools.uniformRandomGenerator.NextDouble() * (ZAMathTools.uniformRandomGenerator.NextDouble() > 0.5 ? Vector3.Cross(this.Velocity, Vector3.Right) : Vector3.Cross(Vector3.Right, this.Velocity)) +
                        (float)ZAMathTools.uniformRandomGenerator.NextDouble() * (ZAMathTools.uniformRandomGenerator.NextDouble() > 0.5 ? Vector3.Cross(this.Velocity, Vector3.Forward) : Vector3.Cross(Vector3.Forward, this.Velocity));
                    rock.Velocity = Vector3.Normalize(rock.Velocity);
                    rock.Velocity *= maxVel;
                    rock.Scale = this.Scale / 3;

                    //These are added to the main pool after CD.
                    MenuContainer.Asteroids.Add(rock);
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            //Now apply the rotations.
            ObjectRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, _rotVel.X);
            ObjectRotation.Normalize();
            ObjectRotation *= Quaternion.CreateFromAxisAngle(-Vector3.UnitY, _rotVel.Y);
            ObjectRotation.Normalize();
            ObjectRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _rotVel.Z);
            ObjectRotation.Normalize();

            _lifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (IsLifetimeLimited)
                if (_lifetime > 120) MenuContainer.Asteroids.Remove(this);
        }
    }
}
