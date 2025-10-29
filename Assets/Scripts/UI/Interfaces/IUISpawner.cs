using Cysharp.Threading.Tasks;

namespace UI.Interfaces
{
    public interface IUISpawner
    {
        T ProvideUIElement<T>(string elementName) where T : IUIElement;
        void ReturnUIElementToPool(IUIElement uiElement);
    }
}
