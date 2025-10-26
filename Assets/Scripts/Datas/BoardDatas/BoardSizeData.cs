using System;
using UnityEngine;

namespace Datas.BoardDatas
{
    [Serializable]
    public struct BoardSizeData
    {
        [SerializeField] private int _rowNumber;
        [SerializeField] private int _columnNumber;
        [SerializeField] private float _cellSize;
        [SerializeField] private Vector3 _boardCenterPosition;
        
        public int RowNumber => _rowNumber;
        public int ColumnNumber => _columnNumber;
        public float CellSize => _cellSize;
        public Vector3 BoardCenterPosition => _boardCenterPosition;
    }
}