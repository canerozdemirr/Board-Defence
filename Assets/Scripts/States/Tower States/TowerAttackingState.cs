using System;
using Datas.EntityDatas.TowerDatas;
using Gameplay.Interfaces;
using UnityEngine;
using Utilities.Helpers;

namespace States.Tower_States
{
    [Serializable]
    public class TowerAttackingState : BaseState<ITowerEntity>
    {
        private IEnemyEntity _targetEnemy;
        private TowerEntityData _towerEntityData;
        private float _attackTimer;
        private float _range;
        private Direction _detectionDirections;
        private Vector2Int[] _directionList;

        public IEnemyEntity TargetEnemy => _targetEnemy;

        public void SetTarget(IEnemyEntity target)
        {
            _targetEnemy = target;
        }

        public override void OnEnter(ITowerEntity context)
        {
            _attackTimer = 0f;
            _towerEntityData = context.TowerEntityData;
            _range = _towerEntityData.Range;
            _detectionDirections = _towerEntityData.DetectionDirections;
            _directionList = DirectionHelper.GetDirectionsFromDetectionFlag(_detectionDirections);
        }

        public override void OnUpdate(ITowerEntity context)
        {
            if (_targetEnemy == null)
            {
                //This is for enemy death case
                FinishState();
                return;
            }

            if (!IsEnemyInRange(context))
            {
                // This is for enemy moving out of range case
                _targetEnemy = null;
                FinishState();
                return;
            }

            _attackTimer += Time.deltaTime;

            if (!(_attackTimer >= _towerEntityData.AttackInterval))
                return;

            Attack();
            _attackTimer = 0f;
        }

        private bool IsEnemyInRange(ITowerEntity tower)
        {
            Vector2Int towerBlockIndex = tower.BoardIndex;
            Vector2Int enemyBlockIndex = _targetEnemy.BoardIndex;
            int rangeInBlocks = Mathf.RoundToInt(_range);

            foreach (Vector2Int direction in _directionList)
            {
                for (int i = 1; i <= rangeInBlocks; i++)
                {
                    Vector2Int blockToCheck = towerBlockIndex + direction * i;
                    if (blockToCheck == enemyBlockIndex)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void OnExit(ITowerEntity context)
        {
            _targetEnemy = null;
            _attackTimer = 0f;
        }

        private void Attack()
        {
            Debug.Log("ATTACKKKKKKK");
        }
    }
}
