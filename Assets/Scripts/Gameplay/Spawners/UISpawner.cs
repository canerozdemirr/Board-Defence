using System.Collections.Generic;
using System.Linq;
using Datas.Configs.UI_Configs;
using UI;
using UI.Interfaces;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class UISpawner : MonoBehaviour, IUISpawner, IInitializable
    {
        private Dictionary<string, GameObjectPool<BaseUIElement>> _uiElementPoolMap;
        private UIElementLibrary _uiElementLibrary;
        private List<IUIElement> _activeUIElements;

        [Inject] private DiContainer _container;

        public void SetUIElementLibrary(UIElementLibrary uiElementLibrary)
        {
            _uiElementLibrary = uiElementLibrary;
        }

        public void Initialize()
        {
            _uiElementPoolMap = new Dictionary<string, GameObjectPool<BaseUIElement>>();
            _activeUIElements = new List<IUIElement>();
        }

        public T ProvideUIElement<T>(string elementName) where T : IUIElement
        {
            BaseUIElement spawnedUIElement;

            if (_uiElementPoolMap.ContainsKey(elementName))
            {
                spawnedUIElement = _uiElementPoolMap[elementName].Spawn();
                _container.InjectGameObject(spawnedUIElement.gameObject);
                spawnedUIElement.Initialize();

                _activeUIElements.Add(spawnedUIElement);
                return (T)(IUIElement)spawnedUIElement;
            }

            UIElementConfig config = FindUIElementConfig(elementName);
            GameObjectPool<BaseUIElement> uiPool = GameObjectPool<BaseUIElement>.Create(config.Prefab, transform, defaultSize: 1, maxSize: 2, dontDestroyOnLoad: true);

            _uiElementPoolMap.Add(elementName, uiPool);
            spawnedUIElement = _uiElementPoolMap[elementName].Spawn();
            _container.InjectGameObject(spawnedUIElement.gameObject);
            spawnedUIElement.Initialize();
            _activeUIElements.Add(spawnedUIElement);

            return (T)(IUIElement)spawnedUIElement;
        }

        public void ReturnUIElementToPool(IUIElement uiElement)
        {
            BaseUIElement baseElement = uiElement as BaseUIElement;
            if (baseElement == null)
                return;
            
            string elementName = baseElement.ElementName;
            if (!_uiElementPoolMap.TryGetValue(elementName, out GameObjectPool<BaseUIElement> pool))
                return;
                
            _activeUIElements.Remove(uiElement);
            uiElement.Hide();
            pool.DeSpawn(baseElement);
        }
        
        private UIElementConfig FindUIElementConfig(string elementName)
        {
            return _uiElementLibrary.UIElementConfigs.FirstOrDefault(config => config.ElementName == elementName);
        }

        private void OnDestroy()
        {
            foreach (GameObjectPool<BaseUIElement> pool in _uiElementPoolMap.Values)
            {
                pool.ClearObjectReferences();
            }
            _uiElementPoolMap.Clear();
            _uiElementPoolMap = null;
            _uiElementLibrary = null;
            _activeUIElements.Clear();
            _activeUIElements = null;
        }
    }
}
