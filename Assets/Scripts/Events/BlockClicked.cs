using Events.Interfaces;
using UnityEngine;

namespace Events
{
    public struct BlockClicked : IEvent
    {
        public readonly Vector2Int BlockIndex;

        public BlockClicked(Vector2Int blockIndex)
        {
            BlockIndex = blockIndex;
        }
    }
}