using Datas.EntityDatas;
using Datas.EntityDatas.ProjectileDatas;
using UnityEngine;

namespace Datas.Configs.Entity_Configs.Projectile_Configs
{
    [CreateAssetMenu(fileName = "New Projectile Entity Config", menuName = "Configs/Entities/Create New Projectile Entity")]
    public class ProjectileEntityConfig : BaseDataConfig
    {
        [SerializeField] private ProjectileEntityData _projectileEntityData;
        [SerializeField] private EntityVisualData _entityVisualData;

        public ProjectileEntityData ProjectileEntityData => _projectileEntityData;
        public EntityVisualData EntityVisualData => _entityVisualData;
    }
}
