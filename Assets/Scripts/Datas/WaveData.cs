using System;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct WaveData
    {
        [SerializeField] private EnemySpawnData _enemySpawnData;
        [SerializeField] private float _spawnIntervalBetweenEnemies;
        
        public EnemySpawnData EnemySpawnData => _enemySpawnData;
        public float SpawnIntervalBetweenEnemies => _spawnIntervalBetweenEnemies;
    }
}