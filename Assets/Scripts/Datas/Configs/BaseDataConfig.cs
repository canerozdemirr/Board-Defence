using System;
using NaughtyAttributes;
using UnityEngine;

namespace Datas.Configs
{
    public class BaseDataConfig : ScriptableObject
    {
        [SerializeField]
        [ReadOnly]
        protected string _configID;
        
        public string ConfigID => _configID;
        
        [Button]
        public void GenerateConfigID()
        {
            if (!string.IsNullOrEmpty(_configID))
            {
                Debug.LogError("The id is already been assigned! You can't re-assign it again.");
                return;
            }
            _configID = Guid.NewGuid().ToString();
        }
    }
}