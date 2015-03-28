using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZitaAsteria.Common.Filters
{
    /// <summary>
    /// A common first order filter.
    /// </summary>
    public class FirstOrderFilter
    {
        #region Public Properties
        /// <summary>
        /// The convergence factor normalized by time - generally around 0.5 - 1.2. 4x this value is the time it will take to converge.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// The current value.
        /// </summary>
        public float CurrentValue { get; set; }

        /// <summary>
        /// The target value.
        /// </summary>
        public float TargetValue { get; set; }
        #endregion

        public FirstOrderFilter()
        {
            Alpha = 0.9f;
            CurrentValue = 0;
            TargetValue = 0;
        }

        public FirstOrderFilter(float alpha, float currentValue, float targetValue)
        {
            Alpha = alpha;
            CurrentValue = currentValue;
            TargetValue = targetValue;
        }

        /// <summary>
        /// Update the filter.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            CurrentValue += Alpha * (float)(gameTime.ElapsedGameTime.TotalSeconds * (TargetValue - CurrentValue));
        }
    }
}
