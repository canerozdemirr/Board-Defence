using System;
using NaughtyAttributes;
using UnityEngine;

namespace Datas.EntityDatas.EnemyDatas
{
    [Serializable]
    public struct EnemyEntityData
    {
        [SerializeField] private string _enemyID;

        [SerializeField] private string _enemyName;

        [SerializeField] private float _health;

        [SerializeField] private float _blockPassPerSecond;

        [SerializeField] private EnemyClass _enemyClass;

        public string EnemyID => _enemyID;
        public string EnemyName => _enemyName;
        public float Health => _health;
        public float BlockPassPerSecond => _blockPassPerSecond;
        public EnemyClass EnemyClass => _enemyClass;

        public void SetID(string id)
        {
            _enemyID = id;
        }

        public EnemyEntityData Clone()
        {
            return new EnemyEntityData
            {
                _enemyID = _enemyID,
                _enemyName = _enemyName,
                _health = _health,
                _blockPassPerSecond = _blockPassPerSecond,
                _enemyClass = _enemyClass
            };
        }
    }

    public enum EnemyClass
    {
        Light,
        Medium,
        Heavy
    }
}