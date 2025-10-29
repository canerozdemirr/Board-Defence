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
    public class LevelSystem : ILevelSystem, IInitializable, IDisposable
    {
        private readonly LevelConfig _levelConfig;
        private readonly IWaveSystem _waveSystem;
        private readonly IInventorySystem _inventorySystem;

        private int _currentLevelIndex;
        private LevelData _currentLevelData;
        private LevelState _levelState;

        public event Action<int> LevelStarted;
        public event Action<int> LevelCompleted;
        public event Action<LevelData> LevelDataLoaded;

        public LevelSystem(LevelConfig levelConfig, IWaveSystem waveSystem, IInventorySystem inventorySystem)
        {
            _levelConfig = levelConfig;
            _waveSystem = waveSystem;
            _inventorySystem = inventorySystem;
        }

        public void Initialize()
        {
            _currentLevelIndex = 1;
            _currentLevelData = _levelConfig.LevelDataList[_currentLevelIndex];
            _levelState = LevelState.WaitingTowerPlacement;

            _inventorySystem.UpdateInventory(_currentLevelData.InventoryData);

            LevelDataLoaded?.Invoke(_currentLevelData);

            _waveSystem.EnemyWaveCompleted += OnWaveCompleted;

            _waveSystem.RegisterWaveData(_currentLevelData.WaveDataList, _currentLevelData.SpawnIntervalBetweenEnemies, _currentLevelData.SpawnWaitTimeBeforeWave);
            _waveSystem.StartNextWave();
        }

        private void OnWaveCompleted()
        {
            LevelCompleted?.Invoke(_currentLevelIndex);
            _currentLevelIndex++;
        }

        private void StartNewLevel()
        {
            LevelStarted?.Invoke(_currentLevelIndex);
        }

        public void Dispose()
        {
            _waveSystem.EnemyWaveCompleted -= OnWaveCompleted;
        }
    }
    
    public enum LevelState
    {
        WaitingTowerPlacement,
        BattleInProgress,
        Succeeded,
        Failed
    }
}