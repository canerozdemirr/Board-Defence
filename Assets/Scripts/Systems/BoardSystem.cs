using System;
using Datas.Configs;
using UnityEngine;
using Zenject;

namespace Systems
{
    [Serializable]
    public class BoardSystem : IInitializable, IDisposable
    {
        private BoardPreparationListConfig _boardPreparationListConfig;
        
        public BoardSystem(BoardPreparationListConfig boardPreparationListConfig)
        {
            _boardPreparationListConfig = boardPreparationListConfig;
        }
        
        public void Initialize()
        {
            BoardPreparationConfig boardPreparationConfig = _boardPreparationListConfig.GetCurrentBoardPreparationConfig();
            Debug.Log(boardPreparationConfig.BoardSizeData.RowNumber);
            Debug.Log(boardPreparationConfig.BoardSizeData.ColumnNumber);
            Debug.Log(boardPreparationConfig.BoardSizeData.CellSize);
            Debug.Log(boardPreparationConfig.BoardSizeData.BoardOriginPosition);
        }

        public void Dispose()
        {
            
        }
    }
}