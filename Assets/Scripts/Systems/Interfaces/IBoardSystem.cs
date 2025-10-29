using System.Collections.Generic;
using Datas.BoardDatas;
using Gameplay.Interfaces;
using UnityEngine;

namespace Systems.Interfaces
{
    public interface IBoardSystem
    {
        BoardSizeData BoardSizeData { get; }
        Vector3 GetWorldPositionFromBlock(Vector2Int blockIndex);
        bool IsBlockOccupied(Vector2Int blockIndex);
        void OccupyBlock(Vector2Int blockIndex);
        void FreeBlock(Vector2Int blockIndex);
        bool IsValidPlacementPosition(Vector2Int blockIndex);
        void AddEntityAtBlock(Vector2Int blockIndex, IBlockEntity entity);
        void RemoveEntityAtBlock(Vector2Int blockIndex, IBlockEntity entity);
        List<IBlockEntity> GetEntitiesAtBlock(Vector2Int blockIndex);
    }
}