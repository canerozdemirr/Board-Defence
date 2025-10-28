using System;
using Gameplay.Interfaces;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyIdleState : BaseState<IEnemyEntity>
    {
        private float _idleTime;
        private readonly float _idleDuration = 2f;

        public override void OnEnter(IEnemyEntity context)
        {
            _idleTime = 0f;
        }

        public override void OnUpdate(IEnemyEntity context)
        {
            _idleTime += UnityEngine.Time.deltaTime;

            if (_idleTime >= _idleDuration)
            {
                FinishState();
            }
        }
    }
}
