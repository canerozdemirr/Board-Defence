using System;
using Gameplay.Objects.Entities;
using States.Interfaces;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyMovementPrepareState : BaseState<EnemyEntity>
    {
        private float _time = 1f;
        public override void OnEnter(EnemyEntity context)
        {
            base.OnEnter(context);
        }

        public override void OnUpdate(EnemyEntity context)
        {
            base.OnUpdate(context);
            _time -= UnityEngine.Time.deltaTime;
            if (!(_time <= 0f))
                return;
            
            FinishState();
        }

        public override void OnExit(EnemyEntity context)
        {
            base.OnExit(context);
        }
    }
}