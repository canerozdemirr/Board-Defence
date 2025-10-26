using System;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField] private WaveData _waveData;
        
        //TODO: Add player allowance data here.
        public WaveData WaveData => _waveData;
    }
}