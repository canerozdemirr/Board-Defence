using System;

namespace Gameplay.Interfaces
{
    public interface IHealthEntityComponent : IEntityComponent
    {
        event Action<float> HealthChanged;
        event Action EntityDeath;
    }
}