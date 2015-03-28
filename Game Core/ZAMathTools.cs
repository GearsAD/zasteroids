using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria
{
    /// <summary>
    /// Quick class for general-purpose math functions.
    /// </summary>
    public static class ZAMathTools
    {
        /// <summary>
        /// The general purpose uniform random number generator.
        /// </summary>
        public static Random uniformRandomGenerator = new Random(System.Environment.TickCount);

        /// <summary>
        /// Generate a random number from a gaussian distribution. Based upon code from http://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stdDev"></param>
        /// <returns></returns>
        public static double GetGaussianRandom(double mean, double stdDev)
        {
            double u1 = uniformRandomGenerator.NextDouble(); //these are uniform(0,1) random doubles 
            double u2 = uniformRandomGenerator.NextDouble(); 
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1) 
            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            if (Double.IsNaN(randNormal)) //WTF - seems like it can happen
                return 0;
            return randNormal;
        }

    }
}
