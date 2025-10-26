using System;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private WaveData[] _waveDataList;
        [SerializeField] private float _spawnIntervalBetweenEnemies;
        [SerializeField] private float _spawnWaitTimeBeforeWave;

    //TODO: Add player allowance data here.
        public WaveData[] WaveData => _waveDataList;
        public float SpawnIntervalBetweenEnemies => _spawnIntervalBetweenEnemies;
        public float SpawnWaitTimeBeforeWave => _spawnWaitTimeBeforeWave;
    }
}