using System;
using Datas.WaveDatas;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct WaveData
    {
        [SerializeField] private EnemySpawnData _enemySpawnData;
        [SerializeField] private InventoryData _inventoryData;

        public EnemySpawnData EnemySpawnData => _enemySpawnData;
        public InventoryData InventoryData => _inventoryData;
    }
}