using Cysharp.Threading.Tasks;
using UI;

namespace Systems.Interfaces
{
    public interface IUISystem
    {
        T ShowUIElement<T>(string elementName) where T : IUIElement;
        void HideUIElement(string elementName);
    }
}
