using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.MenuSystem.World;
using ZitaAsteria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ZitaAsteria.MenuSystem.World.Collision_Objects;
using ZAsteroids.World.Effects.ExplosionSmoke;
using ZitaAsteria.MenuSystem;

namespace ZAsteroids.World.SPHEREs
{
    public class SPHERE : SceneObject
    {
        #region Public Properties
        /// <summary>
        /// The mass of the sphere.
        /// </summary>
        public float MassKG {get; set;}
        /// <summary>
        /// The inertia of the SPHERE - calculated in Initialize().
        /// </summary>
        public float Inertia {get; set;}

        public PneumaticActuator[] Actuators {get; set;}

        /// <summary>
        /// The linear acceleration.
        /// </summary>
        public Vector3 AccelerationLinear { get; set; }

        /// <summary>
        /// The rotational acceleration.
        /// </summary>
        public Vector3 AccelerationRotat { get; set; }

        /// <summary>
        /// The rotational velocity - adding it in here.
        /// </summary>
        public Vector3 VelocityRotat { get; set; }

        /// <summary>
        /// If true then the SPHERE is controlled by the keyboard.
        /// </summary>
        public bool IsControlledByKeyboard { get; set; }

        /// <summary>
        /// If true then the model is linearly and rotationally damped, i.e. easier to control for ppl who don't want a hyperreal model [GearsAD]
        /// </summary>
        public bool IsUsingDamping { get; set; }
        
        #endregion

        #region Private Physics Constants
        /// <summary>
        /// The radius of the sphere
        /// </summary>
        float _radius = 0; //Pulled from the model mesh.
        #endregion

        #region Private Fields
        /// <summary>
        /// The last keyboard state.
        /// </summary>
        KeyboardState _lastKeyState;

        /// <summary>
        /// The last gamepad state.
        /// </summary>
        private GamePadState _lastGamePadState;
        #endregion
        public SPHERE()
        {
            MassKG = 5;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.IsDrawing = true;

            //Set the model.
            this.Model = WorldContent.menuSystemContent.SPHEREModel;
            //Get the radius from the model.
            _radius = this.Model.Meshes[0].BoundingSphere.Radius;

            //Create one bounding sphere to go with the normal bounding sphere.
            this._cdCollisionContainers.Add(new CollisionSphere(Vector3.Zero, _radius));

            //Calculate the intertia of a sphere of the given radius and weight assuming homogenous material/density.
            //I_sphere = 2/5 * m * R^2.
            Inertia = 2.0f / 5.0f * MassKG * _radius * _radius;

            //Create the actuators
            Actuators = new PneumaticActuator[6 /*Linear actuators*/ + 6 /*Rotational actuators*/];

            float linearActuatorForce = 120;    
            float rotationalActuatorForce = 30.0f;
            //Linear actuators
            Actuators[0] = new PneumaticActuator(this, -Vector3.UnitX, linearActuatorForce * Vector3.UnitX, Vector3.Zero, 5);
            Actuators[1] = new PneumaticActuator(this, Vector3.UnitX, -linearActuatorForce * Vector3.UnitX, Vector3.Zero, 5);
            Actuators[2] = new PneumaticActuator(this, -Vector3.UnitY, linearActuatorForce * Vector3.UnitY, Vector3.Zero, 5);
            Actuators[3] = new PneumaticActuator(this, Vector3.UnitY, -linearActuatorForce * Vector3.UnitY, Vector3.Zero, 5);
            Actuators[4] = new PneumaticActuator(this, -Vector3.UnitZ, linearActuatorForce * Vector3.UnitZ, Vector3.Zero, 5);
            Actuators[5] = new PneumaticActuator(this, Vector3.UnitZ, -linearActuatorForce * Vector3.UnitZ, Vector3.Zero, 5);
            //Rotational actuators
            Actuators[6] = new PneumaticActuator(this, Vector3.UnitX, Vector3.Zero, rotationalActuatorForce * Vector3.UnitZ, 5);
            Actuators[7] = new PneumaticActuator(this, -Vector3.UnitX, Vector3.Zero, rotationalActuatorForce * Vector3.UnitZ, 5);
            Actuators[8] = new PneumaticActuator(this, Vector3.UnitY, Vector3.Zero, rotationalActuatorForce * Vector3.UnitX, 5);
            Actuators[9] = new PneumaticActuator(this, -Vector3.UnitY, Vector3.Zero, rotationalActuatorForce * Vector3.UnitX, 5);
            Actuators[10] = new PneumaticActuator(this, Vector3.UnitZ, Vector3.Zero, rotationalActuatorForce * Vector3.UnitY, 5);
            Actuators[11] = new PneumaticActuator(this, -Vector3.UnitZ, Vector3.Zero, rotationalActuatorForce * Vector3.UnitY, 5);

            //Testing! Set them to constant actuators
            for (int i = 0; i < Actuators.Length; i++)
                Actuators[i].IsPulseActuator = true;
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
            WorldContainer.soundEffectsMgr.PlaySoundEffectInZAsteroids(WorldContent.sfxContent.IncendiaryExplosionInSpace, this.Location);

            //Shut down all actuators and stop drawing it.
            this.IsDrawing = false;
            for (int i = 0; i < Actuators.Length; i++)
                Actuators[i].DutyCycleNorm = 0;
        }

