using Datas.EntityDatas.EnemyDatas;
using Gameplay.Interfaces;
using States;
using States.Enemy_States;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Entities
{
    public class EnemyEntity : BaseBlockEntity, IEnemyEntity
    {
        private EnemyEntityData _enemyEntityData;
        private StateMachine<IEnemyEntity> _stateMachine;

        private EnemyIdleState _idleState;
        private EnemyMoveState _moveState;
        private EnemyDeathState _deathState;
        
        private IHealthEntityComponent _healthComponent;
        private IMovementEntityComponent _movementEntityComponent;
        private IAnimateComponent _animateComponent;

        public EnemyEntityData EnemyEntityData => _enemyEntityData;
        public bool IsAlive => _healthComponent != null && _healthComponent.CurrentHealthAmount > 0;
        
        public override void Initialize()
        {
            transform.localScale = Vector3.one;
            base.Initialize();
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Initialize(this);
            }
            _animateComponent = RequestEntityComponent<IAnimateComponent>();
            _healthComponent = RequestEntityComponent<IHealthEntityComponent>();
            _movementEntityComponent = RequestEntityComponent<IMovementEntityComponent>();
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine<IEnemyEntity>(this);

            // We can write custom editor scripts to expose states and transitions in the editor, making it more flexible to design and handle. For now, we are hardcoding them.
            _idleState = new EnemyIdleState();
            _moveState = new EnemyMoveState();
            _deathState = new EnemyDeathState();

            _stateMachine.AddTransition(_idleState, _moveState, _ => true);
            _stateMachine.AddTransition(_moveState, _deathState, _ => true);
            _stateMachine.AddAnyStateTransition(_deathState, context =>
            {
                _healthComponent = RequestEntityComponent<IHealthEntityComponent>();
                return _healthComponent is { CurrentHealthAmount: <= 0 };
            });
        }

        public override void OnActivate()
        {
            base.OnActivate();
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Enable();
            }

            _healthComponent.EntityDeath += OnEntityDeath;
            _movementEntityComponent.ReachToEndBlock += OnReachToEndBlock;
            _stateMachine.Start(_idleState);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Disable();
            }

            _healthComponent.EntityDeath -= OnEntityDeath;
            _movementEntityComponent.ReachToEndBlock -= OnReachToEndBlock;
            _stateMachine.Stop();
        }

        private void Update()
        {
            if (_stateMachine is { IsActive: true, IsPaused: false })
                _stateMachine.Update();
        }
        
        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
            _animateComponent.PlayAnimation(Constants.EnemyDamageAnimationTag);
        }

        private void OnEntityDeath()
        {
            _stateMachine.TryTransitioningToState(_deathState);
        }
        
        private void OnReachToEndBlock()
        {
            _stateMachine.TryTransitioningToState(_deathState);
        }

        public void AssignEnemyEntityData(EnemyEntityData enemyEntityData)
        {
            _enemyEntityData = enemyEntityData;
        }

        private void OnDestroy()
        {
            _healthComponent.EntityDeath -= OnEntityDeath;
            _stateMachine.Clear();
            _stateMachine = null;
            _idleState = null;
            _moveState = null;
            _deathState = null;
            _healthComponent = null;
        }
    }
}