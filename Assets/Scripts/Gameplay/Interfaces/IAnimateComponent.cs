using Cysharp.Threading.Tasks;

namespace Gameplay.Interfaces
{
    public interface IAnimateComponent : IEntityComponent
    {
        void PlayAnimation(string animationKey);
        UniTask PlayAnimationAsync(string animationKey);
    }
}
