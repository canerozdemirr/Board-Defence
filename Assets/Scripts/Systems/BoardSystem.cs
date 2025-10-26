using System;
using Datas.BoardDatas;
using Datas.Configs;
using Datas.Configs.Board_Configs;
using Events.Board;
using UnityEngine;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class BoardSystem : IInitializable, IDisposable
    {
        private BoardPreparationListConfig _boardPreparationListConfig;
        private BoardPreparationConfig _boardPreparationConfig;

        private BoardSizeData _boardSizeData;
        private BoardCellData[,] _boardCellDataList;

        public BoardSystem(BoardPreparationListConfig boardPreparationListConfig)
        {
            _boardPreparationListConfig = boardPreparationListConfig;
        }

        public void Initialize()
        {
            _boardPreparationConfig = _boardPreparationListConfig.GetCurrentBoardPreparationConfig();
            PrepareBoardData();
        }

        private void PrepareBoardData()
        {
            _boardSizeData = _boardPreparationConfig.BoardSizeData;
            int rowNumber = _boardSizeData.RowNumber;
            int columnNumber = _boardSizeData.ColumnNumber;
            _boardCellDataList = new BoardCellData[rowNumber, columnNumber];

            //TODO: This is temporary. I will add this section to the config later
            int playerZoneStartRow = rowNumber / 2;

            for (int row = 0; row < rowNumber; row++)
            {
                for (int col = 0; col < columnNumber; col++)
                {
                    CellType cellType = row >= playerZoneStartRow ? CellType.PlayerZone : CellType.EnemyZone;
                    _boardCellDataList[row, col] =
                        new BoardCellData(new Vector2Int(row, col), cellType, CellState.Empty);
                }
            }

            //I am publishing the event here to notify that the board data is ready, it will be used by the spawner as well as other systems if needed, like providing data for AI decision making in advanced cases or providing visual indicators for the player.
            EventBus.Publish(new BoardDataReady(_boardSizeData, _boardCellDataList));
        }

        public void Dispose()
        {
        }
    }
}