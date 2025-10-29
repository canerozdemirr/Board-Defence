using Datas.EntityDatas.ProjectileDatas;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Objects.Entities
{
    public class ProjectileEntity : MonoBehaviour, IProjectileEntity, IPoolable
    {
        private ProjectileEntityData _projectileEntityData;
        private IEnemyEntity _targetEnemy;
        private float _damage;
        private float _speed;

        public Transform WorldTransform => transform;

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

            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 0.1f)
            {
                OnHitTarget();
            }
        }

        private void OnHitTarget()
        {
            _targetEnemy.TakeDamage(_damage);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _targetEnemy = null;
            _damage = 0f;
            OnDeactivate();
        }
    }
}
