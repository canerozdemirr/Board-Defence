using Datas.BoardDatas;
using Events.Board;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities.Board_Entities;
using Systems.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class BoardSpawner : MonoBehaviour, IInitializable
    {
        private BoardBlockEntity _blockEntityPrefab;

        private GameObjectPool<BoardBlockEntity> _boardCellPool;

        private BoardSizeData _boardSizeData;

        [Inject] private IBoardSystem _boardSystem;

        public void Initialize()
        {
            EventBus.Subscribe<BoardDataReady>(OnBoardDataReady);
        }

        private void OnBoardDataReady(BoardDataReady boardDataReadyEvent)
        {
            _boardSizeData = boardDataReadyEvent.BoardSizeData;
            _boardCellPool = GameObjectPool<BoardBlockEntity>.Create(_blockEntityPrefab.gameObject, transform, _boardSizeData.RowNumber * _boardSizeData.ColumnNumber);
            
            for (int row = 0; row < _boardSizeData.RowNumber; row++)
            {
                for (int col = 0; col < _boardSizeData.ColumnNumber; col++)
                {
                    IBlockEntity boardBlockEntity = _boardCellPool.Spawn();
                    Vector2Int boardIndex = new(row, col);
                    Vector3 worldPositionInBoard = _boardSizeData.CalculateCenteredCellPosition(row, col);
                    
                    boardBlockEntity.SetWorldPosition(worldPositionInBoard);
                    boardBlockEntity.SetBoardIndex(boardIndex);
                    boardBlockEntity.Initialize();
                    boardBlockEntity.OnActivate();
                    _boardSystem.AddEntityAtBlock(boardIndex, boardBlockEntity);
                }
            }
        }

        public void InjectDependencies(BoardBlockEntity blockEntityPrefab)
        {
            _blockEntityPrefab = blockEntityPrefab;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BoardDataReady>(OnBoardDataReady);
            _blockEntityPrefab = null;
            _boardCellPool.ClearObjectReferences();
            _boardCellPool = null;
        }
    }
}