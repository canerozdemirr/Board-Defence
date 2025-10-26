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
        
        public abstract void Initialize();

        public abstract void OnActivate();

        public abstract void OnDeactivate();

        public void OnCalledFromPool()
        {
            OnActivate();
        }

        public void OnReturnToPool()
        {
            OnDeactivate();
        }
    }
}