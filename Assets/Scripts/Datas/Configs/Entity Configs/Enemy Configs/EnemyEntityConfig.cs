using Datas.EntityDatas;
using Datas.EntityDatas.EnemyDatas;
using UnityEngine;

namespace Datas.Configs.Entity_Configs.Enemy_Configs
{
    [CreateAssetMenu(fileName = "New Enemy Entity Config", menuName = "Configs/Entities/Create New Enemy Entity")]
    public class EnemyEntityConfig : ScriptableObject
    {
        [SerializeField] private EnemyEntityData _enemyEntityData;
        [SerializeField] private EntityVisualData _entityVisualData;
        
        public EnemyEntityData EnemyEntityData => _enemyEntityData;
        public EntityVisualData EntityVisualData => _entityVisualData;
    }
}