using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Objects.Entities
{
    public abstract class BaseGridEntity : MonoBehaviour, IGridEntity, IPoolable
    {
        public Vector2Int GridPosition { get; protected set; }
        public Vector3 WorldPosition { get; protected set; }
        
        public void SetGridPosition(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            WorldPosition = worldPosition;
        }
        
        public abstract void Initialize();

        public abstract void Activate();

        public abstract void Deactivate();

        public virtual void OnCalledFromPool()
        {
            
        }

        public virtual void OnReturnToPool()
        {
            
        }
    }
}