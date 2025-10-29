using System;

namespace Systems.Interfaces
{
    public interface IItemPlacementSystem
    {
        void StartPlacementMode(string itemName);
        void CancelPlacementMode();
        bool IsInPlacementMode { get; }
        event Action PlacementStarted;
        event Action PlacementEnded;
    }
}