using UnityEngine;

namespace Datas.Configs.Entity_Configs.Projectile_Configs
{
    [CreateAssetMenu(fileName = "New Projectile Entity Library", menuName = "Configs/Libraries/Create Projectile Entity Library")]
    public class ProjectileEntityLibrary : ScriptableObject
    {
        [SerializeField] private ProjectileEntityConfig[] _projectileEntityConfigList;

        public ProjectileEntityConfig[] ProjectileEntityConfigList => _projectileEntityConfigList;
    }
}
