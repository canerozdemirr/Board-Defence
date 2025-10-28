using Datas.Configs.Entity_Configs.Enemy_Configs;
using Gameplay.Spawners;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemySpawnInstaller : MonoInstaller
    {
        [SerializeField] private EnemyEntityLibrary _enemyEntityLibrary;
        [SerializeField] private EnemySpawner _enemySpawner;
        
        public override void InstallBindings()
        {
            Container.Inject(_enemySpawner);
            _enemySpawner.SetEnemyEntityLibrary(_enemyEntityLibrary);
            Container.BindInterfacesAndSelfTo<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
        }
    }
}