using System;
using System.Collections.Generic;

namespace PoolingSystem
{
    [Serializable]
    public class PoolManager<T> where T : class
    {
        private readonly Queue<T> _pool;
        private readonly Func<T> _createAction;

        public int MinPoolCount { get; private set; }
        public int MaxPoolCount { get; private set; }
        public int Count => _pool.Count;

        public PoolManager(Func<T> createAction, int minPoolCount = 10, int maxPoolCount = 50)
        {
            _createAction = createAction ?? throw new ArgumentNullException(nameof(createAction));
            MinPoolCount = minPoolCount;
            MaxPoolCount = maxPoolCount;

            _pool = new Queue<T>();

            for (var i = 0; i < minPoolCount; i++)
            {
                var item = _createAction();
                _pool.Enqueue(item);
            }
        }
        public T GetFromPool()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : _createAction();
        }
        
        public void AddToPool(T item)
        {
            _pool.Enqueue(item);
        }
    }
}