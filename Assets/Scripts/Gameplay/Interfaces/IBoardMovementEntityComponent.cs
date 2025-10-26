using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IBoardMovementEntityComponent : IEnemyEntityComponent
    {
        bool CanMoveTo(Vector2Int targetGridPosition);
        void StopMovement();
    }
}