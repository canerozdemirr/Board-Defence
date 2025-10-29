using System;
using UnityEngine;

namespace Datas.BoardDatas
{
    [Serializable]
    public struct BoardBlockData
    {
        private BlockType _blockType;
        private BlockState _blockState;
        
        public BoardBlockData(Vector2Int boardIndex, BlockType blockType, BlockState blockState)
        {
            _blockType = blockType;
            _blockState = blockState;
        }

        public BlockType BlockType => _blockType;
        public BlockState BlockState => _blockState;
    }
    
    public enum BlockType
    {
        PlayerZone,
        EnemyZone,      
        NeutralZone
    }
    
    
    public enum BlockState
    {
        //TODO: I wont be using all of these states, because it would require more complex gameplay mechanics to make use of them. But, it is good to have them defined for future expansions as the gameplay deepens.
        Empty,
        Occupied,
        Reserved,
        Blocked
    }
}