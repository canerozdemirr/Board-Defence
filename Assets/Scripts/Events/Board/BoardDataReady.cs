using Datas.BoardDatas;
using Events.Interfaces;

namespace Events.Board
{
    public struct BoardDataReady : IEvent
    {
        public readonly BoardSizeData BoardSizeData;
        public readonly BoardBlockData[,] BoardCellDataList;
        
        public BoardDataReady(BoardSizeData boardSizeData, BoardBlockData[,] boardCellDataList)
        {
            BoardSizeData = boardSizeData;
            BoardCellDataList = boardCellDataList;
        }
    }
}