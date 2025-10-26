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
    public class BoardMovementComponent : BaseEntityComponent, IBoardMovementEntityComponent
    {
        private float _blockPassPerSecond;
        private float _movementSpeed;
        private IEnemyEntity _enemyEntity;

        private CancellationTokenSource _moveCancellationToken;

        private IBoardSystem _boardSystem;
        private BoardSizeData _boardSizeData;
        private EnemyEntityData _enemyEntityData;

        private Vector2Int _currentBlockPosition;
        private Vector2Int _targetBlockPosition;
        private Vector3 _targetWorldPosition;

        private bool _isMoving;
        private float _moveProgress;
        private Vector3 _moveStartPosition;

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
                SetSpeedBySecondsPerCell();
            }
            
            _boardSizeData = _boardSystem.BoardSizeData;
            _currentBlockPosition = WorldToGridPosition(transform.position);
            _targetBlockPosition = _currentBlockPosition;
        }

        private void StartSmoothMovement()
        {
            _isMoving = true;
            _moveProgress = 0f;
            _moveStartPosition = transform.position;

            _moveCancellationToken?.Cancel();
            _moveCancellationToken?.Dispose();
            _moveCancellationToken = new CancellationTokenSource();

            int cellDistance = GetCellDistance(_currentBlockPosition, _targetBlockPosition);
            float moveDuration = cellDistance * (1f / _movementSpeed);

            StartMovementRoutine(moveDuration, _moveCancellationToken.Token).Forget();
        }

        private async UniTask StartMovementRoutine(float duration, CancellationToken cancellationToken)
        {
            float elapsed = 0f;

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
                _currentBlockPosition = _targetBlockPosition;
                _isMoving = false;
                _moveProgress = 1f;
            }
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            float halfWidth = (_boardSizeData.RowNumber - 1) * _boardSizeData.CellSize / 2f;
            float halfDepth = (_boardSizeData.ColumnNumber - 1) * _boardSizeData.CellSize / 2f;

            float x = gridPosition.x * _boardSizeData.CellSize - halfWidth;
            float z = gridPosition.y * _boardSizeData.CellSize - halfDepth;

            return _boardSizeData.BoardCenterPosition + new Vector3(x, _boardSizeData.CellYPosition, z);
        }

        private Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            float halfWidth = (_boardSizeData.RowNumber - 1) * _boardSizeData.CellSize / 2f;
            float halfDepth = (_boardSizeData.ColumnNumber - 1) * _boardSizeData.CellSize / 2f;

            Vector3 localPos = worldPosition - _boardSizeData.BoardCenterPosition;

            int row = Mathf.RoundToInt((localPos.x + halfWidth) / _boardSizeData.CellSize);
            int col = Mathf.RoundToInt((localPos.z + halfDepth) / _boardSizeData.CellSize);

            return new Vector2Int(row, col);
        }

        private void MoveToGridPosition(Vector2Int targetPosition)
        {
            if (_isMoving)
            {
                StopMovement();
            }
            
            _targetBlockPosition = targetPosition;
            _targetWorldPosition = GridToWorldPosition(targetPosition);
            StartSmoothMovement();
        }

        public void StopMovement()
        {
            if (!_isMoving) return;
            
            _moveCancellationToken?.Cancel();
            _moveCancellationToken?.Dispose();
            _moveCancellationToken = null;
            
            _isMoving = false;
            _moveProgress = 0f;
            
            _currentBlockPosition = WorldToGridPosition(transform.position);
            _targetBlockPosition = _currentBlockPosition;
        }

        private void SetSpeed(float blocksPerSecond)
        {
            _movementSpeed = Mathf.Max(0.01f, blocksPerSecond);
        }

        private void SetSpeedBySecondsPerCell()
        {
            float blocksPerSecond = 1f / Mathf.Max(0.01f, _blockPassPerSecond);
            SetSpeed(blocksPerSecond);
        }

        public float GetTimeToReach(Vector2Int targetPosition)
        {
            int distance = GetCellDistance(_currentBlockPosition, targetPosition);
            return distance * (1f / _movementSpeed);
        }

        public int GetDistanceTo(Vector2Int targetPosition)
        {
            return GetCellDistance(_currentBlockPosition, targetPosition);
        }

        private int GetCellDistance(Vector2Int from, Vector2Int to)
        {
            return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y);
        }

        public bool CanMoveTo(Vector2Int targetPosition)
        {
            return targetPosition.x >= 0 && targetPosition.x < _boardSizeData.RowNumber &&
                   targetPosition.y >= 0 && targetPosition.y < _boardSizeData.ColumnNumber;
        }

        public void MoveInDirection(Vector2Int direction)
        {
            Vector2Int targetPos = _currentBlockPosition + direction;
            MoveToGridPosition(targetPos);
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