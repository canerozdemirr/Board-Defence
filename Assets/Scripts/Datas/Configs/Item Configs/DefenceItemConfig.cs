using Datas.ItemDatas;
using UnityEngine;

namespace Datas.Configs.Item_Configs
{
    [CreateAssetMenu(fileName = "New Defence Item Config", menuName = "Configs/Items/Defence Item Config")]
    public class DefenceItemConfig : ScriptableObject
    {
        [SerializeField] private DefenceItemData _defenceItemData;
        
        public DefenceItemData DefenceItemData => _defenceItemData;
    }
}