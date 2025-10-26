using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IGridEntity : IEntity
    {
        Vector2Int BoardIndex { get; }
        void SetBoardIndex(Vector2Int gridPosition);
        void SetWorldPosition(Vector3 worldPosition);
    }
}