using Datas.BoardDatas;
using Datas.Configs;
using Datas.Configs.Board_Configs;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "Board Config Installer", menuName = "Installers/Board Config Installer")]
    public class BoardConfigInstaller : ScriptableObjectInstaller<BoardConfigInstaller>
    {
        [SerializeField] private BoardPreparationListConfig _boardPreparationListConfig;
        public override void InstallBindings()
        {
            Container.Bind<BoardPreparationListConfig>().FromInstance(_boardPreparationListConfig).AsSingle();
        }
    }
}