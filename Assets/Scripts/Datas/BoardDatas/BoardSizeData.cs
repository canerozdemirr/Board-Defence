using System;
using UnityEngine;

namespace Datas.BoardDatas
{
    [Serializable]
    public struct BoardSizeData
    {
        [SerializeField] [Min(0f)] private int _rowNumber;
        [SerializeField] [Min(0f)] private int _columnNumber;
        [SerializeField] [Min(1f)] private float _cellSize;
        [SerializeField] private Vector3 _boardCenterPosition;
        [SerializeField] private float _cellYPosition;

        [SerializeField] [Min(0f)] private int _playerRowLimit;
        [SerializeField] [Min(0f)] private int _playerColumnLimit;
        
        public int RowNumber => _rowNumber;
        public int ColumnNumber => _columnNumber;
        public float CellSize => _cellSize;
        public Vector3 BoardCenterPosition => _boardCenterPosition;
        public float CellYPosition => _cellYPosition;
        public int PlayerRowLimit => _playerRowLimit;
        public int PlayerColumnLimit => _playerColumnLimit;
        
        public Vector3 CalculateCenteredCellPosition(int row, int col)
        {
            float halfWidth = (_rowNumber - 1) * _cellSize / 2f;
            float halfDepth = (_columnNumber - 1) * _cellSize / 2f;

            float x = row * _cellSize - halfWidth;
            float z = col * _cellSize - halfDepth;

            return _boardCenterPosition + new Vector3(x, _cellYPosition, z);
        }
    }
}