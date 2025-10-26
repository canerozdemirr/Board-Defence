using Events.Interfaces;
using UnityEngine;

namespace Events
{
    public struct MouseClicked : IEvent
    {
        public readonly Vector3 MouseClickedPosition;
        
        public MouseClicked(Vector3 mouseClickedPosition)
        {
            MouseClickedPosition = mouseClickedPosition;
        }
    }
}