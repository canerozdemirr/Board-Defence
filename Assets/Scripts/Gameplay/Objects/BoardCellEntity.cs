using Datas.BoardDatas;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Objects
{
    public class BoardCellEntity : MonoBehaviour, IPoolable
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