using Datas.BoardDatas;
using UnityEngine;
using UnityEngine.Serialization;

namespace Datas.Configs
{
    [CreateAssetMenu(fileName = "New Board Preparation Config", menuName = "Configs/Board/Board Preparation Config")]
    public class BoardPreparationConfig : ScriptableObject
    {
        [FormerlySerializedAs("boardSizeData")] [SerializeField]
        private BoardSizeData _boardSizeData;
        
        public BoardSizeData BoardSizeData => _boardSizeData;
        
    }
}