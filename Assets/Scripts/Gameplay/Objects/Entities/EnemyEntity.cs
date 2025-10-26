using System.Collections.Generic;
using Datas.EntityDatas.EnemyDatas;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities.Entity_Components;
using States;
using States.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Objects.Entities
{
    public class EnemyEntity : BaseGridEntity, IEnemyEntity
    {
        [SerializeField] 
        private BaseEntityComponent[] _entityComponentList;
        
        private EnemyEntityData _enemyEntityData;

        private StateMachine<IEnemyEntity> _stateMachine;

        public EnemyEntityData EnemyEntityData => _enemyEntityData;
        
        public override void Initialize()
        {
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Initialize(this);
            }
            _stateMachine = new StateMachine<IEnemyEntity>(this);
            
        }

        public override void OnActivate()
        {
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Enable();
            }
        }

        public override void OnDeactivate()
        {
            foreach (IEntityComponent component in _entityComponentList)
            {
                component.Disable();
            }
        }

        public void AssignEnemyEntityData(EnemyEntityData enemyEntityData)
        {
            _enemyEntityData = enemyEntityData;
        }
        
        public T RequestComponent<T>() where T : class, IEnemyEntityComponent
        {
            foreach (BaseEntityComponent component in _entityComponentList)
            {
                if (component is T requestedComponent)
                {
                    return requestedComponent;
                }
            }
            return null;
        }
        
    }
}