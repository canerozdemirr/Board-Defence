using System;
using System.Collections.Generic;
using UnityEngine;

namespace Datas.WaveDatas
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField]
        private List<InventoryEntityData> _availableEntityList;

        public List<InventoryEntityData> AvailableEntityList => _availableEntityList;

        public InventoryData Clone()
        {
            InventoryData clonedInventoryData = new()
            {
                _availableEntityList = new List<InventoryEntityData>()
            };

            foreach (InventoryEntityData item in _availableEntityList)
            {
                clonedInventoryData._availableEntityList.Add(item.Clone());
            }

            return clonedInventoryData;
        }
    }
}