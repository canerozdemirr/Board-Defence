using System;
using Gameplay.Interfaces;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Utilities
{
    public class GameObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        protected readonly IObjectPool<T> _pool;
        protected readonly Transform _poolParent;
        protected readonly bool _dontDestroyOnLoad;
        
        protected GameObject _prefab;

        protected GameObjectPool(GameObject prefab, Transform poolParent = null, int defaultSize = 1, int maxSize = 100, bool dontDestroyOnLoad = false)
        {
            _prefab = prefab;
            _poolParent = poolParent;
            _dontDestroyOnLoad = dontDestroyOnLoad;
            _pool = new ObjectPool<T>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, defaultSize, maxSize);
        }

        #region Pool Events

        protected virtual void OnTakeFromPool(T takenObject)
        {
            takenObject.OnCalledFromPool();
        }

        protected virtual void OnReturnedToPool(T returnedObject)
        {
            returnedObject.OnReturnToPool();
            returnedObject.transform.parent = _poolParent;
        }

        protected virtual void OnDestroyPoolObject(T destroyedObject)
        {
            Object.Destroy(destroyedObject.gameObject);
        }

        #endregion

        public virtual T Spawn()
        {
            return _pool.Get();
        }

        public static GameObjectPool<T> Create(GameObject prefab, Transform parent, int defaultSize = 10, int maxSize = 100, bool dontDestroyOnLoad = false)
        {
            return new GameObjectPool<T>(prefab, parent, defaultSize, maxSize, dontDestroyOnLoad);
        }

        public virtual void DeSpawn(T targetObject)
        {
            _pool.Release(targetObject);
        }

        public virtual void ClearObjectReferences()
        {
            _pool.Clear();
        }

        protected virtual T CreatePoolObject()
        {
            GameObject pooledObject = Object.Instantiate(_prefab, _poolParent);
            pooledObject.name = $"{_prefab.name} (Pooled)";
            
            T component = pooledObject.GetComponent<T>();
            
            switch (_dontDestroyOnLoad)
            {
                case true when _poolParent != null:
                    Object.DontDestroyOnLoad(_poolParent.gameObject);
                    break;
                case true:
                    Object.DontDestroyOnLoad(pooledObject);
                    break;
            }

            return component;
        }
    }
}