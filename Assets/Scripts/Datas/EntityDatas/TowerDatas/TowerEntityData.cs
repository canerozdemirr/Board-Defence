using System;
using NaughtyAttributes;
using UnityEngine;

namespace Datas.EntityDatas.TowerDatas
{
    [Serializable]
    public struct TowerEntityData
    {
        private string _towerID;

        [SerializeField] private string _itemName;

        [SerializeField] [Min(0f)] private float _damage;

        [SerializeField] [Min(0f)] private float _range;

        [SerializeField] [Min(0f)] private float _attackInterval;

        [SerializeField] private Direction _detectionDirections;

        public string TowerID => _towerID;
        public string ItemName => _itemName;
        public float Damage => _damage;
        public float Range => _range;
        public float AttackInterval => _attackInterval;
        public Direction DetectionDirections => _detectionDirections;

        public void SetID(string id)
        {
            _towerID = id;
        }

        public TowerEntityData Clone()
        {
            return new TowerEntityData
            {
                _towerID = _towerID,
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