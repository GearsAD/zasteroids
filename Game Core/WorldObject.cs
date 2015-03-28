using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;

namespace ZitaAsteria.World
{
    /// <summary>
    /// The parent class for all items in our world. 
    /// </summary>
    public abstract class WorldObject
    {
        /// <summary>
        /// The type of the WorldObject
        /// </summary>
        //[Browsable(false)]
        public WorldObjectType worldObjectType { get; set; }

        /// <summary>
        /// The WorldObject's Location.
        /// </summary>
        private Vector2 location;

        /// <summary>
        /// The location of the object.
        /// </summary>
        //[Browsable(true)]
        public virtual Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        //[Browsable(false)]
        public float xLocation
        {
            get { return location.X; }
            set { location.X = value; }
        }

        //[Browsable(false)]
        public float yLocation
        {
            get { return location.Y; }
            set { location.Y = value; }
        }

        /// <summary>
        /// The heading - where 0 is true north.
        /// </summary>
        //[Browsable(false)]
        public double heading { get; set; }

        //[Browsable(true)]
        public double Heading
        {
            get { return heading; }
            set { heading = value; }
        }

        //[Browsable(true)]
        public double HeadingDegrees
        {
            get { return heading * 180.0 / 3.14159265358979323; }
            set { heading = value / 180.0 * 3.14159265358979323; }
        }

        [ContentSerializerIgnore]
        Vector2 forward;

        [ContentSerializerIgnore]
        Vector2 back;

        [ContentSerializerIgnore]
        Vector2 left;

        [ContentSerializerIgnore]
        Vector2 right;

        /// <summary>
        /// The WorldObject's origin (drawing centre). This is from the top left corner...
        /// </summary>
        //[Browsable(false)]
        [ContentSerializerIgnore]
        public Vector2 origin;
        /// <summary>
        /// The bounding box for the WorldObject in world coordinates! Note that this is relative to origin because that's where drawing the sprite!
        /// </summary>
        [ContentSerializerIgnore]
        //[Browsable(false)]
        public RectangleF boundingBox;

        //[Browsable(false)]
        [ContentSerializerIgnore]
        public Vector3[] BoundingBoxVectors { get; set; }

        /// The actual size of the WorldObject.
        /// </summary>
        //[NonSerialized]
        //[Browsable(false)]
        public Vector2 worldScale = Vector2.One;

        /// <summary>
        /// <summary>
        /// If true then the bounding box isn't rotated for checking.
        /// </summary>
        protected bool IsBoundingBoxRotationInvariant = false;

        #region Cached Fields
        Matrix
                _scaleMatrix, 
                _yRotationMatrix, 
                _LocationOffsetMatrix, 
                _worldMatrix;
        #endregion


        /// <summary>
        /// Allocated here to save new calls during game updates.
        /// </summary>
        private Vector3
            _bottomRight,
            _bottomLeft,
            _topRight,
            _topLeft;

        /// <summary>
        /// Allocated here to save new calls during game updates.
        /// </summary>
        private Matrix _transformMatrix;

        public WorldObject()
        {
            Location = Vector2.Zero;
            origin = Vector2.Zero;
            boundingBox = new RectangleF();
            heading = 0;

            //Must set this!
            this.origin = new Vector2(0, 0);

            //Calculate the bounding box so that it exists for the first Update() call.
            BoundingBoxVectors = new Vector3[4];
            for (int i = 0; i < 4; i++) BoundingBoxVectors[i] = Vector3.Zero;
        }

        /// <summary>
        /// Our abstract Initialize method. Initialize the general stuff here...
        /// </summary>
        public virtual void Initialize()
        {
            GetBoundingBox(BoundingBoxVectors);
        }

        /// <summary>
        /// The WorldObject Update method, where the bounding box is calculated.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Force an update of the bounding box.
        /// </summary>
        protected void ForceBoundingBoxUpdate()
        {
            //Just calculate the bounding box
            GetBoundingBox(BoundingBoxVectors);
        }

        /// <summary>
        /// Our abstract Draw method - FOR 3D.
        /// </summary>
        public virtual void Draw3D() 
        {
        }

        /// <summary>
        /// Our abstract Draw method - for 2D objects and billboards. The Z-buffer is cleared before this is run.
        /// </summary>
        public virtual void Draw2D()
        {
        }

