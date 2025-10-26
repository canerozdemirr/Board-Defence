using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datas;
using Datas.Configs.Entity_Configs.Enemy_Configs;
using Datas.EntityDatas.EnemyDatas;
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
        
        public void SetEnemyEntityLibrary(EnemyEntityLibrary enemyEntityLibrary)
        {
            _enemyEntityLibrary = enemyEntityLibrary;
        }
        
        public void Initialize()
        {
            _enemyEntityPoolMap = new Dictionary<EnemyClass, AddressableGameObjectPool<EnemyEntity>>();
        }
        
        public async UniTask<EnemyEntity> ProvideEnemyEntity(EnemyClass enemyClass)
        {
            if (_enemyEntityPoolMap.ContainsKey(enemyClass)) 
                return _enemyEntityPoolMap[enemyClass].Spawn();

            AddressableGameObjectPool<EnemyEntity> enemyPool = null;
            foreach (EnemyEntityConfig enemyEntityConfig in _enemyEntityLibrary.EnemyEntityConfigList)
            {
                if (enemyEntityConfig.EnemyEntityData.EnemyClass != enemyClass) 
                    continue;
                
                enemyPool = await AddressableGameObjectPool<EnemyEntity>.CreateAsync(enemyEntityConfig.EntityVisualData.AssetReference, transform);
                break;
            }
            _enemyEntityPoolMap.Add(enemyClass, enemyPool);
            
            return _enemyEntityPoolMap[enemyClass].Spawn();
        }
    }
}