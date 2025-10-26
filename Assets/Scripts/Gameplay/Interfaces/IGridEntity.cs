using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IGridEntity : IEntity
    {
        Vector2Int GridPosition { get; }
        Vector3 WorldPosition { get;}
        
        void SetGridPosition(Vector2Int gridPosition);
        void SetWorldPosition(Vector3 worldPosition);
    }
}