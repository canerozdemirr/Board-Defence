using System;
using System.Collections.Generic;
using Datas.EntityDatas.TowerDatas;
using Gameplay.Interfaces;
using Systems.Interfaces;
using UnityEngine;
using Utilities.Helpers;
using Zenject;

namespace States.Tower_States
{
    [Serializable]
    public class TowerScanningState : BaseState<ITowerEntity>
    {
        [Inject] private IBoardSystem _boardSystem;

        private IEnemyEntity _detectedEnemy;
        private TowerEntityData _towerEntityData;
        private float _range;
        private Direction _detectionDirections;
        
        private Vector2Int[] _directionList;
        private Vector2Int _towerBlockIndex;
        private List<IBlockEntity> _blockEntityList;
        
        public IEnemyEntity DetectedEnemy => _detectedEnemy;
        
        public override void OnEnter(ITowerEntity context)
        {
            _detectedEnemy = null;
            _towerEntityData = context.TowerEntityData;
            _range = _towerEntityData.Range;
            _detectionDirections = _towerEntityData.DetectionDirections;
            _directionList = DirectionHelper.GetDirectionsFromDetectionFlag(_detectionDirections);
            _towerBlockIndex = context.BoardIndex;
        }

        public override void OnUpdate(ITowerEntity context)
        {
            if (ScanForEnemies() != null)
            {
                FinishState();
            }
        }

        public override void OnExit(ITowerEntity context)
        {

        }

        private IEnemyEntity ScanForEnemies()
        {
            int rangeInBlocks = Mathf.RoundToInt(_range);

            foreach (Vector2Int direction in _directionList)
            {
                _detectedEnemy = ScanDirection(_towerBlockIndex, direction, rangeInBlocks);
                if (_detectedEnemy != null) 
                    return _detectedEnemy;
            }

            return null;
        }

        private IEnemyEntity ScanDirection(Vector2Int startBlock, Vector2Int direction, int range)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int blockToCheck = startBlock + direction * i;
                _blockEntityList = _boardSystem.GetEntitiesAtBlock(blockToCheck);

                foreach (IBlockEntity entity in _blockEntityList)
                {
                    if (entity is IEnemyEntity enemyEntity)
                    {
                        return enemyEntity;
                    }
                }
            }

            return null;
        }
    }
}
