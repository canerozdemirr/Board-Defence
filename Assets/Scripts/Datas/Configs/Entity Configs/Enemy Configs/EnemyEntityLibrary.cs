using UnityEngine;

namespace Datas.Configs.Entity_Configs.Enemy_Configs
{
    [CreateAssetMenu(fileName = "New Enemy Entity Library", menuName = "Configs/Libraries/Create Enemy Entity Library")]
    public class EnemyEntityLibrary : ScriptableObject
    {
        [SerializeField] private EnemyEntityConfig[] _enemyEntityConfigList;
        
        public EnemyEntityConfig[] EnemyEntityConfigList => _enemyEntityConfigList;
    }
}