using Datas.EntityDatas.EnemyDatas;

namespace Gameplay.Interfaces
{
    public interface IEnemyEntity : IBlockEntity
    {
        EnemyEntityData EnemyEntityData { get; }
        void TakeDamage(float damage);
    }
}