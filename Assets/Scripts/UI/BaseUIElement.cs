using Gameplay.Interfaces;
using UnityEngine;

namespace UI
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement, IPoolable
    {
        private bool _isInitialized;
        public bool IsVisible => gameObject.activeInHierarchy;
        public string ElementName => GetType().Name;

        protected virtual void Awake()
        {
            
        }

        public virtual void Initialize()
        {
            _isInitialized = true;
            OnInitialize();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        public virtual void OnDestroy()
        {
            OnCleanup();
            _isInitialized = false;
        }

        public virtual void OnCalledFromPool()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnReturnToPool()
        {
            Hide();
            gameObject.SetActive(false);
        }

        protected virtual void OnInitialize() { }
        protected abstract void OnShow();
        protected abstract void OnHide();
        protected virtual void OnCleanup() { }

    }
}
