using System;

namespace Systems.Interfaces
{
    public interface ITowerPlacementSystem
    {
        public event Action PlacementStarted;
        public event Action PlacementEnded;
        public event Action<string, int> TowerInventoryUpdated;
    }
}