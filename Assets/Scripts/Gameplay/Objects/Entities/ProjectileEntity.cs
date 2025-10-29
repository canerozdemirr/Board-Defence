using System;
using Datas.EntityDatas.ProjectileDatas;
using Events.Projectile;
using Gameplay.Interfaces;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Entities
{
    public class ProjectileEntity : MonoBehaviour, IEntity, IPoolable
    {
        private ProjectileEntityData _projectileEntityData;
        private IEnemyEntity _targetEnemy;
        private float _damage;
        private float _speed;

        public Transform WorldTransform => transform;
        public ProjectileEntityData ProjectileEntityData => _projectileEntityData;

        public void Initialize()
        {
            _speed = _projectileEntityData.Speed;
        }

        public void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        public void OnCalledFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            _targetEnemy = null;
            _damage = 0f;
            OnDeactivate();
        }

        public void AssignProjectileData(ProjectileEntityData projectileData)
        {
            _projectileEntityData = projectileData.Clone();
        }

        public void SetTarget(IEnemyEntity target)
        {
            _targetEnemy = target;
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        private void Update()
        {
            if (_targetEnemy == null)
            {
                // Target is killed by other means before projectile hits, return projectile to pool
                ReturnToPool();
                return;
            }

            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            Vector3 targetPosition = _targetEnemy.WorldTransform.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Constants.EnemyTag))
            {
                OnHitTarget();
            }
        }

        private void OnHitTarget()
        {
            _targetEnemy.TakeDamage(_damage);
            EventBus.Publish(new ProjectileHit(this));
        }

        private void ReturnToPool()
        {
            EventBus.Publish(new ProjectileHit(this));
        }
    }
}
