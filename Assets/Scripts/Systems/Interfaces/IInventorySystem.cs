using System;
using System.Collections.Generic;
using Datas.WaveDatas;

namespace Systems.Interfaces
{
    public interface IInventorySystem
    {
        void UpdateInventory(InventoryData inventoryData);
        bool HasItem(string itemName, int amount = 1);
        bool TryToSpendItem(string itemName, int amount = 1);
        void ReturnItem(string itemName, int amount = 1);
        int GetItemAmount(string itemName);
        event Action<string, int> InventoryUpdated;
    }
}
