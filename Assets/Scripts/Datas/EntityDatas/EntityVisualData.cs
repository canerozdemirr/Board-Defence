using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Datas.EntityDatas
{
    [Serializable]
    public struct EntityVisualData
    {
        [SerializeField] private AssetReference _assetReference;
        
        public AssetReference AssetReference => _assetReference;
    }
}