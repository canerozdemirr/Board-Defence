using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Datas.ItemDatas
{
    [Serializable]
    public struct TowerEntityVisualData
    {
        [SerializeField] private AssetReference _assetReference;
        
        public AssetReference AssetReference => _assetReference;
    }
}