using System;
using System.Collections.Generic;
using Datas.WaveDatas;
using Events.Wave;
using Systems.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace UI.Elements
{
    public class ItemSelectionPanel : BaseUIElement
    {
        [Header("References")]
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private ItemSelectionButton _itemSelectionButtonPrefab;
        [SerializeField] private UnityEngine.UI.Button _startWaveButton;

        [Inject] private IInventorySystem _inventorySystem;
        [Inject] private IItemPlacementSystem _itemPlacementSystem;

        private Dictionary<string, ItemSelectionButton> _itemButtonList;

        public event Action<string> ItemSelected;

        protected override void OnInitialize()
        {
            _itemButtonList = new Dictionary<string, ItemSelectionButton>();
            _startWaveButton.onClick.AddListener(OnStartWaveClicked);
        }

        protected override void OnShow()
        {
            _inventorySystem.InventoryUpdated += OnInventoryUpdated;
        }

        protected override void OnHide()
        {
            _inventorySystem.InventoryUpdated -= OnInventoryUpdated;
        }

        private void OnStartWaveClicked()
        {
            EventBus.Publish(new StartWaveRequested());
            Hide();
        }

        public void SetupButtons(InventoryData inventoryData)
        {
            ClearButtons();

            foreach (InventoryEntityData item in inventoryData.AvailableEntityList)
            {
                CreateButton(item.ItemName, item.ItemAmount);
            }
        }

        private void CreateButton(string itemName, int amount)
        { 
            ItemSelectionButton towerButton = Instantiate(_itemSelectionButtonPrefab, _buttonContainer);
            towerButton.Initialize(itemName, amount, OnTowerButtonClicked);
            _itemButtonList[itemName] = towerButton;
        }

        private void OnTowerButtonClicked(string itemName)
        {
            ItemSelected?.Invoke(itemName);
            _itemPlacementSystem.StartPlacementMode(itemName);
        }

        private void OnInventoryUpdated(string itemName, int amount)
        {
            _itemButtonList[itemName].UpdateAmount(amount);
        }

        private void ClearButtons()
        {
            foreach (ItemSelectionButton button in _itemButtonList.Values)
            {
                Destroy(button.gameObject);
            }
            _itemButtonList.Clear();
        }

        protected override void OnCleanup()
        {
            ClearButtons();
            _startWaveButton.onClick.RemoveListener(OnStartWaveClicked);
        }
    }
}
