using Systems;
using Zenject;

namespace Installers
{
    public class SystemInstaller : MonoInstaller 
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputSystem>().AsSingle();
            Container.BindInterfacesTo<CameraSystem>().AsSingle();
            Container.BindInterfacesTo<BoardSystem>().AsSingle();
            Container.BindInterfacesTo<WaveSystem>().AsSingle();
            Container.BindInterfacesTo<InventorySystem>().AsSingle();
            Container.BindInterfacesTo<LevelSystem>().AsSingle();
            Container.BindInterfacesTo<UISystem>().AsSingle();

            Container.BindExecutionOrder<CameraSystem>(0);
            Container.BindExecutionOrder<UISystem>(1);
            Container.BindExecutionOrder<InputSystem>(2);
            Container.BindExecutionOrder<BoardSystem>(3);
            Container.BindExecutionOrder<WaveSystem>(4);
            Container.BindExecutionOrder<InventorySystem>(5);
            Container.BindExecutionOrder<LevelSystem>(6);
        }
    }
}
