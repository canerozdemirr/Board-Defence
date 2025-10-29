using Cysharp.Threading.Tasks;
using Gameplay.Objects.Entities;

namespace Gameplay.Interfaces
{
    public interface ITowerSpawner
    {
        UniTask<TowerEntity> ProvideTowerEntity(string towerName);
        void ReturnTowerToPool(TowerEntity tower);
    }
}