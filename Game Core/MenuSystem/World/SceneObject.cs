using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZitaAsteria.MenuSystem.World.Collision_Objects;

namespace ZitaAsteria.MenuSystem.World
{
    /// <summary>
    /// An abstract scene object, contains a 3D position and some update methods.
    /// </summary>
    public abstract class SceneObject
    {
        #region Public Properties
        public Vector3 Scale { get; set; }
        public Vector3 Location { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 SceneRotation { get; set; }
        /// <summary>
        /// The life of the object.
        /// </summary>
        public float Life { get; set; }
        /// <summary>
        /// Inner rotation of the object before it is translated into a scene RSL (rot/scal/loc)
        /// </summary>
        public Quaternion ObjectRotation { get; set; }
        public Model Model { get; set; }
        /// <summary>
        /// If true (default), then this model uses lighting.
        /// </summary>
        public bool IsUsingLighting { get; set; }

        /// <summary>
        /// If true then it is drawn.
        /// </summary>
        public bool IsDrawing { get; set; }

        /// <summary>
        /// The alpha content.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// If true then doesn't cast onto the depth buffer, e.g. shields.
        /// </summary>
        public bool IsZTransparent { get; set; }

        /// <summary>
        /// If true then it will use additive blending.
        /// </summary>
        public bool IsAdditiveBlending { get; set; }

        /// <summary>
        /// If true then the collision boxes are checked.
        /// </summary>
        public bool IsCollidable { get; set; }
        #endregion

        #region Cached Fields
        Matrix _proj, _view, _world;
        #endregion

        #region Private Fields
        /// <summary>
        /// The bounding sphere radius.
        /// </summary>
        protected BoundingSphere _cdBoundingSphere = new BoundingSphere(Vector3.Zero, 0);
        /// <summary>
        /// The other bounding objects, generally null except for the satellite.
        /// </summary>
        protected List<CollisionContainer> _cdCollisionContainers = new List<CollisionContainer>();
        /// <summary>
        /// True if the CD is initialized.
        /// </summary>
        protected bool _cdIsInitialized = false;
        #endregion

        public SceneObject()
        {
            IsUsingLighting = true;
            Scale = Vector3.One;
            Life = 100;
            IsDrawing = true;
            Alpha = 1;
            IsCollidable = true;
        }

        /// <summary>
        /// Initialize the scene object
        /// </summary>
        public virtual void Initialize()
        {
            Location = Vector3.Zero;
            SceneRotation = Vector3.Zero;
            ObjectRotation = Quaternion.Identity;
            Model = null;
        }

        /// <summary>
        /// Updates the location with the velocity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //Update the bounding sphere radius, would rather do it here than in a Post-Initialize method because it's always forgotten there.
            if (!_cdIsInitialized && Model != null) //To initialize...
            {
                //Calculate the bounding sphere.
                if (Model.Meshes.Count > 0)
                    _cdBoundingSphere = Model.Meshes[0].BoundingSphere;
                float max = (float)(Math.Max(Math.Max(Scale.X, Scale.Y), Scale.Z));
                _cdBoundingSphere.Radius *= max;
                _cdIsInitialized = true;
            }
            _cdBoundingSphere.Center = this.Location;

            //Update the bounding containers.
            //If we get any fancier with this we might have to make this better.
            foreach (CollisionContainer selfContainer in this._cdCollisionContainers)
            {
                selfContainer.Location = this.Location;
            }

            //Update the location
            Location += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Do some effect with this object at a specific location.
        /// </summary>
        public virtual void DieEffect(Vector3 locationOfCollision)
        {
            //Do nothing.
        }

        /// <summary>
        /// Returns true if the two collide.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsCollidingWith(SceneObject other)
        {
            //Update the bounding boxes.
            //other._cdBoundingSphere.Center = other.Location;
            //this._cdBoundingSphere.Center = this.Location;
            //If the spheres are colliding.
            if (this._cdBoundingSphere.Intersects(other._cdBoundingSphere))
            {
                if (this._cdCollisionContainers.Count == 0 && other._cdCollisionContainers.Count == 0) //Neither have bounding boxes.
                    return true;
                //Otherwise check their bounding objects.
                //Then check the bounding containers - a little inefficient but come on, this is asteroids, how many bounding containers are we really going to use?!
                //...
                //...
                //...Dear gott, have to be careful when i say that... [GearsAD]

                foreach (CollisionContainer selfContainer in this._cdCollisionContainers)
                {
                    foreach (CollisionContainer otherContainer in other._cdCollisionContainers)
                    {
                        if (selfContainer.IsCollidingWith(otherContainer))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the two collide.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float? IsCollidingWith(Ray other)
        {
            this._cdBoundingSphere.Center = this.Location;
            //If the spheres are colliding.
            if (this._cdBoundingSphere.Intersects(other) != null)
            {
                //The only thing that casts a ray is the laser so ignore any complex CD for it.
                return this._cdBoundingSphere.Intersects(other);
            }

            return null;
        }

        public bool IsCollidingWith(BoundingSphere otherSphere)
        {
            this._cdBoundingSphere.Center = this.Location;
            //If the spheres are colliding.
            if (this._cdBoundingSphere.Intersects(otherSphere))
            {
                return true;
            }

            return false;

        }

        public virtual void DrawInBlack()
        {
            Draw(true);
        }

        public virtual void Draw()
        {
            Draw(false);
        }

        /// <summary>
        /// Draws the model at its location.
        /// </summary>
        public virtual void Draw(bool inBlack)
        {
            if (!IsDrawing) return;

            if (Model == null)
                throw new Exception("Model is null during draw call!");

            _proj = MenuContainer.Camera.GetProjectionMatrix(WorldContainer.graphicsDevice.Viewport);
            _view = MenuContainer.Camera.GetViewMatrix();
            _world =
                Matrix.CreateScale(Scale) *
                Matrix.CreateRotationX(SceneRotation.X) * Matrix.CreateRotationY(SceneRotation.Y) * Matrix.CreateRotationZ(SceneRotation.Z) *
                Matrix.CreateFromQuaternion(ObjectRotation) *
                Matrix.CreateTranslation(Location);

            BlendState prev = MenuContainer.GraphicsDevice.BlendState;

            if (IsAdditiveBlending)
            {
                if (inBlack) return;
                MenuContainer.GraphicsDevice.BlendState = BlendState.Additive;
            }
            if (IsZTransparent)
                MenuContainer.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            foreach (ModelMesh modmesh in Model.Meshes)
            {
                foreach (BasicEffect effect in modmesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.Alpha = Alpha;
                    if (inBlack)
                    {
                        effect.DirectionalLight0.DiffuseColor = Color.Black.ToVector3();
                        effect.DirectionalLight0.Direction = Vector3.UnitX;
                        effect.DirectionalLight1.Enabled = false;
                        effect.DirectionalLight2.Enabled = false;
                        effect.AmbientLightColor = Color.Black.ToVector3();
                    }
                    else
                    {
                        effect.LightingEnabled = IsUsingLighting;
                        if (IsUsingLighting)
                        {
                            //All light from the sun to the planet at 80000 UnitX. So use the unit X vector.
                            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3() * 0.8f; //Make it over-bright - start sunlight.
                            effect.DirectionalLight0.Direction = Vector3.UnitX;
                            effect.DirectionalLight0.Enabled = true;
                            effect.DirectionalLight1.DiffuseColor = Color.LightYellow.ToVector3() * 0.3f;
                            effect.DirectionalLight1.Direction = -Vector3.UnitX;
                            effect.DirectionalLight1.Enabled = true;
                            effect.DirectionalLight2.DiffuseColor = Color.Yellow.ToVector3() * 0.1f;
                            effect.DirectionalLight2.Direction = Vector3.UnitZ;
                            effect.DirectionalLight2.Enabled = true;
                            //effect.AmbientLightColor = Color.Yellow.ToVector3() * 0.2f;
                        }
                    }
                    effect.World = _world;
                    effect.View = _view;
                    effect.Projection = _proj;
                }
                modmesh.Draw();
            }

            //Set back.
            MenuContainer.GraphicsDevice.BlendState = prev;
            MenuContainer.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        
    }
}
