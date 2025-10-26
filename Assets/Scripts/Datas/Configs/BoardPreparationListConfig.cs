using UnityEngine;

namespace Datas.Configs
{
    [CreateAssetMenu(fileName = "New Board Preparation List Config", menuName = "Configs/Board/Board Preparation List Config")]
    public class BoardPreparationListConfig : ScriptableObject
    {
        [SerializeField] private BoardPreparationConfig[] _boardPreparationConfigList;
        
        public BoardPreparationConfig[] BoardPreparationConfigList => _boardPreparationConfigList;
        
        public BoardPreparationConfig GetCurrentBoardPreparationConfig(int index = 0)
        {
            // For the sake of simplicity, I am returning the first config if the index is out of range.
            // In a more complex implementation, I would handle this differently, like fetching from a save system or picking out custom board configurations from a menu.
            return _boardPreparationConfigList.Length > index ? _boardPreparationConfigList[index] : _boardPreparationConfigList[0];
        }
    }
}