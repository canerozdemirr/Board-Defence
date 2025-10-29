using System;
using System.Collections.Generic;
using Datas.BoardDatas;
using Datas.Configs.Board_Configs;
using Events.Board;
using Gameplay.Interfaces;
using Systems.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class BoardSystem : IBoardSystem, IInitializable, IDisposable
    {
        private readonly BoardPreparationConfig _boardPreparationConfig;
        
        private BoardSizeData _boardSizeData;
        private BoardBlockData[,] _boardBlockDataList;
        private Dictionary<Vector2Int, List<IBlockEntity>> _blockToEntityMap;
        public BoardSizeData BoardSizeData => _boardSizeData;

        public BoardSystem(BoardPreparationConfig boardPreparationConfig)
        {
            _boardPreparationConfig = boardPreparationConfig;
        }

        public void Initialize()
        {
            _blockToEntityMap = new Dictionary<Vector2Int, List<IBlockEntity>>();
            PrepareBoardData();
        }

        private void PrepareBoardData()
        {
            _boardSizeData = _boardPreparationConfig.BoardSizeData;
            int rowNumber = _boardSizeData.RowNumber;
            int columnNumber = _boardSizeData.ColumnNumber;
            _boardBlockDataList = new BoardBlockData[rowNumber, columnNumber];

            for (int row = 0; row < rowNumber; row++)
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    BlockType blockType = BlockType.EnemyZone;
                    if (row >= _boardPreparationConfig.BoardSizeData.PlayerRowLimit
                        && column <= _boardPreparationConfig.BoardSizeData.PlayerColumnLimit)
                    {
                        blockType = BlockType.PlayerZone;
                    }
                    
                    _boardBlockDataList[row, column] =
                        new BoardBlockData(new Vector2Int(row, column), blockType, BlockState.Empty);
                }
            }

            //I am publishing the event here to notify that the board data is ready, it will be used by the spawner as well as other systems if needed, like providing data for AI decision making in advanced cases or providing visual indicators for the player.
            EventBus.Publish(new BoardDataReady(_boardSizeData, _boardBlockDataList));
        }

        public Vector3 GetWorldPositionFromBlock(Vector2Int blockIndex)
        {
            return _boardSizeData.CalculateCenteredCellPosition(blockIndex.x, blockIndex.y);
        }

        public bool IsBlockOccupied(Vector2Int blockIndex)
        {
            return _boardBlockDataList[blockIndex.x, blockIndex.y].BlockState == BlockState.Occupied;
        }

        public void OccupyBlock(Vector2Int blockIndex)
        {
            _boardBlockDataList[blockIndex.x, blockIndex.y] = new BoardBlockData(
                blockIndex,
                _boardBlockDataList[blockIndex.x, blockIndex.y].BlockType,
                BlockState.Occupied
            );
        }

        public void FreeBlock(Vector2Int blockIndex)
        {
            _boardBlockDataList[blockIndex.x, blockIndex.y] = new BoardBlockData(
                blockIndex,
                _boardBlockDataList[blockIndex.x, blockIndex.y].BlockType,
                BlockState.Empty
            );
        }

        public bool IsValidPlacementPosition(Vector2Int blockIndex)
        {
            BoardBlockData blockData = _boardBlockDataList[blockIndex.x, blockIndex.y];
            return blockData is { BlockType: BlockType.PlayerZone, BlockState: BlockState.Empty };
        }

        public void AddEntityAtBlock(Vector2Int blockIndex, IBlockEntity entity)
        {
            if (!_blockToEntityMap.ContainsKey(blockIndex))
            {
                _blockToEntityMap[blockIndex] = new List<IBlockEntity>();
            }
            _blockToEntityMap[blockIndex].Add(entity);
            entity.SetBoardIndex(blockIndex);
        }

        public void RemoveEntityAtBlock(Vector2Int blockIndex, IBlockEntity entity)
        {
            if (_blockToEntityMap.ContainsKey(blockIndex))
            {
                _blockToEntityMap[blockIndex].Remove(entity);
            }
        }

        public List<IBlockEntity> GetEntitiesAtBlock(Vector2Int blockIndex)
        {
            return _blockToEntityMap.GetValueOrDefault(blockIndex);
        }

        public void Dispose()
        {
            _boardBlockDataList = null;
            _boardSizeData = default;
            _blockToEntityMap.Clear();
            _blockToEntityMap = null;
        }
    }
}