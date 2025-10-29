using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Systems.Interfaces;
using UI;
using UI.Interfaces;
using Zenject;

namespace Systems
{
    [Serializable]
    public class UISystem : IUISystem, IInitializable, IDisposable
    {
        private Dictionary<string, IUIElement> _activeUIElements;

        [Inject] private IUISpawner _uiSpawner;

        public void Initialize()
        {
            _activeUIElements = new Dictionary<string, IUIElement>();
        }

        public T ShowUIElement<T>(string elementName) where T : IUIElement
        {
            if (_activeUIElements.ContainsKey(elementName) && _activeUIElements[elementName].IsVisible)
            {
                return (T)_activeUIElements[elementName];
            }

            IUIElement uiElement = GetUIElement<T>(elementName);

            uiElement.Show();
            _activeUIElements[elementName] = uiElement;

            return (T)uiElement;
        }

        public void HideUIElement(string elementName)
        {
            if (!_activeUIElements.TryGetValue(elementName, out IUIElement element))
                return;

            element.Hide();
            _uiSpawner.ReturnUIElementToPool(element);
            _activeUIElements.Remove(elementName);
        }

        public T GetUIElement<T>(string elementName) where T : IUIElement
        {
            if (_activeUIElements.TryGetValue(elementName, out IUIElement element))
            {
                return (T)element;
            }
            
            T uiElement = _uiSpawner.ProvideUIElement<T>(elementName);
            _activeUIElements[elementName] = uiElement;
            return uiElement;
        }

        private void HideAllUIElements()
        {
            List<string> activeKeys = new(_activeUIElements.Keys);
            foreach (string elementName in activeKeys)
            {
                HideUIElement(elementName);
            }
        }

        public void Dispose()
        {
            HideAllUIElements();
            _activeUIElements.Clear();
        }
    }
}
