namespace UI
{
    public interface IUIElement
    {
        void Show();
        void Hide();
        bool IsVisible { get; }
        void Initialize();
    }
}