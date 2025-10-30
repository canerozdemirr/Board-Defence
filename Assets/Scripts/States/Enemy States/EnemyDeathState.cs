using System;
using Cysharp.Threading.Tasks;
using Events.Enemies;
using Events.Interfaces;
using Gameplay.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using EventBus = Utilities.EventBus;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyDeathState : BaseState<IEnemyEntity>
    {
        private IAnimateComponent _animateComponent;

        public override void OnEnter(IEnemyEntity context)
        {
            context.RequestEntityComponent<IHealthEntityComponent>();
            _animateComponent = context.RequestEntityComponent<IAnimateComponent>();
            PlayDeathAnimation(context).Forget();
        }

        private async UniTask PlayDeathAnimation(IEnemyEntity context)
        {
            if (_animateComponent != null)
            {
                await _animateComponent.PlayAnimationAsync(Constants.EnemyDeathAnimationTag);
            }

            EventBus.Publish(new EnemyDeath(context));
            FinishState();
        }
    }
}