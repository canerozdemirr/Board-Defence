using System;
using Gameplay.Interfaces;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Entities.Entity_Components
{
    public class HealthComponent : BaseEntityComponent, IHealthEntityComponent
    {
        private float _currentHealthAmount = 1;

        private IEnemyEntity _enemyEntity;

        public float CurrentHealthAmount => _currentHealthAmount;

        public event Action<float> HealthChanged;
        public event Action EntityDeath;

        public override void Initialize(IEntity owner)
        {
            base.Initialize(owner);
            _enemyEntity = owner as IEnemyEntity;
            if (_enemyEntity != null)
            {
                _currentHealthAmount = _enemyEntity.EnemyEntityData.Health;
            }
        }
        
        public void TakeDamage(int damage)
        {
            _currentHealthAmount -= damage;
            HealthChanged?.Invoke(_currentHealthAmount);
            if (!(_currentHealthAmount <= 0)) 
                return;
            
            _currentHealthAmount = 0;
            EntityDeath?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PROJECTILE))
            {
                //TODO: Get damage amount from projectile component. Fetch the damage value from Projectile Interface.
            }
        }

        
    }
}