using UnityEngine;

namespace Datas.Configs.Entity_Configs.Tower_Configs
{
    [CreateAssetMenu(fileName = "New Tower Entity Library", menuName = "Configs/Libraries/Create Tower Entity Library")]
    public class TowerEntityLibrary : ScriptableObject
    {
        [SerializeField] private TowerEntityConfig[] _towerEntityConfigList;
        
        public TowerEntityConfig[] TowerEntityConfigList => _towerEntityConfigList;
    }
}