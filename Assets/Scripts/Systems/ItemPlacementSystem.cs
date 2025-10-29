using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using Events.Wave;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities.Item_Entities;
using Systems.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class ItemPlacementSystem : IItemPlacementSystem, IInitializable, IDisposable
    {
        [Inject] private IBoardSystem _boardSystem;
        [Inject] private ILevelSystem _levelSystem;
        [Inject] private ITowerSpawner _towerSpawner;
        [Inject] private IInventorySystem _inventorySystem;

        private string _selectedItemName;
        private bool _isInPlacementMode;
        private List<TowerEntity> _activePlacedItems = new();

        public bool IsInPlacementMode => _isInPlacementMode;

        public event Action PlacementStarted;
        public event Action PlacementEnded;

        public void Initialize()
        {
            _isInPlacementMode = false;
            EventBus.Subscribe<StartWaveRequested>(OnWaveStarted);
            _levelSystem.LevelCompleted += OnLevelCompleted;
        }

        private void OnLevelCompleted(int levelIndex)
        {
            foreach (TowerEntity placedItem in _activePlacedItems)
            {
                _boardSystem.FreeBlock(placedItem.BoardIndex);
                _boardSystem.RemoveEntityAtBlock(placedItem.BoardIndex, placedItem);
            }
            _activePlacedItems.Clear();
        }

        private void OnWaveStarted(StartWaveRequested waveStarted)
        {
            CancelPlacementMode();
        }

        public void StartPlacementMode(string itemName)
        {
            _selectedItemName = itemName;
            _isInPlacementMode = true;
            EventBus.Subscribe<BlockClicked>(OnMouseClicked);
            PlacementStarted?.Invoke();
        }

        public void CancelPlacementMode()
        {
            _isInPlacementMode = false;
            _selectedItemName = null;
            EventBus.Unsubscribe<BlockClicked>(OnMouseClicked);
            PlacementEnded?.Invoke();
        }

        private void OnMouseClicked(BlockClicked clickEvent)
        {
            if (!_isInPlacementMode)
                return;
            
            if (!_inventorySystem.HasItem(_selectedItemName))
                return;
            
            Vector2Int blockIndex = clickEvent.BlockIndex;
            if (!_boardSystem.IsValidPlacementPosition(blockIndex))
                return;

            PlaceTowerAsync(blockIndex, _selectedItemName).Forget();
        }

        private async UniTask PlaceTowerAsync(Vector2Int blockIndex, string itemName)
        {
            TowerEntity tower = await _towerSpawner.ProvideTowerEntity(itemName);
            tower.SetBoardIndex(blockIndex);

            Vector3 worldPosition = _boardSystem.GetWorldPositionFromBlock(blockIndex) + Vector3.up * 1.15f;
            tower.SetWorldPosition(worldPosition);

            tower.Initialize();
            tower.OnActivate();

            _boardSystem.OccupyBlock(blockIndex);
            _boardSystem.AddEntityAtBlock(blockIndex, tower);
            _inventorySystem.TryToSpendItem(itemName);

            if (!_inventorySystem.HasItem(itemName))
            {
                CancelPlacementMode();
            }
            
            _activePlacedItems.Add(tower);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<StartWaveRequested>(OnWaveStarted);
            if (_isInPlacementMode)
            {
                CancelPlacementMode();
            }
            _levelSystem.LevelCompleted -= OnLevelCompleted;
        }
    }
}