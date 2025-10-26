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
            
            //TODO: We might want to turn this into more of a editor-based order editing for more convenience but for now this will do.
            Container.BindExecutionOrder<CameraSystem>(0);
            Container.BindExecutionOrder<InputSystem>(1);
            Container.BindExecutionOrder<BoardSystem>(2);
        }
    }
}
