using Datas.EntityDatas.ProjectileDatas;

namespace Gameplay.Interfaces
{
    public interface IProjectileEntity : IEntity
    {
        void AssignProjectileData(ProjectileEntityData projectileData);
        void SetTarget(IEnemyEntity target);
        void SetDamage(float damage);
    }
}
