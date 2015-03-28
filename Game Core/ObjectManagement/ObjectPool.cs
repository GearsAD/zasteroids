using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using ZitaAsteria.World.Effects;
using ZitaAsteria.World;

namespace ZitaAsteria
{
    public class ObjectPool<T> : IObjectPool where T:class, new()
    {
        private ConstructorInfo _objectConstructor;
        private Queue<T> _objectPool;

        public int InitialSize { get; set; }
        public int BatchIncreaseSize { get; set; }
        public bool BatchIncreaseSizeIsPercentage { get; set; }

        public int CurrentSize
        {
            get { return _objectPool.Count; }
        }

        public ObjectPool()
        {
            Type objectType = typeof(T);

            _objectConstructor = objectType.GetConstructor(System.Type.EmptyTypes);
            _objectPool = new Queue<T>();

            InitialSize = 100;
            BatchIncreaseSize = 100;
            BatchIncreaseSizeIsPercentage = true;
        }

        public ObjectPool(int initialSize)
            : this()
        {
            this.InitialSize = initialSize;
        }

        public ObjectPool(int initialSize, int batchIncreaseSize)
            : this()
        {
            this.InitialSize = initialSize;
            this.BatchIncreaseSize = batchIncreaseSize;
        }

        public ObjectPool(int initialSize, int batchIncreaseSize, bool batchIncreaseSizeIsPercentage)
            : this()
        {
            this.InitialSize = initialSize;
            this.BatchIncreaseSize = batchIncreaseSize;
            this.BatchIncreaseSizeIsPercentage = batchIncreaseSizeIsPercentage;
        }

        private void IncreasePoolSize(int value)
        {
            for (int i = 0; i < value; i++)
            {
                try
                {
                    T obj = _objectConstructor.Invoke(null) as T;

                    //Special case - need to initialize all ACEs so that they contain their effects.
                    if (typeof(T).IsSubclassOf(typeof(AbstractCompoundEffect))) //Then we need to initialize it.
                        (obj as AbstractCompoundEffect).Initialize();

                    _objectPool.Enqueue(obj);

                }
                catch (Exception ex)
                {
                    //Need to know when this happens, want it to fail here, and explicitly! [Gears]
                    throw new Exception("Cannot expand object pool - " + ex.Message, ex);
                }
            }
                    //Need to know this stuff! Annnnnndddd now I'm removing it lol....
#if WINDOWS
                    //PhysicsWorldObject.FireObjectMessage(new PhysicsWorldObjectMessageEventArgs(null, "Object pool for '" + typeof(T).Name + "' has been increased with another " + value + " elements, new size " + _objectPool.Count + ")."));
#endif
        }

        public virtual void Initialize()
        {
            IncreasePoolSize(InitialSize);
        }

        public virtual T GetObject()
        {
            if (_objectPool.Count == 0)
            {
                //Debug.WriteLine("Increasing object pool...");
                int valueToIncreaseBy = BatchIncreaseSize;

                if (BatchIncreaseSizeIsPercentage)
                    valueToIncreaseBy = (int)((double)InitialSize * ((double)BatchIncreaseSize / 100));

                IncreasePoolSize(valueToIncreaseBy);
            }

            return _objectPool.Dequeue();
        }

        public virtual T GetObject<T>() where T:class
        {
            return GetObject() as T;
        }

        public virtual void ReleaseObject(T obj)
        {
            _objectPool.Enqueue(obj);
        }

        public virtual void ReleaseObject(object obj)
        {
            _objectPool.Enqueue((T)obj);
        }
    }
}
