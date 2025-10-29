using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datas.Configs.Entity_Configs.Projectile_Configs;
using Datas.EntityDatas.ProjectileDatas;
using Gameplay.Interfaces;
using Gameplay.Objects.Entities;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay.Spawners
{
    public class ProjectileSpawner : MonoBehaviour, IProjectileSpawner, IInitializable
    {
        private Dictionary<string, AddressableGameObjectPool<ProjectileEntity>> _projectileEntityPoolMap;
        private ProjectileEntityLibrary _projectileEntityLibrary;
        private List<ProjectileEntity> _activeProjectiles;

        [Inject] private DiContainer _container;

        public void SetProjectileEntityLibrary(ProjectileEntityLibrary projectileEntityLibrary)
        {
            _projectileEntityLibrary = projectileEntityLibrary;
        }

        public void Initialize()
        {
            _projectileEntityPoolMap = new Dictionary<string, AddressableGameObjectPool<ProjectileEntity>>();
            _activeProjectiles = new List<ProjectileEntity>();
        }

        public async UniTask<ProjectileEntity> ProvideProjectileEntity(string projectileName, Vector3 spawnPosition)
        {
            ProjectileEntity spawnedProjectile;
            ProjectileEntityData projectileEntityData;

            if (_projectileEntityPoolMap.ContainsKey(projectileName))
            {
                spawnedProjectile = _projectileEntityPoolMap[projectileName].Spawn();
                _container.InjectGameObject(spawnedProjectile.gameObject);

                foreach (ProjectileEntityConfig entityConfig in _projectileEntityLibrary.ProjectileEntityConfigList)
                {
                    projectileEntityData = entityConfig.ProjectileEntityData.Clone();
                    if (projectileEntityData.ProjectileName != projectileName)
                        continue;
                    spawnedProjectile.AssignProjectileData(projectileEntityData);
                    break;
                }

                spawnedProjectile.transform.position = spawnPosition;
                _activeProjectiles.Add(spawnedProjectile);
                return spawnedProjectile;
            }

            AddressableGameObjectPool<ProjectileEntity> projectilePool = null;
            foreach (ProjectileEntityConfig projectileEntityConfig in _projectileEntityLibrary.ProjectileEntityConfigList)
            {
                projectileEntityData = projectileEntityConfig.ProjectileEntityData.Clone();
                if (projectileEntityData.ProjectileName != projectileName)
                    continue;

                projectilePool = await AddressableGameObjectPool<ProjectileEntity>.CreateAsync(
                    projectileEntityConfig.EntityVisualData.AssetReference, transform);
                break;
            }

            _projectileEntityPoolMap.Add(projectileName, projectilePool);
            spawnedProjectile = _projectileEntityPoolMap[projectileName].Spawn();
            _container.InjectGameObject(spawnedProjectile.gameObject);

            foreach (ProjectileEntityConfig entityConfig in _projectileEntityLibrary.ProjectileEntityConfigList)
            {
                projectileEntityData = entityConfig.ProjectileEntityData.Clone();
                if (projectileEntityData.ProjectileName != projectileName)
                    continue;
                spawnedProjectile.AssignProjectileData(projectileEntityData);
                break;
            }

            spawnedProjectile.transform.position = spawnPosition;
            _activeProjectiles.Add(spawnedProjectile);
            return spawnedProjectile;
        }

        public void ReturnProjectileToPool(ProjectileEntity projectile)
        {
            string projectileName = projectile.GetComponent<ProjectileEntity>().ToString();
            if (!_projectileEntityPoolMap.TryGetValue(projectileName, out AddressableGameObjectPool<ProjectileEntity> pool))
                return;

            _activeProjectiles.Remove(projectile);
            projectile.OnDeactivate();
            pool.DeSpawn(projectile);
        }

        private void OnDestroy()
        {
            foreach (AddressableGameObjectPool<ProjectileEntity> pool in _projectileEntityPoolMap.Values)
            {
                pool.ClearObjectReferences();
            }

            _projectileEntityPoolMap.Clear();
            _projectileEntityPoolMap = null;
            _projectileEntityLibrary = null;
            _activeProjectiles.Clear();
            _activeProjectiles = null;
        }
    }
}
