using System.Collections.Generic;
using UnityEngine;

namespace Datas.Configs.UI_Configs
{
    [CreateAssetMenu(fileName = "New UI Element Library", menuName = "Configs/UI/UI Element Library")]
    public class UIElementLibrary : ScriptableObject
    {
        [SerializeField] private List<UIElementConfig> _uiElementConfigs;

        public List<UIElementConfig> UIElementConfigs => _uiElementConfigs;
    }
}
