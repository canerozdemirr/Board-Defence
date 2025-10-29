using System;
using Cysharp.Threading.Tasks;
using Events;
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
        [Inject] private ITowerSpawner _towerSpawner;
        [Inject] private IInventorySystem _inventorySystem;

        private string _selectedItemName;
        private bool _isInPlacementMode;

        public bool IsInPlacementMode => _isInPlacementMode;

        public event Action PlacementStarted;
        public event Action PlacementEnded;

        public void Initialize()
        {
            _isInPlacementMode = false;
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
            Vector2Int blockIndex = clickEvent.BlockIndex;

            if (!_inventorySystem.HasItem(_selectedItemName))
                return;

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

        }

        public void Dispose()
        {
            if (_isInPlacementMode)
            {
                CancelPlacementMode();
            }
        }
    }
}
