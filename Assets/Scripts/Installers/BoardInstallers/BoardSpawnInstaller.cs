using Gameplay.Board;
using UnityEngine;
using Zenject;

namespace Installers.BoardInstallers
{
    public class BoardSpawnInstaller : MonoInstaller
    {
        [SerializeField] private BoardSpawner _boardSpawner;
        [SerializeField] private GameObject _cellPrefab;

        public override void InstallBindings()
        {
            _boardSpawner.InjectDependencies(_cellPrefab);
            
            Container.BindInterfacesAndSelfTo<BoardSpawner>().FromInstance(_boardSpawner).AsSingle();
        }
    }
}
