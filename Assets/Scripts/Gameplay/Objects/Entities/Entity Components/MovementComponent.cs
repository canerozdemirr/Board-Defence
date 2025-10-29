using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Datas.BoardDatas;
using Datas.EntityDatas.EnemyDatas;
using Gameplay.Interfaces;
using Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Objects.Entities.Entity_Components
{
    public class MovementComponent : BaseEntityComponent, IMovementEntityComponent
    {
        private float _blockPassPerSecond;
        private float _movementSpeed;
        private IEnemyEntity _enemyEntity;

        private CancellationTokenSource _moveCancellationToken;

        private IBoardSystem _boardSystem;
        private BoardSizeData _boardSizeData;
        private EnemyEntityData _enemyEntityData;

        private Vector2Int _currentBlockIndex;
        private Vector2Int _targetBlockIndex;

        private Vector3 _targetWorldPosition;

        private bool _isMoving;
        private float _moveProgress;
        private Vector3 _moveStartPosition;

        public Vector2Int CurrentBlockIndex => _currentBlockIndex;
        public bool IsMoving => _isMoving;
        public event Action ReachToEndBlock;

        private readonly AnimationCurve _movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Inject]
        public void Inject(IBoardSystem boardSystem)
        {
            _boardSystem = boardSystem;
        }

        public override void Initialize(IEntity owner)
        {
            base.Initialize(owner);
            _enemyEntity = owner as IEnemyEntity;
            if (_enemyEntity != null)
            {
                _enemyEntityData = _enemyEntity.EnemyEntityData;
                _blockPassPerSecond = _enemyEntityData.BlockPassPerSecond;
                SetSpeedBySecondsPerBlock();
            }

            _boardSizeData = _boardSystem.BoardSizeData;
            _currentBlockIndex = WorldToBlockPosition(transform.position);
            _targetBlockIndex = _currentBlockIndex;
        }

        public override void Disable()
        {
            base.Disable();
            StopMovement();
        }

        private void StartMovement()
        {
            _isMoving = true;
            _moveProgress = 0f;
            _moveStartPosition = transform.position;

            _moveCancellationToken?.Cancel();
            _moveCancellationToken?.Dispose();
            _moveCancellationToken = new CancellationTokenSource();

            int blockDistance = GetBlockDistance(_currentBlockIndex, _targetBlockIndex);
            float moveDuration = blockDistance * (1f / _movementSpeed);

            StartMovementRoutine(moveDuration, _moveCancellationToken.Token).Forget();
        }

        private async UniTask StartMovementRoutine(float duration, CancellationToken cancellationToken)
        {
            float elapsed = 0f;

            _boardSystem.RemoveEntityAtBlock(_currentBlockIndex, _enemyEntity);

            while (elapsed < duration && !cancellationToken.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                _moveProgress = Mathf.Clamp01(elapsed / duration);

                float curvedProgress = _movementCurve.Evaluate(_moveProgress);

                Vector3 newPosition = Vector3.Lerp(_moveStartPosition, _targetWorldPosition, curvedProgress);
                transform.position = newPosition;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                transform.position = _targetWorldPosition;
                _currentBlockIndex = _targetBlockIndex;
                _boardSystem.AddEntityAtBlock(_targetBlockIndex, _enemyEntity);
                _isMoving = false;
                _moveProgress = 1f;

                if (_boardSizeData.ColumnNumber - _currentBlockIndex.y == _boardSizeData.ColumnNumber)
                {
                    ReachToEndBlock?.Invoke();
                }
            }
        }

        private Vector3 BlockToWorldPosition(Vector2Int gridPosition)
        {
            float halfWidth = (_boardSizeData.RowNumber - 1) * _boardSizeData.BlockSize / 2f;
            float halfDepth = (_boardSizeData.ColumnNumber - 1) * _boardSizeData.BlockSize / 2f;

            float x = gridPosition.x * _boardSizeData.BlockSize - halfWidth;
            float z = gridPosition.y * _boardSizeData.BlockSize - halfDepth;

            return _boardSizeData.BoardCenterPosition + new Vector3(x, transform.position.y, z);
        }

        private Vector2Int WorldToBlockPosition(Vector3 worldPosition)
        {
            float halfWidth = (_boardSizeData.RowNumber - 1) * _boardSizeData.BlockSize / 2f;
            float halfDepth = (_boardSizeData.ColumnNumber - 1) * _boardSizeData.BlockSize / 2f;

            Vector3 localPos = worldPosition - _boardSizeData.BoardCenterPosition;

            int row = Mathf.RoundToInt((localPos.x + halfWidth) / _boardSizeData.BlockSize);
            int col = Mathf.RoundToInt((localPos.z + halfDepth) / _boardSizeData.BlockSize);

            return new Vector2Int(row, col);
        }

        private void MoveToBlockPosition(Vector2Int targetPosition)
        {
            if (_isMoving)
            {
                StopMovement();
            }

            _targetBlockIndex = targetPosition;
            _targetWorldPosition = BlockToWorldPosition(targetPosition);
            StartMovement();
        }

        private void StopMovement()
        {
            if (!_isMoving) return;

            _moveCancellationToken?.Cancel();
            _moveCancellationToken?.Dispose();
            _moveCancellationToken = null;

            _isMoving = false;
            _moveProgress = 0f;

            _currentBlockIndex = WorldToBlockPosition(transform.position);
            _targetBlockIndex = _currentBlockIndex;

            transform.position = BlockToWorldPosition(_currentBlockIndex);
        }

        private void SetSpeed(float blocksPerSecond)
        {
            _movementSpeed = Mathf.Max(0.01f, blocksPerSecond);
        }

        private void SetSpeedBySecondsPerBlock()
        {
            SetSpeed(_blockPassPerSecond);
        }

        private int GetBlockDistance(Vector2Int from, Vector2Int to)
        {
            return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y);
        }

        private bool CanMoveTo(Vector2Int targetPosition)
        {
            return targetPosition.x >= 0 && targetPosition.x < _boardSizeData.RowNumber &&
                   targetPosition.y >= 0 && targetPosition.y < _boardSizeData.ColumnNumber;
        }

        public void MoveInDirection(Vector2Int direction)
        {
            Vector2Int targetPosition = _currentBlockIndex + direction;
            if (!CanMoveTo(targetPosition))
                return;
            MoveToBlockPosition(targetPosition);
        }

        private void OnDestroy()
        {
            _moveCancellationToken?.Cancel();
            _moveCancellationToken?.Dispose();
            _moveCancellationToken = null;
            _enemyEntity = null;
            _boardSystem = null;
        }
    }
}