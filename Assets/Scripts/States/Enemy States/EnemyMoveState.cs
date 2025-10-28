using System;
using Datas.ItemDatas;
using Gameplay.Interfaces;
using UnityEngine;
using Utilities.Helpers;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyMoveState : BaseState<IEnemyEntity>
    {
        private IMovementEntityComponent _movementComponent;
        private readonly int _targetColumn = 0;

        public override void OnEnter(IEnemyEntity context)
        {
            _movementComponent = context.RequestEntityComponent<IMovementEntityComponent>();
        }

        public override void OnUpdate(IEnemyEntity context)
        {
            if (_movementComponent is not { IsMoving: false }) 
                return;
            
            if (_movementComponent.CurrentBlockIndex.y <= _targetColumn)
            {
                FinishState();
                return;
            }

            Vector2Int columnDirection = DirectionHelper.GetDirectionFromDetectionFlag(Direction.Backward);
            _movementComponent.MoveInDirection(columnDirection);
        }

        public override void OnExit(IEnemyEntity context)
        {
            _movementComponent = null;
        }
    }
}
