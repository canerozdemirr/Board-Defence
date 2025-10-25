using Systems;
using Zenject;

namespace Installers
{
    public class SystemInstaller : MonoInstaller 
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputSystem>().AsSingle();
            
            Container.BindExecutionOrder<InputSystem>(0);
        }
    }
}
