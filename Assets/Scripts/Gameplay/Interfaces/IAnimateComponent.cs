namespace Gameplay.Interfaces
{
    public interface IAnimateComponent : IEntityComponent
    {
        void PlayAnimation(string animationKey);
    }
}
