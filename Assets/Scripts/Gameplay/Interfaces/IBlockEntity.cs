using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IBlockEntity : IEntity
    {
        T RequestEntityComponent<T>() where T : class, IEntityComponent;
        Vector2Int BoardIndex { get; }
        void SetBoardIndex(Vector2Int blockPosition);
        void SetWorldPosition(Vector3 worldPosition);
        Transform WorldTransform { get; }
    }
}