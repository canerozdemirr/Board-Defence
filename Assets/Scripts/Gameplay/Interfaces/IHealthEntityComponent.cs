using System;

namespace Gameplay.Interfaces
{
    public interface IHealthEntityComponent : IEnemyEntityComponent
    {
        event Action<float> HealthChanged;
        event Action EntityDeath;
    }
}