using System;
using Datas.EntityDatas.EnemyDatas;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct EnemySpawnData
    {
        [SerializeField] private EnemyClass _enemyClass;
        [SerializeField] private int _count;
        
        public EnemyClass EnemyData => _enemyClass;
        public int Count => _count;
    }
}