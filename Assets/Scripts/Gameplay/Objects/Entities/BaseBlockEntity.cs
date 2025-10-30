using System;
using System.Collections.Generic;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities.Entity_Components;
using UnityEngine;

namespace Gameplay.Objects.Entities
{
    public abstract class BaseBlockEntity : MonoBehaviour, IBlockEntity, IPoolable
    {
        [SerializeField]
        protected BaseEntityComponent[] _entityComponentList;

        private Dictionary<Type, IEntityComponent> _componentCachingMap;
        public Transform WorldTransform => transform;
        public Vector2Int BoardIndex { get; private set; }
        
        public void SetBoardIndex(Vector2Int blockPosition)
        {
            BoardIndex = blockPosition;
        }
        
        public void SetWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public virtual void Initialize()
        {
            _componentCachingMap = new Dictionary<Type, IEntityComponent>();
        }

        public virtual void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        public void OnCalledFromPool()
        {
            transform.localScale = Vector3.one;
        }

        public void OnReturnToPool()
        {
            OnDeactivate();
        }
        
        public T RequestEntityComponent<T>() where T : class, IEntityComponent
        {
            Type requestedType = typeof(T);

            if (_componentCachingMap.TryGetValue(requestedType, out IEntityComponent cachedEntityComponent))
            {
                return cachedEntityComponent as T;
            }

            foreach (BaseEntityComponent component in _entityComponentList)
            {
                if (component is not T requestedComponent)
                    continue;

                _componentCachingMap[requestedType] = requestedComponent;
                return requestedComponent;
            }

            return null;
        }
    }
}