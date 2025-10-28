using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IGridEntity : IEntity
    {
        T RequestEntityComponent<T>() where T : class, IEntityComponent;
        Vector2Int BoardIndex { get; }
        void SetBoardIndex(Vector2Int gridPosition);
        void SetWorldPosition(Vector3 worldPosition);
        Transform WorldTransform { get; }
    }
}