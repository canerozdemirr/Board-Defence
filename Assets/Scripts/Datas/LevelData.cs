using System;
using System.Collections.Generic;
using Datas.WaveDatas;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private List<WaveData> _waveDataList;
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private float _spawnIntervalBetweenEnemies;
        [SerializeField] private float _spawnWaitTimeBeforeWave;

        public InventoryData InventoryData => _inventoryData;
        public List<WaveData> WaveDataList => _waveDataList;
        public float SpawnIntervalBetweenEnemies => _spawnIntervalBetweenEnemies;
        public float SpawnWaitTimeBeforeWave => _spawnWaitTimeBeforeWave;
    }
}