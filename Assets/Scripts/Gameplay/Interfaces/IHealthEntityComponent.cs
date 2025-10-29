using System;

namespace Gameplay.Interfaces
{
    public interface IHealthEntityComponent : IEntityComponent
    {
        float CurrentHealthAmount { get; }
        void TakeDamage(float damage);
        event Action<float> HealthChanged;
        event Action EntityDeath;
    }
}