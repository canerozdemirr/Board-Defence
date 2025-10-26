using Datas.BoardDatas;
using Events.Board;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using Gameplay.Objects.Entities.Board_Entities;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Board
{
    public class BoardSpawner : MonoBehaviour, IInitializable
    {
        private BoardCellEntity _cellEntityPrefab;

        private GameObjectPool<BoardCellEntity> _boardCellPool;

        private BoardSizeData _boardSizeData;

        public void Initialize()
        {
            EventBus.Subscribe<BoardDataReady>(OnBoardDataReady);
        }

        private void OnBoardDataReady(BoardDataReady boardDataReadyEvent)
        {
            _boardSizeData = boardDataReadyEvent.BoardSizeData;
            _boardCellPool = GameObjectPool<BoardCellEntity>.Create(_cellEntityPrefab.gameObject, transform, _boardSizeData.RowNumber * _boardSizeData.ColumnNumber);
            
            for (int row = 0; row < _boardSizeData.RowNumber; row++)
            {
                for (int col = 0; col < _boardSizeData.ColumnNumber; col++)
                {
                    IGridEntity boardCellEntity = _boardCellPool.Spawn();
                    Vector2Int boardIndex = new(row, col);
                    Vector3 worldPositionInBoard = CalculateCenteredCellPosition(row, col);
                    
                    boardCellEntity.SetWorldPosition(worldPositionInBoard);
                    boardCellEntity.SetBoardIndex(boardIndex);
                    boardCellEntity.Initialize();
                    boardCellEntity.OnActivate();
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

        public void InjectDependencies(BoardCellEntity cellEntityPrefab)
        {
            _cellEntityPrefab = cellEntityPrefab;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BoardDataReady>(OnBoardDataReady);
            _cellEntityPrefab = null;
            _boardCellPool.ClearObjectReferences();
            _boardCellPool = null;
        }
    }
}