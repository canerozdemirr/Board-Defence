using System;
using System.Linq;
using Datas;
using Datas.Configs.Level_Configs;
using Events.Enemies;
using Events.Wave;
using Gameplay.Interfaces;
using Systems.Interfaces;
using UI.Elements;
using UnityEngine;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class LevelSystem : ILevelSystem, IInitializable, IDisposable
    {
        private readonly LevelConfig _levelConfig;
        private readonly IWaveSystem _waveSystem;
        private readonly IInventorySystem _inventorySystem;
        private readonly ITowerSpawner _towerSpawner;
        private readonly IProjectileSpawner _projectileSpawner;
        private readonly IUISystem _uiSystem;

        private int _currentLevelIndex;
        private LevelData _currentLevelData;
        private LevelState _levelState;
        private int _totalEnemiesInWave;
        private int _enemiesDefeated;

        public event Action<int> LevelStarted;
        public event Action<int> LevelCompleted;
        public event Action<LevelData> LevelDataLoaded;
        public event Action AllLevelsCompleted;

        public LevelSystem(LevelConfig levelConfig, IWaveSystem waveSystem, IInventorySystem inventorySystem,
            ITowerSpawner towerSpawner, IProjectileSpawner projectileSpawner, IUISystem uiSystem)
        {
            _levelConfig = levelConfig;
            _waveSystem = waveSystem;
            _inventorySystem = inventorySystem;
            _towerSpawner = towerSpawner;
            _projectileSpawner = projectileSpawner;
            _uiSystem = uiSystem;
        }

        public void Initialize()
        {
            _currentLevelIndex = 0;
            _currentLevelData = _levelConfig.LevelDataList[_currentLevelIndex];
            _levelState = LevelState.WaitingTowerPlacement;

            _inventorySystem.UpdateInventory(_currentLevelData.InventoryData.Clone());

            LevelDataLoaded?.Invoke(_currentLevelData);

            _waveSystem.EnemyWaveStarted += OnWaveStarted;
            EventBus.Subscribe<StartWaveRequested>(OnStartWaveRequested);
            EventBus.Subscribe<EnemyDeath>(OnEnemyDeath);

            _waveSystem.RegisterWaveData(_currentLevelData.WaveDataList, _currentLevelData.SpawnIntervalBetweenEnemies, _currentLevelData.SpawnWaitTimeBeforeWave);
        }

        private void OnWaveStarted(int totalEnemies)
        {
            _totalEnemiesInWave = totalEnemies;
            _enemiesDefeated = 0;
            _levelState = LevelState.BattleInProgress;
            LevelUIPanel panel = _uiSystem.ShowUIElement<LevelUIPanel>(Constants.LevelUIPanelTag);
            panel.RegisterEnemyCount(_totalEnemiesInWave);
            panel.RegisterLevelCount(_currentLevelIndex + 1,_levelConfig.LevelDataList.Length);
            StartNewLevel();
        }

        private void OnEnemyDeath(EnemyDeath enemyDeath)
        {
            _enemiesDefeated++;

            if (_enemiesDefeated >= _totalEnemiesInWave)
            {
                OnLevelCompleted();
            }
        }

        private void OnStartWaveRequested(StartWaveRequested waveEvent)
        {
            _waveSystem.StartNextWave();
        }

        private void OnLevelCompleted()
        {
            _levelState = LevelState.Succeeded;
            LevelCompleted?.Invoke(_currentLevelIndex);
            CleanupLevel();

            _currentLevelIndex++;

            if (_currentLevelIndex < _levelConfig.LevelDataList.Length)
            {
                LoadNextLevel();
            }
            else
            {
                AllLevelsCompleted?.Invoke();
            }
        }

        private void CleanupLevel()
        {
            _projectileSpawner.ReturnAllProjectilesToPool();
            _towerSpawner.ReturnAllTowersToPool();
        }

        private void LoadNextLevel()
        {
            _currentLevelData = _levelConfig.LevelDataList[_currentLevelIndex];
            _levelState = LevelState.WaitingTowerPlacement;

            _inventorySystem.UpdateInventory(_currentLevelData.InventoryData.Clone());

            LevelDataLoaded?.Invoke(_currentLevelData);

            _waveSystem.RegisterWaveData(_currentLevelData.WaveDataList, _currentLevelData.SpawnIntervalBetweenEnemies, _currentLevelData.SpawnWaitTimeBeforeWave);
        }

        private void StartNewLevel()
        {
            LevelStarted?.Invoke(_currentLevelIndex + 1);
        }

        public void Dispose()
        {
            _waveSystem.EnemyWaveStarted -= OnWaveStarted;
            EventBus.Unsubscribe<StartWaveRequested>(OnStartWaveRequested);
            EventBus.Unsubscribe<EnemyDeath>(OnEnemyDeath);
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