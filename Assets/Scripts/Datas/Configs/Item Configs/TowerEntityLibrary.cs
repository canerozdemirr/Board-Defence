using UnityEngine;

namespace Datas.Configs.Item_Configs
{
    [CreateAssetMenu(fileName = "New Tower Entity Library", menuName = "Configs/Entities/Create Tower Entity Library")]
    public class TowerEntityLibrary : ScriptableObject
    {
        [SerializeField] private TowerEntityConfig[] _defenceItemConfigList;
        
        public TowerEntityConfig[] DefenceItemConfigList => _defenceItemConfigList;
    }
}