using System;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Objects.Entities.Entity_Components
{
    public abstract class BaseEntityComponent : MonoBehaviour, IEntityComponent
    {
        protected IEntity _owner;
        
        public IEntity Owner => _owner;
        public bool IsEnabled => gameObject.activeInHierarchy;

        public virtual void Initialize(IEntity owner)
        {
            _owner = owner;
        }
        
        public virtual void Enable()
        {
            enabled = true;
        }
        
        public virtual void Disable()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }
    }
}