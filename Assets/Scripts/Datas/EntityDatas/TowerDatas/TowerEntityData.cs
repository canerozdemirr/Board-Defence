using System;
using UnityEngine;

namespace Datas.ItemDatas
{
    [Serializable]
    public struct TowerEntityData
    {
        [SerializeField] 
        private string _itemName;
        
        [SerializeField] 
        [Min(0f)]
        private float _damage;
        
        [SerializeField]
        [Min(0f)]
        private float _range;

        [SerializeField] 
        [Min(0f)]
        private float _attackInterval;
        
        [SerializeField]
        private Direction _detectionDirections;
        
        public string ItemName => _itemName;
        public float Damage => _damage;
        public float Range => _range;
        public float AttackInterval => _attackInterval;
        public Direction DetectionDirections => _detectionDirections;
        
        public TowerEntityData Clone()
        {
            return new TowerEntityData
            {
                _itemName = _itemName,
                _damage = _damage,
                _range = _range,
                _attackInterval = _attackInterval,
                _detectionDirections = _detectionDirections
            };
        }
    }

    [Flags]
    public enum Direction
    {
        Forward = 1 << 0,
        Backward = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        AllDirections = Forward | Backward | Left | Right
    }
}