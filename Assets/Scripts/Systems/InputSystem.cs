using System;
using Events;
using Gameplay.Interfaces;
using Systems.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using Zenject;

namespace Systems
{
    [Serializable]
    public class InputSystem : IInitializable, IDisposable
    {
        private ICameraSystem _cameraSystem;

        private GameInput _gameInput;

        private Vector2 _clickPosition;

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

            if (!Physics.Raycast(ray, out RaycastHit hit)) 
                return;
            
            IBlockEntity blockEntity = hit.collider.GetComponent<IBlockEntity>();
            if (blockEntity == null) 
                return;
                
            Vector2Int blockIndex = blockEntity.BoardIndex;
            EventBus.Publish(new BlockClicked(blockIndex));
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