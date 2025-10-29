using UnityEngine;

namespace Datas.Configs.Level_Configs
{
    [CreateAssetMenu(fileName = "New Level Config", menuName = "Configs/Levels/Create Level Config")]
    public class LevelConfig : BaseDataConfig
    {
        [SerializeField] private LevelData[] _levelDataList;
        
        public LevelData[] LevelDataList => _levelDataList;
    }
}