using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UI;
using UnityEngine;
using Utilities;

namespace Datas.Configs.UI_Configs
{
    [Serializable]
    public class UIElementConfig
    {
        [SerializeField] [Dropdown("GetUITagList")]
        private string _elementName;

        [SerializeField] private BaseUIElement _prefab;

        public string ElementName => _elementName;
        public BaseUIElement Prefab => _prefab;

        private static List<string> GetUITagList()
        {
            return new List<string>
            {
                Constants.LevelUIPanelTag,
                Constants.ItemSelectionPanelTag
            };
        }
    }
}