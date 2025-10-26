namespace Gameplay.Interfaces
{
    public interface IPoolable
    {
        void OnCalledFromPool();
        void OnReturnToPool();
    }
}