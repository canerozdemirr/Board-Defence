using Datas.EntityDatas.TowerDatas;

namespace Gameplay.Interfaces
{
    public interface ITowerEntity : IBlockEntity
    {
        TowerEntityData TowerEntityData { get; }
        void AssignTowerData(TowerEntityData towerEntityData);
    }
}