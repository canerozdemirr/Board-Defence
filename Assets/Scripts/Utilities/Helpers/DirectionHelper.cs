using System.Collections.Generic;
using Datas.EntityDatas.TowerDatas;
using UnityEngine;

namespace Utilities.Helpers
{
    public static class DirectionHelper
    {
        private static readonly Vector2Int Forward = new(0, 1);
        private static readonly Vector2Int Backward = new(0, -1);
        private static readonly Vector2Int Left = new(-1, 0);
        private static readonly Vector2Int Right = new(1, 0);

        private static readonly Vector2Int ForwardLeft = new(-1, 1);
        private static readonly Vector2Int ForwardRight = new(1, 1);
        private static readonly Vector2Int BackwardLeft = new(-1, -1);
        private static readonly Vector2Int BackwardRight = new(1, -1);

        private static readonly Vector2Int None = Vector2Int.zero;
        
        public static Vector2Int GetDirectionFromDetectionFlag(Direction detectionDirection)
        {
            if (detectionDirection.HasFlag(Direction.Forward))
                return Forward;
            if (detectionDirection.HasFlag(Direction.Backward))
                return Backward;
            if (detectionDirection.HasFlag(Direction.Left))
                return Left;
            if (detectionDirection.HasFlag(Direction.Right))
                return Right;
            if (detectionDirection.HasFlag(Direction.ForwardLeft))
                return ForwardLeft;
            if (detectionDirection.HasFlag(Direction.ForwardRight))
                return ForwardRight;
            if (detectionDirection.HasFlag(Direction.BackwardLeft))
                return BackwardLeft;
            return detectionDirection.HasFlag(Direction.BackwardRight) ? BackwardRight : None;
        }
        
        public static Vector2Int[] GetDirectionsFromDetectionFlag(Direction detectionDirection)
        {
            List<Vector2Int> directions = new();
            
            if (detectionDirection.HasFlag(Direction.Forward))
                directions.Add(Forward);
            if (detectionDirection.HasFlag(Direction.Backward))
                directions.Add(Backward);
            if (detectionDirection.HasFlag(Direction.Left))
                directions.Add(Left);
            if (detectionDirection.HasFlag(Direction.Right))
                directions.Add(Right);
            if (detectionDirection.HasFlag(Direction.ForwardLeft))
                directions.Add(ForwardLeft);
            if (detectionDirection.HasFlag(Direction.ForwardRight))
                directions.Add(ForwardRight);
            if (detectionDirection.HasFlag(Direction.BackwardLeft))
                directions.Add(BackwardLeft);
            if (detectionDirection.HasFlag(Direction.BackwardRight))
                directions.Add(BackwardRight);

            return directions.ToArray();
        }
    }
}