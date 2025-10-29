using System;
using NaughtyAttributes;
using UnityEngine;

namespace Datas.EntityDatas.ProjectileDatas
{
    [Serializable]
    public struct ProjectileEntityData
    {
        private string _projectileID;

        [SerializeField] private string _projectileName;

        [SerializeField] [Min(0f)] private float _speed;

        public string ProjectileID => _projectileID;
        public string ProjectileName => _projectileName;
        public float Speed => _speed;

        public void SetID(string id)
        {
            _projectileID = id;
        }

        public ProjectileEntityData Clone()
        {
            return new ProjectileEntityData
            {
                _projectileID = _projectileID,
                _projectileName = _projectileName,
                _speed = _speed
            };
        }
    }
}
