using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem.World;

namespace ZAsteroids.World
{
    /// <summary>
    /// Small ray intersection container class.
    /// </summary>
    public class RayIntersectionInfo
    {
        #region Public Fields
        public SceneObject Object { get; private set; }
        public Ray Ray { get; private set; }
        public float Distance { get; private set; }
        public List<SceneObject> ListToRemoveFrom { get; private set; }
        #endregion
        public RayIntersectionInfo(SceneObject obj, Ray ray, float distance, List<SceneObject> listToRemoveFrom)
        {
            Object = obj;
            Ray = ray;
            Distance = distance;
            ListToRemoveFrom = listToRemoveFrom;
        }
    }

}
