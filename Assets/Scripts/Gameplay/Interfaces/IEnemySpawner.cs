using Cysharp.Threading.Tasks;
using Datas.EntityDatas.EnemyDatas;
using Gameplay.Objects.Entities;

namespace Gameplay.Interfaces
{
    public interface IEnemySpawner
    {
        UniTask<EnemyEntity> ProvideEnemyEntity(EnemyClass enemyClass);
        void ReturnAllEnemiesToPool();
    }
}