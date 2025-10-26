using UnityEngine;

namespace Datas.Configs.Item_Configs
{
    [CreateAssetMenu(fileName = "New Defence Item Library Config", menuName = "Configs/Items/Defence Item Library Config")]
    public class DefenceItemLibraryConfig : ScriptableObject
    {
        [SerializeField] private DefenceItemConfig[] _defenceItemConfigList;
        
        public DefenceItemConfig[] DefenceItemConfigList => _defenceItemConfigList;
    }
}