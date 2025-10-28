using System;
using Events.Enemies;
using Events.Interfaces;
using Gameplay.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = Utilities.EventBus;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyDeathState : BaseState<IEnemyEntity>
    {
        private IHealthEntityComponent _healthEntityComponent;
        private bool _eventProvoked;

        public override void OnEnter(IEnemyEntity context)
        {
            _eventProvoked = false;
            _healthEntityComponent = context.RequestEntityComponent<IHealthEntityComponent>();
        }

        public override void OnUpdate(IEnemyEntity context)
        {
            if (_eventProvoked)
                return;
            
            // TODO: Add death animation and effects here before invoking death callback.
            _eventProvoked = true;
            EventBus.Publish(new EnemyDeath(context));
        }
    }
}