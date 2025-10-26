using System;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct WaveData
    {
        [SerializeField] private EnemySpawnData _enemySpawnData;
        public EnemySpawnData EnemySpawnData => _enemySpawnData;
    }
}