using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Datas;
using Datas.EntityDatas.EnemyDatas;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using Systems.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class WaveSystem : IWaveSystem, IInitializable, IDisposable, ITickable
    {
        private readonly IBoardSystem _boardSystem;
        private readonly IEnemySpawner _enemySpawner;
        
        private List<WaveData> _waveDataList;
        private WaveData _currentWaveData;
        private int _currentEnemyCount;
        private float _spawnIntervalBetweenEnemies;
        private float _spawnWaitTimeBeforeWave;
        private int _enemyMinSpawnColumnNumber;

        private Dictionary<EnemyClass, int> _enemySpawnedCountMap;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action EnemyWaveCompleted;
        public event Action<int> EnemyWaveStarted;

        private WaveSystem(IBoardSystem boardSystem, IEnemySpawner enemySpawner)
        {
            _boardSystem = boardSystem;
            _enemySpawner = enemySpawner;
        }
        
        public void Initialize()
        {
            _enemySpawnedCountMap = new Dictionary<EnemyClass, int>();
            _waveDataList = new List<WaveData>();
        }

        public void RegisterWaveData(List<WaveData> waveDataList, float spawnIntervalBetweenEnemies, float spawnWaitTimeBeforeWave)
        {
            _waveDataList.AddRange(waveDataList);
            _spawnIntervalBetweenEnemies = spawnIntervalBetweenEnemies;
            _spawnWaitTimeBeforeWave = spawnWaitTimeBeforeWave;
            _enemyMinSpawnColumnNumber = _boardSystem.BoardSizeData.ColumnNumber - _boardSystem.BoardSizeData.PlayerColumnLimit;
        }

        public void StartNextWave()
        {
            foreach (WaveData waveData in _waveDataList)
            {
                _currentEnemyCount += waveData.EnemySpawnData.Count;
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _ = SpawnWaveAsync();
        }

        private async UniTask SpawnWaveAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnWaitTimeBeforeWave), cancellationToken: _cancellationTokenSource.Token);
            EnemyWaveStarted?.Invoke(_currentEnemyCount);
            
            while (_currentEnemyCount > 0 && _waveDataList.Count > 0)
            {
                int randomWaveIndex = UnityEngine.Random.Range(0, _waveDataList.Count);
                _currentWaveData = _waveDataList[randomWaveIndex];
                EnemyClass enemyClass = _currentWaveData.EnemySpawnData.EnemyData;
                
                IGridEntity spawnedEnemy = await _enemySpawner.ProvideEnemyEntity(enemyClass);
                Vector2Int spawnBoardIndex = new(UnityEngine.Random.Range(0, _boardSystem.BoardSizeData.RowNumber), UnityEngine.Random.Range(_enemyMinSpawnColumnNumber, _boardSystem.BoardSizeData.ColumnNumber));
                spawnedEnemy.SetBoardIndex(spawnBoardIndex);
                
                Vector3 spawnWorldPosition = _boardSystem.BoardSizeData.CalculateCenteredCellPosition(spawnBoardIndex.x, spawnBoardIndex.y);
                Vector3 finalPosition = new(spawnWorldPosition.x, spawnWorldPosition.y + spawnedEnemy.WorldTransform.localScale.y, spawnWorldPosition.z);
                spawnedEnemy.SetWorldPosition(finalPosition);
                
                spawnedEnemy.Initialize();
                spawnedEnemy.OnActivate();

                if (!_enemySpawnedCountMap.TryAdd(enemyClass, 1))
                {
                    _enemySpawnedCountMap[enemyClass]++;
                }

                if(_enemySpawnedCountMap[enemyClass] >= _currentWaveData.EnemySpawnData.Count)
                {
                    _waveDataList.RemoveAt(randomWaveIndex);
                }
                
                _currentEnemyCount--;
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnIntervalBetweenEnemies), cancellationToken: _cancellationTokenSource.Token);
            }
            
            EnemyWaveCompleted?.Invoke();
        }

        private void OnWaveCompleted()
        {
            //TODO: Do cool stuff when wave is completed
        }

        public void Tick()
        {
            
        }
        
        public void Dispose()
        {
            _waveDataList = null;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _enemySpawnedCountMap.Clear();
            _enemySpawnedCountMap = null;
        }
    }
}