namespace Gameplay.Interfaces
{
    public interface IEntityComponent
    {
        IEntity Owner { get; }
        bool IsEnabled { get; }

        void Initialize(IEntity owner);
        void Enable();
        void Disable();
    }
}