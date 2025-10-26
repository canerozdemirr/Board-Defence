using System;
using System.Linq;
using Datas;
using Datas.Configs.Level_Configs;
using Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    [Serializable]
    public class LevelSystem : IInitializable, IDisposable
    {
        private readonly LevelConfig _levelConfig;
        private readonly IWaveSystem _waveSystem;

        private int _currentLevelIndex;
        private LevelData _currentLevelData;

        public LevelSystem(LevelConfig levelConfig, IWaveSystem waveSystem)
        {
            _levelConfig = levelConfig;
            _waveSystem = waveSystem;
        }

        public void Initialize()
        {
            _currentLevelIndex = 0;
            _currentLevelData = _levelConfig.LevelDataList[_currentLevelIndex];
            
            _waveSystem.WaveCompleted += OnWaveCompleted;
            
            _waveSystem.RegisterWaveData(_currentLevelData.WaveDataList, _currentLevelData.SpawnIntervalBetweenEnemies, _currentLevelData.SpawnWaitTimeBeforeWave);
            _waveSystem.StartNextWave();
            //TODO: Allow player to pick towers and assign necessary datas.
        }

        private void OnWaveCompleted()
        {
            Debug.Log("Wave Completed!");
            _currentLevelIndex++;
        }

        public void Dispose()
        {
            _waveSystem.WaveCompleted -= OnWaveCompleted;
        }
    }
}