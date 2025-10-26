using System;
using Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    [Serializable]
    public class CameraSystem : ICameraSystem, IInitializable, IDisposable
    {
        public Camera GameplayCamera {get; private set; }
        public void Initialize()
        {
            GameplayCamera = Camera.main;
        }

        public void Dispose()
        {
            GameplayCamera = null;
        }

        
    }
}