using Datas.BoardDatas;
using Datas.Configs;
using Datas.Configs.Board_Configs;
using Datas.Configs.Level_Configs;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "Config Installer", menuName = "Installers/Config Installer")]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] private BoardPreparationConfig _boardPreparationConfig;
        [SerializeField] private LevelConfig _levelConfig;
        public override void InstallBindings()
        {
            Container.Bind<BoardPreparationConfig>().FromInstance(_boardPreparationConfig).AsSingle();
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
        }
    }
}