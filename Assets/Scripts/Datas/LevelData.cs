using System;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private List<WaveData> _waveDataList;
        [SerializeField] private float _spawnIntervalBetweenEnemies;
        [SerializeField] private float _spawnWaitTimeBeforeWave;

    //TODO: Add player allowance data here.
        public List<WaveData> WaveDataList => _waveDataList;
        public float SpawnIntervalBetweenEnemies => _spawnIntervalBetweenEnemies;
        public float SpawnWaitTimeBeforeWave => _spawnWaitTimeBeforeWave;
    }
}