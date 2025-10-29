using System;
using UnityEngine;

namespace Datas.WaveDatas
{
    [Serializable]
    public struct InventoryEntityData
    {
        [SerializeField]
        private string _itemID;

        [SerializeField]
        private int _itemAmount;

        public string ItemID => _itemID;
        public int ItemAmount => _itemAmount;

        public InventoryEntityData Clone()
        {
            return new InventoryEntityData
            {
                _itemID = _itemID,
                _itemAmount = _itemAmount
            };
        }
    }
}