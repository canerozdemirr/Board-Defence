using Datas.EntityDatas.TowerDatas;
using Events.Wave;
using Gameplay.Interfaces;
using States;
using States.Tower_States;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Objects.Entities.Item_Entities
{
    public class TowerEntity : BaseBlockEntity, ITowerEntity
    {
        [Inject] private DiContainer _container;

        private TowerEntityData _towerEntityData;
        private StateMachine<ITowerEntity> _stateMachine;

        private TowerScanningState _scanningState;
        private TowerAttackingState _attackingState;

        public TowerEntityData TowerEntityData => _towerEntityData;

        public override void Initialize()
        {
            base.Initialize();
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine<ITowerEntity>(this);

            _scanningState = new TowerScanningState();
            _attackingState = new TowerAttackingState();

            _container.Inject(_scanningState);
            _container.Inject(_attackingState);

            _stateMachine.AddTransition(_scanningState, _attackingState, _ => _scanningState.DetectedEnemy != null, SetTargetEnemy);
            _stateMachine.AddTransition(_attackingState, _scanningState, _ => _attackingState.TargetEnemy == null);
        }

        private void SetTargetEnemy(ITowerEntity towerEntity)
        {
            _attackingState.SetTarget(_scanningState.DetectedEnemy);
        }

        public override void OnActivate()
        {
            base.OnActivate();
            EventBus.Subscribe<StartWaveRequested>(OnWaveStarted);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            EventBus.Unsubscribe<StartWaveRequested>(OnWaveStarted);
            if (_stateMachine.IsActive)
            {
                _stateMachine.Stop();
            }
        }

        private void OnWaveStarted(StartWaveRequested waveStarted)
        {
            _stateMachine.Start(_scanningState);
        }

        private void Update()
        {
            if (_stateMachine is { IsActive: true, IsPaused: false })
                _stateMachine.Update();
        }

        public void AssignTowerData(TowerEntityData towerEntityData)
        {
            _towerEntityData = towerEntityData.Clone();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<StartWaveRequested>(OnWaveStarted);
            _stateMachine.Clear();
            _stateMachine = null;
            _scanningState = null;
            _attackingState = null;
        }
    }
}