        /// <summary>
        /// Update the physics.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            //Update the current rotation.
            //UPDATE - I'm an idiot, I did this with UAVs for years and i got it wrong here, uber-fail [GearsAD]
            this.ObjectRotation *= Quaternion.CreateFromYawPitchRoll(
                (float)(this.VelocityRotat.Y * gameTime.ElapsedGameTime.TotalSeconds), 
                (float)(this.VelocityRotat.X * gameTime.ElapsedGameTime.TotalSeconds), 
                (float)(this.VelocityRotat.Z * gameTime.ElapsedGameTime.TotalSeconds));
            this.Velocity += AccelerationLinear * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.VelocityRotat += AccelerationRotat * (float)gameTime.ElapsedGameTime.TotalSeconds;

            AccelerationLinear = Vector3.Zero;
            AccelerationRotat = Vector3.Zero;

            //Update the actuators.
            for (int i = 0; i < Actuators.Length; i++)
            {
                Actuators[i].Update(gameTime);
                AccelerationLinear += Vector3.Transform( 
                    Actuators[i].LateralForceN_Current / MassKG,
                    this.ObjectRotation);
                AccelerationRotat += Actuators[i].RotationalForceN_Current / MassKG; 
            }

            //Provide damping for a simpler flight model *sigh :p* [GearsAD]
            if (IsUsingDamping)
            {
                Vector3 velDamping = this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (velDamping.LengthSquared() < this.Velocity.LengthSquared()) //Handling the case where a huge loss in framerate would cause it to move backwards.
                    this.Velocity -= velDamping;
                else
                    this.Velocity = Vector3.Zero;
                velDamping = this.VelocityRotat * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (velDamping.LengthSquared() < this.VelocityRotat.LengthSquared())
                    this.VelocityRotat -= velDamping;
                else
                    this.VelocityRotat = Vector3.Zero;
            }

            //Check the keyboard input
            if (IsControlledByKeyboard && Life > 0 && MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (gamePadState.IsConnected)
                    CheckGamepadInput(gamePadState);
                else
                    CheckKeyboardInput();
            }
        }

        private void CheckKeyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            //Check the keyboard.
            Actuators[0].DutyCycleNorm = (keyState.IsKeyDown(Keys.A) ? 1.0f : 0.0f);
            Actuators[1].DutyCycleNorm = (keyState.IsKeyDown(Keys.D) ? 1.0f : 0.0f);
            Actuators[2].DutyCycleNorm = (keyState.IsKeyDown(Keys.Q) ? 1.0f : 0.0f);
            Actuators[3].DutyCycleNorm = (keyState.IsKeyDown(Keys.E) ? 1.0f : 0.0f);
            Actuators[4].DutyCycleNorm = (keyState.IsKeyDown(Keys.W) ? 1.0f : 0.0f);
            Actuators[5].DutyCycleNorm = (keyState.IsKeyDown(Keys.S) ? 1.0f : 0.0f);
            //Rotational actuators - these are paired.
            Actuators[6].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad4) ? 1.0f : 0.0f);
            Actuators[7].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad6) ? 1.0f : 0.0f);
            Actuators[8].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad9) ? 1.0f : 0.0f);
            Actuators[9].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad7) ? 1.0f : 0.0f);
            Actuators[10].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad8) ? 1.0f : 0.0f);
            Actuators[11].DutyCycleNorm = (keyState.IsKeyDown(Keys.NumPad5) ? 1.0f : 0.0f);

            if (_lastKeyState.IsKeyDown(Keys.Tab) && keyState.IsKeyUp(Keys.Tab))
            {
                IsUsingDamping = !IsUsingDamping;
            }
            _lastKeyState = keyState;
        }

        private void CheckGamepadInput(GamePadState gamepadState)
        {
            //Check the gamepad.
            Actuators[0].DutyCycleNorm = (gamepadState.ThumbSticks.Left.X < 0 ? -gamepadState.ThumbSticks.Left.X : 0.0f);
            Actuators[1].DutyCycleNorm = (gamepadState.ThumbSticks.Left.X > 0 ? gamepadState.ThumbSticks.Left.X : 0.0f);
            Actuators[2].DutyCycleNorm = (gamepadState.Buttons.RightShoulder == ButtonState.Pressed ? 1.0f : 0.0f);
            Actuators[3].DutyCycleNorm = (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed ? 1.0f : 0.0f);
            Actuators[4].DutyCycleNorm = (gamepadState.ThumbSticks.Left.Y > 0 ? gamepadState.ThumbSticks.Left.Y : 0.0f);
            Actuators[5].DutyCycleNorm = (gamepadState.ThumbSticks.Left.Y < 0 ? -gamepadState.ThumbSticks.Left.Y : 0.0f);
            //Rotational actuators - these are paired.
            Actuators[6].DutyCycleNorm = (gamepadState.ThumbSticks.Right.X < 0 ? -gamepadState.ThumbSticks.Right.X : 0.0f);
            Actuators[7].DutyCycleNorm = (gamepadState.ThumbSticks.Right.X > 0 ? gamepadState.ThumbSticks.Right.X : 0.0f);
            Actuators[8].DutyCycleNorm = (gamepadState.Triggers.Right > 0 ? gamepadState.Triggers.Right : 0.0f);
            Actuators[9].DutyCycleNorm = (gamepadState.Triggers.Left > 0 ? gamepadState.Triggers.Left : 0.0f);
            Actuators[10].DutyCycleNorm = (gamepadState.ThumbSticks.Right.Y < 0 ? -gamepadState.ThumbSticks.Right.Y : 0.0f);
            Actuators[11].DutyCycleNorm = (gamepadState.ThumbSticks.Right.Y > 0 ? gamepadState.ThumbSticks.Right.Y : 0.0f);

            if (_lastGamePadState.Buttons.Back == ButtonState.Pressed && _lastGamePadState.Buttons.Back == ButtonState.Released)
            {
                IsUsingDamping = !IsUsingDamping;
            }
            _lastGamePadState = gamepadState;
        }
    }
}
