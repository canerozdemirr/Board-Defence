using Datas.EntityDatas.EnemyDatas;

namespace Gameplay.Interfaces
{
    public interface IEnemyEntity : IGridEntity
    {
        EnemyEntityData EnemyEntityData { get; }
    }
}