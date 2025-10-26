using Gameplay.Board;
using Gameplay.Objects;
using UnityEngine;
using Zenject;

namespace Installers.BoardInstallers
{
    public class BoardSpawnInstaller : MonoInstaller
    {
        [SerializeField] private BoardSpawner _boardSpawner;
        [SerializeField] private BoardCell _cellPrefab;

        public override void InstallBindings()
        {
            _boardSpawner.InjectDependencies(_cellPrefab);
            
            Container.BindInterfacesAndSelfTo<BoardSpawner>().FromInstance(_boardSpawner).AsSingle();
        }
    }
}
