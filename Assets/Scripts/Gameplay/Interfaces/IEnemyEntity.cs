using Datas.EntityDatas.EnemyDatas;

namespace Gameplay.Interfaces
{
    public interface IEnemyEntity 
    {
        T RequestComponent<T>() where T : class, IEnemyEntityComponent;
        EnemyEntityData EnemyEntityData { get; }
    }
}