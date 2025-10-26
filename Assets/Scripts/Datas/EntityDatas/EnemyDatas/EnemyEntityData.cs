using System;
using UnityEngine;

namespace Datas.EntityDatas.EnemyDatas
{
    [Serializable]
    public struct EnemyEntityData
    {
        [SerializeField] 
        private string _enemyName;
        
        [SerializeField] 
        private float _health;
        
        [SerializeField]
        private float _blockPassPerSecond;

        public string EnemyName => _enemyName;
        public float Health => _health;
        public float BlockPassPerSecond => _blockPassPerSecond;
        
        public EnemyEntityData Clone()
        {
            return new EnemyEntityData
            {
                _enemyName = _enemyName,
                _health = _health,
                _blockPassPerSecond = _blockPassPerSecond
            };
        }
    }
}