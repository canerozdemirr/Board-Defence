using Datas.ItemDatas;
using UnityEngine;

namespace Datas.Configs.Item_Configs
{
    [CreateAssetMenu(fileName = "New Tower Entity Config", menuName = "Configs/Items/Create New Tower Entity")]
    public class TowerEntityConfig : ScriptableObject
    {
        [SerializeField] private TowerEntityData _towerEntityData;
        [SerializeField] private TowerEntityVisualData _towerEntityVisualData;
        
        public TowerEntityData TowerEntityData => _towerEntityData;
        public TowerEntityVisualData TowerEntityVisualData => _towerEntityVisualData;
    }
}