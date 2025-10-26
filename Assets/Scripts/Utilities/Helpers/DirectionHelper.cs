using System.Collections.Generic;
using Datas.ItemDatas;
using UnityEngine;

namespace Utilities.Helpers
{
    public static class DirectionHelper
    {
        private static readonly Vector2Int Forward = new(1, 0);
        private static readonly Vector2Int Backward = new(-1, 0);
        private static readonly Vector2Int Left = new(0, -1);
        private static readonly Vector2Int Right = new(0, 1);

        private static readonly Vector2Int ForwardLeft = new(1, -1);
        private static readonly Vector2Int ForwardRight = new(1, 1);
        private static readonly Vector2Int BackwardLeft = new(-1, -1);
        private static readonly Vector2Int BackwardRight = new(-1, 1);
     
        public static readonly Vector2Int None = Vector2Int.zero;

        private static Vector2Int[] GetCardinalDirections()
        {
            return new[] { Forward, Backward, Left, Right };
        }

        private static Vector2Int[] GetAllDirections()
        {
            return new[] 
            { 
                Forward, Backward, Left, Right,
                ForwardLeft, ForwardRight, BackwardLeft, BackwardRight
            };
        }
        
        public static Vector2Int GetRandomCardinal()
        {
            Vector2Int[] directions = GetCardinalDirections();
            return directions[Random.Range(0, directions.Length)];
        }
        
        public static Vector2Int GetRandomDirection()
        {
            Vector2Int[] directions = GetAllDirections();
            return directions[Random.Range(0, directions.Length)];
        }
        
        public static Vector2Int[] GetDirectionsFromDetectionFlag(EnemyDetectionDirection detectionDirection)
        {
            List<Vector2Int> directions = new();
            
            if (detectionDirection.HasFlag(EnemyDetectionDirection.Forward))
                directions.Add(Forward);
            if (detectionDirection.HasFlag(EnemyDetectionDirection.Backward))
                directions.Add(Backward);
            if (detectionDirection.HasFlag(EnemyDetectionDirection.Left))
                directions.Add(Left);
            if (detectionDirection.HasFlag(EnemyDetectionDirection.Right))
                directions.Add(Right);

            return directions.ToArray();
        }
    }
}