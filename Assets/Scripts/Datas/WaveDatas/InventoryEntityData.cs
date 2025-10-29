using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Utilities;

namespace Datas.WaveDatas
{
    [Serializable]
    public struct InventoryEntityData
    {
        [SerializeField] private string _itemName;

        [SerializeField] private int _itemAmount;
        public string ItemName => _itemName;
        public int ItemAmount => _itemAmount;

        public InventoryEntityData Clone()
        {
            return new InventoryEntityData
            {
                _itemName = _itemName,
                _itemAmount = _itemAmount
            };
        }

    }
}