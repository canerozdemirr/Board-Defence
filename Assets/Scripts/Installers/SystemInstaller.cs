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
            
            Container.BindExecutionOrder<CameraSystem>(0);
            Container.BindExecutionOrder<InputSystem>(1);
        }
    }
}
