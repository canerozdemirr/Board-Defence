using System;
using System.Collections.Generic;
using Datas.WaveDatas;
using Systems.Interfaces;
using UI.Elements;
using Zenject;

namespace Systems
{
    [Serializable]
    public class InventorySystem : IInventorySystem, IInitializable, IDisposable
    {
        [Inject] private IUISystem _uiSystem;

        private Dictionary<string, int> _currentInventory;

        public event Action<string, int> InventoryUpdated;

        public void Initialize()
        {
            _currentInventory = new Dictionary<string, int>();
        }

        public void UpdateInventory(InventoryData inventoryData)
        {
            foreach (InventoryEntityData item in inventoryData.AvailableEntityList)
            {
                _currentInventory[item.ItemName] = item.ItemAmount;
                InventoryUpdated?.Invoke(item.ItemName, item.ItemAmount);
            }

            ItemSelectionPanel panel = _uiSystem.ShowUIElement<ItemSelectionPanel>("ItemSelectionPanel");
            panel.SetupButtons(inventoryData);
        }

        public bool HasItem(string itemName, int amount = 1)
        {
            return _currentInventory.ContainsKey(itemName) && _currentInventory[itemName] >= amount;
        }

        public bool TryToSpendItem(string itemName, int amount = 1)
        {
            if (!HasItem(itemName, amount))
                return false;

            _currentInventory[itemName] -= amount;
            InventoryUpdated?.Invoke(itemName, _currentInventory[itemName]);
            return true;
        }

        public void ReturnItem(string itemName, int amount = 1)
        {
            _currentInventory[itemName] += amount;
            InventoryUpdated?.Invoke(itemName, _currentInventory[itemName]);
        }

        public int GetItemAmount(string itemName)
        {
            return _currentInventory.GetValueOrDefault(itemName, 0);
        }
        public void Dispose()
        {
            _currentInventory.Clear();
        }
    }
}
