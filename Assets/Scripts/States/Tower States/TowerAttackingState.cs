using System;
using Cysharp.Threading.Tasks;
using Datas.EntityDatas.TowerDatas;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using UnityEngine;
using Utilities;
using Utilities.Helpers;
using Zenject;

namespace States.Tower_States
{
    [Serializable]
    public class TowerAttackingState : BaseState<ITowerEntity>
    {
        [Inject] private IProjectileSpawner _projectileSpawner;

        private IEnemyEntity _targetEnemy;
        private TowerEntityData _towerEntityData;
        private IAnimateComponent _animateComponent;
        private float _attackTimer;
        private float _range;
        private Direction _detectionDirections;
        private Vector2Int[] _directionList;
        private ITowerEntity _towerContext;

        public IEnemyEntity TargetEnemy => _targetEnemy;

        public void SetTarget(IEnemyEntity target)
        {
            _targetEnemy = target;
        }

        public override void OnEnter(ITowerEntity context)
        {
            _towerContext = context;
            _towerEntityData = context.TowerEntityData;
            _animateComponent = context.RequestEntityComponent<IAnimateComponent>();
            _range = _towerEntityData.Range;
            _detectionDirections = _towerEntityData.DetectionDirections;
            _directionList = DirectionHelper.GetDirectionsFromDetectionFlag(_detectionDirections);

            Attack().Forget();
            _attackTimer = 0f;
        }

        public override void OnUpdate(ITowerEntity context)
        {
            if (_targetEnemy == null || !_targetEnemy.IsAlive)
            {
                //This is for enemy death case
                _targetEnemy = null;
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

            Attack().Forget();
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

        private async UniTask Attack()
        {
            await _animateComponent.PlayAnimationAsync(Constants.AttackAnimationTag);
            Vector3 spawnPosition = _towerContext.WorldTransform.position;
            ProjectileEntity projectile = await _projectileSpawner.ProvideProjectileEntity(_towerEntityData.ProjectileName, spawnPosition);
            projectile.SetTarget(_targetEnemy);
            projectile.SetDamage(_towerEntityData.Damage);
            projectile.Initialize();
            projectile.OnActivate();
        }
    }
}
