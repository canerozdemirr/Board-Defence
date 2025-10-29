using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datas.Configs.Entity_Configs.Tower_Configs;
using Datas.EntityDatas.TowerDatas;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using Gameplay.Objects.Entities.Item_Entities;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class TowerSpawner : MonoBehaviour, ITowerSpawner, IInitializable
    {
        private Dictionary<string, AddressableGameObjectPool<TowerEntity>> _towerEntityPoolMap;
        private TowerEntityLibrary _towerEntityLibrary;
        private List<TowerEntity> _activeTowers;

        [Inject] private DiContainer _container;

        public void SetTowerEntityLibrary(TowerEntityLibrary towerEntityLibrary)
        {
            _towerEntityLibrary = towerEntityLibrary;
        }

        public void Initialize()
        {
            _towerEntityPoolMap = new Dictionary<string, AddressableGameObjectPool<TowerEntity>>();
            _activeTowers = new List<TowerEntity>();
        }

        public async UniTask<TowerEntity> ProvideTowerEntity(string towerName)
        {
            TowerEntity spawnedTower;
            TowerEntityData towerEntityData = default;

            if (_towerEntityPoolMap.ContainsKey(towerName))
            {
                spawnedTower = _towerEntityPoolMap[towerName].Spawn();
                _container.InjectGameObject(spawnedTower.gameObject);

                foreach (TowerEntityConfig entityConfig in _towerEntityLibrary.TowerEntityConfigList)
                {
                    towerEntityData = entityConfig.TowerEntityData.Clone();
                    if (towerEntityData.ItemName != towerName)
                        continue;
                    spawnedTower.AssignTowerData(towerEntityData);
                    break;
                }

                _activeTowers.Add(spawnedTower);
                return spawnedTower;
            }

            AddressableGameObjectPool<TowerEntity> towerPool = null;
            foreach (TowerEntityConfig towerEntityConfig in _towerEntityLibrary.TowerEntityConfigList)
            {
                towerEntityData = towerEntityConfig.TowerEntityData.Clone();
                if (towerEntityData.ItemName != towerName)
                    continue;

                towerPool = await AddressableGameObjectPool<TowerEntity>.CreateAsync(
                    towerEntityConfig.EntityVisualData.AssetReference, transform);
                break;
            }

            _towerEntityPoolMap.Add(towerName, towerPool);
            spawnedTower = _towerEntityPoolMap[towerName].Spawn();
            _container.InjectGameObject(spawnedTower.gameObject);
            spawnedTower.AssignTowerData(towerEntityData.Clone());
            _activeTowers.Add(spawnedTower);
            return spawnedTower;
        }

        public void ReturnTowerToPool(TowerEntity tower)
        {
            string towerName = tower.TowerEntityData.ItemName;
            if (!_towerEntityPoolMap.TryGetValue(towerName, out AddressableGameObjectPool<TowerEntity> pool))
                return;

            _activeTowers.Remove(tower);
            tower.OnDeactivate();
            pool.DeSpawn(tower);
        }

        public void ReturnAllTowersToPool()
        {
            for (int i = _activeTowers.Count - 1; i >= 0; i--)
            {
                TowerEntity tower = _activeTowers[i];
                ReturnTowerToPool(tower);
            }
        }

        private void OnDestroy()
        {
            foreach (AddressableGameObjectPool<TowerEntity> pool in _towerEntityPoolMap.Values)
            {
                pool.ClearObjectReferences();
            }

            _towerEntityPoolMap.Clear();
            _towerEntityPoolMap = null;
            _towerEntityLibrary = null;
            _activeTowers.Clear();
            _activeTowers = null;
        }
    }
}