using Enums;
using ScriptableObjects;
using System;
using System.Linq;
using Views.UI;

namespace Controllers.UI
{
    public class EndGameController : IDisposable
    {
        private EndGameView _endGameView;
        private GameScoreData _gameScoreData;

        public event Action RestartButtonPressedEvent;
        public EndGameController(EndGameView endGameView)
        {
            _endGameView = endGameView;
            _endGameView.RestartButton.onClick.AddListener(RestartGame);
            _endGameView.ContinuetButton.onClick.AddListener(ContinueGame);

            _gameScoreData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData;
        }
        public void Dispose()
        {
            _endGameView.RestartButton.onClick.RemoveAllListeners();
            _endGameView.ContinuetButton.onClick.RemoveAllListeners();
        }
        public void ShowBadEndGame()
        {
            _endGameView.BadEndPanelView.gameObject.SetActive(true);
            _endGameView.BadEndPanelView.ScoreText.text = $"Набрано очков: {_gameScoreData.Score}" +
                $"\r\nЛиквидировано: {_gameScoreData.EnemyKillCount}";
        }
        public void ShowGoodEndGame()
        {
            _endGameView.ContinuetButton.gameObject.SetActive(true);
            _endGameView.GoodEndPanelView.gameObject.SetActive(true);
            _endGameView.GoodEndPanelView.ScoreText.text = $"Набрано очков: {_gameScoreData.Score}" +
                $"\r\nЛиквидировано: {_gameScoreData.EnemyKillCount}";
        }
        private void RestartGame()
        {
            Dispose();
            _gameScoreData.Reset();
            RestartButtonPressedEvent?.Invoke();
            DataController.GameState = GameState.RESTART_GAME;
        }
        private void ContinueGame()
        {
            var levelData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.LEVEL_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as LevelsData;
            var currentLevelId = levelData.CurrentLevel.Prefab.GetInstanceID();
            var nextLevel = levelData.Levels
                .Where(i => i.Prefab.GetInstanceID() != currentLevelId)
                .Select(i => i.Prefab)
                .FirstOrDefault();
            levelData.CurrentLevel.Prefab = nextLevel;
            RestartGame();
        }
    }
}