        /// <summary>
        /// Gets the forward vector for this WorldObject.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetForwardVector()
        {
            forward.X = -(float)Math.Sin(heading);
            forward.Y = (float)Math.Cos(heading);
            return forward;
        }

        /// <summary>
        /// Gets the back vector for this WorldObject.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBackVector()
        {
            back.X = -(float)Math.Sin(heading);
            back.Y = (float)Math.Cos(heading);
            back = -back;
            return back;
        }

        /// <summary>
        /// Gets the left vector for this WorldObject.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetLeftVector()
        {
            left.X = -(float)Math.Sin(heading - Math.PI / 2.0);
            left.Y = (float)Math.Cos(heading - Math.PI / 2.0);
            return left;
        }

        /// <summary>
        /// Gets the right vector for this WorldObject.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRightVector()
        {
            right.X = -(float)Math.Sin(heading - Math.PI / 2.0);
            right.Y = (float)Math.Cos(heading - Math.PI / 2.0);
            right = -right;
            return right;
        }

        /// <summary>
        /// Calculates the bounding box vector transform matrix for WorldObject - this is used in GetBoundingBox.
        /// Order of operations -
        /// 1.) Scale to worldScale.
        /// 2.) Rotate to correct Y rotation.
        /// 3.) Translate to correct world Location.
        /// Note - THIS IS OVERRIDDEN IN THE KinematicNPCChild method.
        /// </summary>
        /// <param name="imagePixelWidth"></param>
        /// <param name="imagePixelHeight"></param>
        /// <returns></returns>
        protected virtual Matrix CalculateBoundingTransformMatrix()
        {
            //Create all the matrices for the calculation of the world matrix.
            if (!IsBoundingBoxRotationInvariant)
            {
                _yRotationMatrix = Matrix.CreateRotationY(-(float)heading);
                _LocationOffsetMatrix = Matrix.CreateTranslation(Location.X, 0, Location.Y);

                //Calculate the world matrix.
                _worldMatrix = _yRotationMatrix * _LocationOffsetMatrix;
                return _worldMatrix;
            }
            else
            {
                _LocationOffsetMatrix = Matrix.CreateTranslation(Location.X, 0, Location.Y);

                //Calculate the world matrix.
                _worldMatrix = _LocationOffsetMatrix;
                return _worldMatrix;
            }
        }

        /// <summary>
        /// Converts the general bounding box to a real world bounding box and returns it.
        /// This is in the form of top left, top right, bottom left, bottom right.
        /// </summary>
        /// <param name="imagePixelHeight">The height of the currently drawn image - this is used to rescale the pixel-based bounding box.</param>
        /// <param name="imagePixelWidth">The width of the currently drawn image - this is used to rescale the pixel-based bounding box.</param>
        /// <returns></returns>
        private void GetBoundingBox(Vector3[] bounds)
        {
            //Define the pixel-based bounding vectors - use the state vectors.
            _bottomRight.X = this.boundingBox.X; _bottomRight.Y = 0;_bottomRight.Z = this.boundingBox.Y;
            _bottomLeft.X = this.boundingBox.X + this.boundingBox.Width; _bottomLeft.Y = 0; _bottomLeft.Z = this.boundingBox.Y;
            _topRight.X = this.boundingBox.X; _topRight.Y = 0; _topRight.Z = this.boundingBox.Y + this.boundingBox.Height;
            _topLeft.X = this.boundingBox.X + this.boundingBox.Width; _topLeft.Y = 0; _topLeft.Z = this.boundingBox.Y + this.boundingBox.Height;

            //Now the vectors should be in world coordinates, we now need to put them in the right place because they're currently at the origin.
            _transformMatrix = CalculateBoundingTransformMatrix();

            Vector3.Transform(ref _topLeft, ref _transformMatrix, out _topLeft);
            Vector3.Transform(ref _topRight, ref _transformMatrix, out _topRight);
            Vector3.Transform(ref _bottomLeft, ref _transformMatrix, out _bottomLeft);
            Vector3.Transform(ref _bottomRight, ref _transformMatrix, out _bottomRight);

            bounds[0] = _topLeft;
            bounds[1] = _topRight;
            bounds[2] = _bottomLeft;
            bounds[3] = _bottomRight;
        }

    }
}
