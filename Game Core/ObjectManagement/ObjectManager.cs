using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.ExplosionsSmoke;
using ZitaAsteria.World.ShaderEffects;
using ZitaAsteria.MenuSystem.World;
using ZAsteroids.World.Effects.ExplosionSmoke;

namespace ZitaAsteria
{
    /// <summary>
    /// A static class for managing object pools
    /// </summary>
    public static class ObjectManager
    {
        /// <summary>
        /// A dictionary of IObjectPool's. Key for dictionary is Type.FullName
        /// </summary>
        public static Dictionary<Type, IObjectPool> ObjectPools { get; set; }

        static ObjectManager()
        {
            ObjectPools = new Dictionary<Type, IObjectPool>();
        }

        /// <summary>
        /// Only initialize a subset of the stuff.
        /// </summary>
        public static void Initialize_ForZAsteroids()
        {
            AddObjectPool<Rock>(100, 20, true);
            AddObjectPool<IncendiaryExplosionACE>(30, 5, false);
        }

        public static void AddObjectPool<T>() where T : class, new()
        {
            IObjectPool objectPool = new ObjectPool<T>();
            AddObjectPool(typeof(T), objectPool);
        }

        public static void AddObjectPool<T>(int initialSize) where T : class, new()
        {
            IObjectPool objectPool = new ObjectPool<T>(initialSize);
            AddObjectPool(typeof(T), objectPool);
        }

        public static void AddObjectPool<T>(int initialSize, int batchIncreaseSize) where T : class, new()
        {
            IObjectPool objectPool = new ObjectPool<T>(initialSize, batchIncreaseSize);
            AddObjectPool(typeof(T), objectPool);
        }

        public static void AddObjectPool<T>(int initialSize, int batchIncreaseSize, bool batchIncreaseSizeIsPercentage) where T : class, new()
        {
            IObjectPool objectPool = new ObjectPool<T>(initialSize, batchIncreaseSize, batchIncreaseSizeIsPercentage);
            AddObjectPool(typeof(T), objectPool);
        }

        private static void AddObjectPool(Type type, IObjectPool objectPool)
        {
            objectPool.Initialize();
            if (!ObjectPools.ContainsKey(type))
                ObjectPools.Add(type, objectPool);
        }

        /// <summary>
        /// Get an available object from the object pool of the specified type
        /// </summary>
        /// <typeparam name="T">The type of object to get</typeparam>
        /// <returns>An available object from the object pool</returns>
        public static T GetObject<T>() where T : class, new()
        {

            IObjectPool objectPool = null;

            if (ObjectPools.ContainsKey(typeof(T)))
            {
                objectPool = ObjectPools[typeof(T)];
                return objectPool.GetObject<T>();
            }
            else
            {
                //We need to be strict with object pool management...
                throw new InvalidOperationException("You are requesting an object from the ObjectManager, yet there is no pool set up for it! Type = " + typeof(T).ToString());
                //                return new T();
            }
        }

        public static void ReleaseObject(object obj)
        {
            Type objectType = obj.GetType();
            if (ObjectPools.ContainsKey(objectType))
                ObjectPools[objectType].ReleaseObject(obj);
        }
    }
}
