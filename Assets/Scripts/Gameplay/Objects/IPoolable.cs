namespace Gameplay.Objects
{
    public interface IPoolable
    {
        void OnCalledFromPool();
        void OnReturnToPool();
    }
}