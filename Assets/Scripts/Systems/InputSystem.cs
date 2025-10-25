using System;
using Systems.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems
{
    [Serializable]
    public class InputSystem : IInputSystem, IInitializable, IDisposable
    {
        private GameInput _gameInput;

        private Vector2 _clickPosition;

        public void Initialize()
        {
            _gameInput = new GameInput();

            _gameInput.Enable();
            _gameInput.Player.MouseClick.performed += OnMouseClickPerformed;
            _gameInput.Player.MouseClick.canceled += OnMouseClickCancelled;
        }

        private void OnMouseClickPerformed(InputAction.CallbackContext callbackContext)
        {
            _clickPosition = _gameInput.Player.MousePosition.ReadValue<Vector2>();
            Debug.Log($"Mouse click position: {_clickPosition}");
        }

        private void OnMouseClickCancelled(InputAction.CallbackContext callbackContext)
        {
            _clickPosition = default;
            Debug.Log($"Mouse click position: {_clickPosition}");
        }

        public void Dispose()
        {
            _gameInput.Disable();
            _gameInput.Player.MouseClick.performed -= OnMouseClickPerformed;
            _gameInput.Player.MouseClick.canceled -= OnMouseClickCancelled;
            _gameInput = null;
        }
    }
}