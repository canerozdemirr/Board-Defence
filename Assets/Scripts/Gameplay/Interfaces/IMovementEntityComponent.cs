using System;
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IMovementEntityComponent : IEntityComponent
    {
        Vector2Int CurrentBlockIndex { get; }
        bool IsMoving { get; }
        void MoveInDirection(Vector2Int direction);
        event Action ReachToEndBlock;
        void StopMovement();
    }
}