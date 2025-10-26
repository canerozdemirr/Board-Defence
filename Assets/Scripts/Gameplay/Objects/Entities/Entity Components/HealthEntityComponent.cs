using System;
using Gameplay.Interfaces;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Entities.Entity_Components
{
    public class HealthEntityComponent : BaseEntityComponent, IHealthEntityComponent
    {
        private float _healthAmount;

        private IEnemyEntity _enemyEntity;
        
        public event Action<float> HealthChanged;
        public event Action EntityDeath;

        public override void Initialize(IEntity owner)
        {
            base.Initialize(owner);
            _enemyEntity = owner as IEnemyEntity;
            if (_enemyEntity != null)
                _healthAmount = _enemyEntity.EnemyEntityData.Health;
        }
        
        public void TakeDamage(int damage)
        {
            _healthAmount -= damage;
            HealthChanged?.Invoke(_healthAmount);
            if (!(_healthAmount <= 0)) 
                return;
            
            _healthAmount = 0;
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