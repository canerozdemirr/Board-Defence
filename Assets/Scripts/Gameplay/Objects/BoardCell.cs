using Datas.BoardDatas;
using UnityEngine;

namespace Gameplay.Objects
{
    public class BoardCell : MonoBehaviour, IPoolable
    {
        private BoardCellData _boardCellData;
        public void Initialize(BoardCellData boardCellData)
        {
            _boardCellData = boardCellData;
        }
        public void OnCalledFromPool()
        {
            
        }

        public void OnReturnToPool()
        {
            
        }
    }
}