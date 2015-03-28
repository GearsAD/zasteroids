using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZitaAsteria
{
    /// <summary>
    /// General rectangle structure, why don't they have a float version of this?!
    /// Initializes to a 1x1 with an origin in the centre.
    /// </summary>
    public struct RectangleF
    {
        public float X, Y, Width, Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static RectangleF UnitRectangle = new RectangleF(-0.5f, -0.5f, 1, 1);
    }
}
