using System.Text;
using Datas;
using Events.Enemies;
using NaughtyAttributes;
using Systems.Interfaces;
using TMPro;
using UnityEngine;
using Utilities;
using Zenject;

namespace UI.Elements
{
    public class LevelUIPanel : BaseUIElement
    {
        [BoxGroup("UI References")] [SerializeField]
        private TextMeshProUGUI _levelNumberText;

        [BoxGroup("UI References")] [SerializeField]
        private TextMeshProUGUI _enemyCountText;

        [Inject] private ILevelSystem _levelSystem;

        private int _enemyCount;
        private int _currentLevelCount;
        private int _totalLevelCount;

        private readonly StringBuilder _textBuilder = new();

        protected override void OnInitialize()
        {
            _levelSystem.LevelDataLoaded += OnLevelDataLoaded;
            _levelSystem.LevelStarted += OnLevelStarted;
            EventBus.Subscribe<EnemyDeath>(OnEnemyDeath);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        private void OnLevelStarted(int levelIndex)
        {
            _currentLevelCount = levelIndex;
            _textBuilder.Clear();
            _textBuilder.Append("Level ");
            _textBuilder.Append(_currentLevelCount);
            _textBuilder.Append(" / ");
            _textBuilder.Append(_totalLevelCount);
            _levelNumberText.SetText(_textBuilder.ToString());
        }

        private void OnLevelDataLoaded(LevelData levelData)
        {
            _totalLevelCount = levelData.WaveDataList.Count;
        }

        private void OnEnemyDeath(EnemyDeath obj)
        {
            _enemyCount--;
            _textBuilder.Clear();
            _textBuilder.Append(_enemyCount);
            _textBuilder.Append(" enemies left.");
            _enemyCountText.SetText(_textBuilder.ToString());
        }

        protected override void OnCleanup()
        {
            _levelSystem.LevelStarted -= OnLevelStarted;
            _levelSystem.LevelDataLoaded -= OnLevelDataLoaded;
            EventBus.Unsubscribe<EnemyDeath>(OnEnemyDeath);
        }
    }
}