using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.World.ShaderEffects
{
    /// <summary>
    /// A single light.
    /// </summary>
    public class Light
    {
        /// <summary>
        /// The Location of the light. There is also the option of using the WorldObject/PhysicsWorldObject class here...
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// This is the Red, Green, and Blue values of the light source. We're not going to use an intensity because it'll make the 
        /// calculations more complex. Scale these values between 0 and 1 for both the colour and intensity.
        /// </summary>
        public Vector3 lightColour { get; set; }

        /// <summary>
        /// Create a new light source at a specified Location with a specific color/intensity.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="light_colour"></param>
        public Light(Vector2 Location, Vector3 light_colour)
        {
            this.Location = Location;
            this.lightColour = light_colour;
        }
    }
}
