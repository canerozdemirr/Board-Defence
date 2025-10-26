using Datas.EntityDatas;
using Datas.ItemDatas;
using UnityEngine;
using UnityEngine.Serialization;

namespace Datas.Configs.Entity_Configs.Tower_Configs
{
    [CreateAssetMenu(fileName = "New Tower Entity Config", menuName = "Configs/Entities/Create New Tower Entity")]
    public class TowerEntityConfig : ScriptableObject
    {
        [SerializeField] private TowerEntityData _towerEntityData;
        [FormerlySerializedAs("_towerEntityVisualData")] [SerializeField] private EntityVisualData _entityVisualData;
        
        public TowerEntityData TowerEntityData => _towerEntityData;
        public EntityVisualData EntityVisualData => _entityVisualData;
    }
}