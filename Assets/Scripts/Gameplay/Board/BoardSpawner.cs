using System;
using Datas.BoardDatas;
using Events.Board;
using Gameplay.Objects;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Board
{
    public class BoardSpawner : MonoBehaviour, IInitializable
    {
        private BoardCell _cellPrefab;

        private GameObjectPool<BoardCell> _boardCellPool;

        private BoardSizeData _boardSizeData;

        public void Initialize()
        {
            EventBus.Subscribe<BoardDataReady>(OnBoardDataReady);
        }

        private void OnBoardDataReady(BoardDataReady boardDataReadyEvent)
        {
            _boardSizeData = boardDataReadyEvent.BoardSizeData;
            _boardCellPool = GameObjectPool<BoardCell>.Create(_cellPrefab.gameObject, transform, _boardSizeData.RowNumber * _boardSizeData.ColumnNumber);
            
            for (int row = 0; row < _boardSizeData.RowNumber; row++)
            {
                for (int col = 0; col < _boardSizeData.ColumnNumber; col++)
                {
                    BoardCell boardCell = _boardCellPool.Spawn();
                    boardCell.transform.position = CalculateCenteredCellPosition(row, col);
                    boardCell.gameObject.name = $"Board Cell: ({row}, {col})";
                    boardCell.Initialize(boardDataReadyEvent.BoardCellDataList[row, col]);
                }
            }
        }

        private Vector3 CalculateCenteredCellPosition(int row, int col)
        {
            float halfWidth = (_boardSizeData.RowNumber - 1) * _boardSizeData.CellSize / 2f;
            float halfDepth = (_boardSizeData.ColumnNumber - 1) * _boardSizeData.CellSize / 2f;

            float x = row * _boardSizeData.CellSize - halfWidth;
            float z = col * _boardSizeData.CellSize - halfDepth;

            return _boardSizeData.BoardCenterPosition + new Vector3(x, _boardSizeData.CellYPosition, z);
        }

        public void InjectDependencies(BoardCell cellPrefab)
        {
            _cellPrefab = cellPrefab;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BoardDataReady>(OnBoardDataReady);
            _cellPrefab = null;
            _boardCellPool.ClearObjectReferences();
            _boardCellPool = null;
        }
    }
}