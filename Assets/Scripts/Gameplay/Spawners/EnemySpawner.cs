using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datas;
using Datas.Configs.Entity_Configs.Enemy_Configs;
using Datas.EntityDatas.EnemyDatas;
using Events.Enemies;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner, IInitializable
    {
        private Dictionary<EnemyClass, AddressableGameObjectPool<EnemyEntity>> _enemyEntityPoolMap;
        private EnemyEntityLibrary _enemyEntityLibrary;
        private List<EnemyEntity> _enemyEntityList;

        [Inject] private DiContainer _container;

        public void SetEnemyEntityLibrary(EnemyEntityLibrary enemyEntityLibrary)
        {
            _enemyEntityLibrary = enemyEntityLibrary;
        }

        public void Initialize()
        {
            _enemyEntityPoolMap = new Dictionary<EnemyClass, AddressableGameObjectPool<EnemyEntity>>();
            _enemyEntityList = new List<EnemyEntity>();
            EventBus.Subscribe<EnemyDeath>(OnEnemyDeath);
        }

        public async UniTask<EnemyEntity> ProvideEnemyEntity(EnemyClass enemyClass)
        {
            EnemyEntity spawnedEnemy;
            EnemyEntityData enemyEntityData = default;
            if (_enemyEntityPoolMap.ContainsKey(enemyClass))
            {
                spawnedEnemy = _enemyEntityPoolMap[enemyClass].Spawn();
                _container.InjectGameObject(spawnedEnemy.gameObject);

                foreach (EnemyEntityConfig entityConfig in _enemyEntityLibrary.EnemyEntityConfigList)
                {
                    enemyEntityData = entityConfig.EnemyEntityData.Clone();
                    if (enemyEntityData.EnemyClass != enemyClass)
                        continue;
                    
                    enemyEntityData.SetID(entityConfig.ConfigID);
                    spawnedEnemy.AssignEnemyEntityData(enemyEntityData);
                    break;
                }

                _enemyEntityList.Add(spawnedEnemy);
                return spawnedEnemy;
            }

            AddressableGameObjectPool<EnemyEntity> enemyPool = null;
            foreach (EnemyEntityConfig enemyEntityConfig in _enemyEntityLibrary.EnemyEntityConfigList)
            {
                enemyEntityData = enemyEntityConfig.EnemyEntityData.Clone();
                if (enemyEntityData.EnemyClass != enemyClass)
                    continue;

                enemyPool = await AddressableGameObjectPool<EnemyEntity>.CreateAsync(
                    enemyEntityConfig.EntityVisualData.AssetReference, transform);
                break;
            }

            _enemyEntityPoolMap.Add(enemyClass, enemyPool);
            spawnedEnemy = _enemyEntityPoolMap[enemyClass].Spawn();
            _container.InjectGameObject(spawnedEnemy.gameObject);
            spawnedEnemy.AssignEnemyEntityData(enemyEntityData);
            _enemyEntityList.Add(spawnedEnemy);
            return spawnedEnemy;
        }

        private void OnEnemyDeath(EnemyDeath enemyDeathEvent)
        {
            EnemyEntity deadEnemy = enemyDeathEvent.EnemyEntity as EnemyEntity;
            
            if(deadEnemy == null)
                return;
            
            if (!_enemyEntityList.Contains(deadEnemy))
                return;

            EnemyClass enemyClass = deadEnemy.EnemyEntityData.EnemyClass;
            _enemyEntityList.Remove(deadEnemy);
            _enemyEntityPoolMap[enemyClass].DeSpawn(deadEnemy);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<EnemyDeath>(OnEnemyDeath);
            foreach (AddressableGameObjectPool<EnemyEntity> pool in _enemyEntityPoolMap.Values)
            {
                pool.ClearObjectReferences();
            }
            _enemyEntityPoolMap.Clear();
            _enemyEntityPoolMap = null;
            _enemyEntityLibrary = null;
            _enemyEntityList.Clear();
            _enemyEntityList = null;
        }
    }
}