using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZAsteroids.World.Upgrades
{
    /// <summary>
    /// Upgrade data like position, etc.
    /// </summary>
    public class UpgradeInfo
    {
        #region Public Properties
        /// <summary>
        /// The bounding sphere.
        /// </summary>
        public BoundingSphere BoundingSphere { get; set; }

        /// <summary>
        /// The upgrade type.
        /// </summary>
        public UpgradesEnum UpgradeType { get; private set; }

        /// <summary>
        /// The name of the upgrade
        /// </summary>
        public string UpgradeName { get; private set; }

        /// <summary>
        /// The cost of the upgrade
        /// </summary>
        public int Cost { get; private set; }

        /// <summary>
        /// If true then it is enabled and possible to use.
        /// </summary>
        public bool IsEnabled {get; set;}

        /// <summary>
        /// If true then it is in range.
        /// </summary>
        public bool IsInRange { get; set; }

        #endregion

        public UpgradeInfo(BoundingSphere sphere, UpgradesEnum upgrade, string name, int cost)
        {
            BoundingSphere = sphere;
            UpgradeType = upgrade;
            UpgradeName = name;
            Cost = cost;
        }
    }
}
