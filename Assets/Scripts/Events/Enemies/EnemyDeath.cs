using System;
using Events.Interfaces;
using Gameplay.Interfaces;

namespace Events.Enemies
{
    [Serializable]
    public struct EnemyDeath : IEvent
    {
        public readonly IEnemyEntity EnemyEntity;
        public EnemyDeath(IEnemyEntity enemyEntity)
        {
            EnemyEntity = enemyEntity;
        }
    }
}