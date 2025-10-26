using System;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using UnityEngine;

namespace States.Enemy_States
{
    [Serializable]
    public class EnemyMovementState : BaseState<EnemyEntity>
    {
        private const int TARGET_COLUMN = 0; 
        
        public override void OnEnter(EnemyEntity context)
        {
            base.OnEnter(context);
        }
        
        public override void OnUpdate(EnemyEntity context)
        {
            base.OnUpdate(context);
        }
        
        public override void OnExit(EnemyEntity context)
        {
            base.OnExit(context);
        }
    }
}