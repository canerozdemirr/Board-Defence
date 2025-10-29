using System;
using NaughtyAttributes;
using UnityEngine;

namespace Datas.BoardDatas
{
    [Serializable]
    public struct BoardSizeData
    {
        [SerializeField] [Min(0f)] private int _rowNumber;
        [SerializeField] [Min(0f)] private int _columnNumber;
        [SerializeField] [ReadOnly] [Min(1f)] private float _blockSize;
        [SerializeField] private Vector3 _boardCenterPosition;
        [SerializeField] private float _cellYPosition;

        [SerializeField] [Min(0f)] private int _playerRowLimit;
        [SerializeField] [Min(0f)] private int _playerColumnLimit;
        
        public int RowNumber => _rowNumber;
        public int ColumnNumber => _columnNumber;
        public float BlockSize => _blockSize;
        public Vector3 BoardCenterPosition => _boardCenterPosition;
        public float CellYPosition => _cellYPosition;
        public int PlayerRowLimit => _playerRowLimit;
        public int PlayerColumnLimit => _playerColumnLimit;
        
        public Vector3 CalculateCenteredCellPosition(int row, int col)
        {
            float halfWidth = (_rowNumber - 1) * _blockSize / 2f;
            float halfDepth = (_columnNumber - 1) * _blockSize / 2f;

            float x = row * _blockSize - halfWidth;
            float z = col * _blockSize - halfDepth;

            return _boardCenterPosition + new Vector3(x, _cellYPosition, z);
        }
    }
}