using System;
using Cysharp.Threading.Tasks;
using Gameplay.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities
{
    public class AddressableGameObjectPool<T> : GameObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly string _addressKey;
        private AsyncOperationHandle<GameObject> _prefabHandle;
        private bool _isInitialized;
        
        private AddressableGameObjectPool(string addressKey, Transform poolParent = null, int defaultSize = 1, int maxSize = 100, bool dontDestroyOnLoad = false) : base(null, poolParent, defaultSize, maxSize, dontDestroyOnLoad)
        {
            _addressKey = addressKey;
            _isInitialized = false;
        }
        
        public static async UniTask<AddressableGameObjectPool<T>> CreateAsync(AssetReference assetReference, Transform parent = null, int defaultSize = 10, int maxSize = 100, bool dontDestroyOnLoad = false)
        {
            AddressableGameObjectPool<T> pool = new(assetReference.AssetGUID, parent, defaultSize, maxSize, dontDestroyOnLoad);
            
            await pool.InitializeAsync();
            return pool;
        }
        
        private async UniTask InitializeAsync()
        {
            try
            {
                _prefabHandle = Addressables.LoadAssetAsync<GameObject>(_addressKey);
                _prefab = await _prefabHandle.ToUniTask();
                if (_prefabHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Pool initialize failed with exception: {ex.Message}");
                throw;
            }
        }
        
        public void Release()
        {
            ClearObjectReferences();
            Addressables.Release(_prefabHandle);
            _isInitialized = false;
            _prefab = null;
        }
    }
}