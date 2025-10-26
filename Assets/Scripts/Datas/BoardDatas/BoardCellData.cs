using System;
using UnityEngine;

namespace Datas.BoardDatas
{
    [Serializable]
    public struct BoardCellData
    {
        [SerializeField] private Vector2Int _boardIndex;
        [SerializeField] private CellType _cellType;
        [SerializeField] private CellState _cellState;
        
        public Vector2Int BoardIndex => _boardIndex;
        public CellType CellType => _cellType;
        public CellState CellState => _cellState;
    }
    
    public enum CellType
    {
        PlayerZone,
        EnemyZone,      
        NeutralZone
    }
    
    
    public enum CellState
    {
        //TODO: I wont be using all of these states, because it would require more complex gameplay mechanics to make use of them. But, it is good to have them defined for future expansions as the gameplay deepens.
        Empty,
        Occupied,
        Reserved,
        Blocked
    }
}