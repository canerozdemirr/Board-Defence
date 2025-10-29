using System;
using Gameplay.Interfaces;
using Utilities;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyIdleState : BaseState<IEnemyEntity>
    {
        private bool _animationPlayed;
        private IAnimateComponent _animateComponent;

        public override void OnEnter(IEnemyEntity context)
        {
            _animationPlayed = false;
            _animateComponent = context.RequestEntityComponent<IAnimateComponent>();
            _animateComponent.PlayAnimationAsync(Constants.PlacementAnimationTag, () => _animationPlayed = true);
        }

        public override void OnUpdate(IEnemyEntity context)
        {
            if (_animationPlayed)
            {
                FinishState();
            }
        }
    }
}
