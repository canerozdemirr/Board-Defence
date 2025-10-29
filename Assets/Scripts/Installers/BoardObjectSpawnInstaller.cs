using Datas.Configs.Entity_Configs.Enemy_Configs;
using Datas.Configs.Entity_Configs.Projectile_Configs;
using Datas.Configs.Entity_Configs.Tower_Configs;
using Gameplay.Objects.Entities.Board_Entities;
using Gameplay.Spawners;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BoardObjectSpawnInstaller : MonoInstaller
    {
        [BoxGroup("Enemy Spawn Attributes")] [SerializeField]
        private EnemyEntityLibrary _enemyEntityLibrary;

        [BoxGroup("Enemy Spawn Attributes")] [SerializeField]
        private EnemySpawner _enemySpawner;

        [BoxGroup("Tower Spawn Attributes")] [SerializeField]
        private TowerSpawner _towerSpawner;

        [BoxGroup("Tower Spawn Attributes")] [SerializeField]
        private TowerEntityLibrary _towerEntityLibrary;

        [BoxGroup("Board Spawn Attributes")] [SerializeField]
        private BoardSpawner _boardSpawner;

        [BoxGroup("Board Spawn Attributes")] [SerializeField]
        private BoardBlockEntity _blockEntityPrefab;
        
        [BoxGroup("Projectile Spawn Attributes")] [SerializeField]
        private ProjectileSpawner _projectileSpawner;
        
        [BoxGroup("Projectile Spawn Attributes")] [SerializeField]
        private ProjectileEntityLibrary _projectileEntityLibrary;

        public override void InstallBindings()
        {
            Container.Inject(_enemySpawner);
            Container.Inject(_towerSpawner);
            Container.Inject(_boardSpawner);
            Container.Inject(_projectileSpawner);

            _enemySpawner.SetEnemyEntityLibrary(_enemyEntityLibrary);
            _towerSpawner.SetTowerEntityLibrary(_towerEntityLibrary);
            _boardSpawner.InjectDependencies(_blockEntityPrefab);
            _projectileSpawner.SetProjectileEntityLibrary(_projectileEntityLibrary);

            Container.BindInterfacesAndSelfTo<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
            Container.BindInterfacesAndSelfTo<TowerSpawner>().FromInstance(_towerSpawner).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardSpawner>().FromInstance(_boardSpawner).AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().FromInstance(_projectileSpawner).AsSingle();
        }
    }
}