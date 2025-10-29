using System;
using System.Collections.Generic;
using Datas;

namespace Systems.Interfaces
{
    public interface IWaveSystem
    {
        void RegisterWaveData(List<WaveData> waveDataList, float spawnIntervalBetweenEnemies, float spawnWaitTimeBeforeWave);
        void StartNextWave();
        event Action EnemyWaveCompleted;
        event Action <int> EnemyWaveStarted;
    }
}