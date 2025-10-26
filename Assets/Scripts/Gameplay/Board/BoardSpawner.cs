using System;
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
        
        public void Initialize()
        {
            EventBus.Subscribe<BoardDataReady>(OnBoardDataReady);
        }

        private void OnBoardDataReady(BoardDataReady boardDataReadyEvent)
        {
            int rowNumber = boardDataReadyEvent.BoardSizeData.RowNumber;
            int columnNumber = boardDataReadyEvent.BoardSizeData.ColumnNumber;
            float cellSize = boardDataReadyEvent.BoardSizeData.CellSize;
            Vector3 boardOriginPosition = boardDataReadyEvent.BoardSizeData.BoardOriginPosition;
            
            _boardCellPool = GameObjectPool<BoardCell>.Create(_cellPrefab.gameObject, transform, rowNumber * columnNumber);
            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    BoardCell boardCell = _boardCellPool.Spawn();
                    boardCell.transform.position = new Vector3(
                        boardOriginPosition.x + j * cellSize,
                        boardOriginPosition.y,
                        boardOriginPosition.z + i * cellSize
                    );
                    boardCell.gameObject.name = "Board Cell: (" + i + ", " + j + ")";
                    boardCell.Initialize(boardDataReadyEvent.BoardCellDataList[i, j]);
                }
            }
        }

        public void InjectDependencies(BoardCell cellPrefab)
        {
            _cellPrefab = cellPrefab;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BoardDataReady>(OnBoardDataReady);
        }
    }
}