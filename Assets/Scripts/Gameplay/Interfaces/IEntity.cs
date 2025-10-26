namespace Gameplay.Interfaces
{
    public interface IEntity
    {
        void Initialize();
        void OnActivate();
        void OnDeactivate();
    }
}