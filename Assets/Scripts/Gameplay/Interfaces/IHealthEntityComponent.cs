using System;

namespace Gameplay.Interfaces
{
    public interface IHealthEntityComponent : IEntityComponent
    {
        float CurrentHealthAmount { get; }
        event Action<float> HealthChanged;
        event Action EntityDeath;
    }
}