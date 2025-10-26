using Datas.BoardDatas;
using Events.Interfaces;

namespace Events.Board
{
    public struct BoardDataReady : IEvent
    {
        public readonly BoardSizeData BoardSizeData;
        public readonly BoardCellData[,] BoardCellDataList;
        
        public BoardDataReady(BoardSizeData boardSizeData, BoardCellData[,] boardCellDataList)
        {
            BoardSizeData = boardSizeData;
            BoardCellDataList = boardCellDataList;
        }
    }
}