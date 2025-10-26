using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Objects.Entities
{
    public abstract class BaseGridEntity : MonoBehaviour, IGridEntity, IPoolable
    {
        public Vector2Int BoardIndex { get; private set; }
        
        public void SetBoardIndex(Vector2Int gridPosition)
        {
            BoardIndex = gridPosition;
        }
        
        public void SetWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
        
        public abstract void Initialize();

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
            
        }

        public void OnReturnToPool()
        {
            OnDeactivate();
        }
    }
}