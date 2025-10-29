using Datas.Configs.UI_Configs;
using Gameplay.Spawners;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIElementLibrary _uiElementLibrary;
        [SerializeField] private UISpawner _uiSpawner;

        public override void InstallBindings()
        {
            Container.Inject(_uiSpawner);
            _uiSpawner.SetUIElementLibrary(_uiElementLibrary);

            Container.BindInterfacesAndSelfTo<UISpawner>().FromInstance(_uiSpawner).AsSingle();
        }
    }
}
