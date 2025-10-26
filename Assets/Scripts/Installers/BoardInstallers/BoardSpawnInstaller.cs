using Gameplay.Board;
using Gameplay.Objects;
using Gameplay.Objects.Entities;
using Gameplay.Objects.Entities.Board_Entities;
using UnityEngine;
using Zenject;

namespace Installers.BoardInstallers
{
    public class BoardSpawnInstaller : MonoInstaller
    {
        [SerializeField] private BoardSpawner _boardSpawner;
        [SerializeField] private BoardCellEntity _cellEntityPrefab;

        public override void InstallBindings()
        {
            _boardSpawner.InjectDependencies(_cellEntityPrefab);
            
            Container.BindInterfacesAndSelfTo<BoardSpawner>().FromInstance(_boardSpawner).AsSingle();
        }
    }
}
