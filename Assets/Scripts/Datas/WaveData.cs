using System;
using Datas.WaveDatas;
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