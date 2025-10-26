using System;
using Events;
using Systems.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class InputSystem : IInputSystem, IInitializable, IDisposable
    {
        private ICameraSystem _cameraSystem;

        private GameInput _gameInput;

        private Vector2 _clickPosition;
        private Vector3 _screenPosition;

        public InputSystem(ICameraSystem cameraSystem)
        {
            _cameraSystem = cameraSystem;
        }

        public void Initialize()
        {
            _gameInput = new GameInput();

            _gameInput.Enable();
            _gameInput.Player.Enable();
            _gameInput.Player.Click.performed += OnMouseClickPerformed;
            _gameInput.Player.Click.canceled += OnMouseClickCancelled;
        }

        private void OnMouseClickPerformed(InputAction.CallbackContext callbackContext)
        {
            _clickPosition = Mouse.current.position.ReadValue();
            Ray ray = _cameraSystem.GameplayCamera.ScreenPointToRay(_clickPosition);

            if (!(Mathf.Abs(ray.direction.y) > 0.0001f)) 
                return;
            
            float t = -ray.origin.y / ray.direction.y;
            _screenPosition = ray.origin + ray.direction * t;
            
            EventBus.Publish(new MouseClicked(_screenPosition));
            
            Debug.Log("Player clicked at world position: " + _screenPosition);
        }

        private void OnMouseClickCancelled(InputAction.CallbackContext callbackContext)
        {
            _clickPosition = default;
        }

        public void Dispose()
        {
            _gameInput.Disable();
            _gameInput.Player.Click.performed -= OnMouseClickPerformed;
            _gameInput.Player.Click.canceled -= OnMouseClickCancelled;
            _gameInput = null;
        }
    }
}