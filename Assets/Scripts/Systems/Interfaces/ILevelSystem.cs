using System;
using Datas;

namespace Systems.Interfaces
{
    public interface ILevelSystem
    {
        event Action<LevelData> LevelDataLoaded; 
        event Action<int> LevelStarted;
        event Action<int> LevelCompleted;
    }
}