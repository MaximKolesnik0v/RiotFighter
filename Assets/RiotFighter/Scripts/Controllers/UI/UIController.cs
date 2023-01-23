using Enums;
using ScriptableObjects;
using System;
using System.Linq;
using Views.UI;

namespace Controllers.UI
{
    public class UIController : BaseController, IDisposable
    {
        private MainUIView _mainUiView;
        private MenuView _menuView;
        private GameHUDView _gameHudView;
        private EndGameView _endGameView;

        private MenuController _menuController;
        private GameHUDController _gameHudController;
        private EndGameController _endGameController;

        private Action EndGameAction;

        private float _endGameCounter;
        private float _endGameTime = 3f;
        private bool _isEndGame = false;

        public UIController(MainUIView mainUiView)
        {
            _endGameCounter = _endGameTime;
            _mainUiView = mainUiView;
            _mainUiView.Canvas.gameObject.SetActive(true);

            _menuView = _mainUiView.MenuView;
            _gameHudView = _mainUiView.GameHUDView;
            _endGameView = _mainUiView.EndGameView;

            _menuController = new MenuController(_menuView);
            _menuController.PlayButtonPressedEvent += PlayButtonPressed;
            _endGameController = new EndGameController(_endGameView);
        }

        public override void Update(float time)
        {
            if (_isEndGame)
            {
                EndGameTimer(time, EndGameAction);
            }
        }
        public void Dispose()
        {
            _menuController.PlayButtonPressedEvent -= PlayButtonPressed;
            _gameHudController.BadEndGameEvent -= ShowBadEndGameScreen;
            _gameHudController.GoodEndGameEvent -= ShowGoodEndGameScreen;
            EndGameAction -= BadEndGameAction;
            EndGameAction -= GoodEndGameAction;
            _endGameController.RestartButtonPressedEvent -= RestartGamePressed;
        }
        private void PlayButtonPressed()
        {
            DataController.GameState = GameState.GAME_PLAY;
            _gameHudController = new GameHUDController(_gameHudView);
            _gameHudController.BadEndGameEvent += ShowBadEndGameScreen;
            _gameHudController.GoodEndGameEvent += ShowGoodEndGameScreen;
        }
        private void ShowBadEndGameScreen()
        {
            _isEndGame = true;
            _gameHudView.gameObject.SetActive(false);
            EndGameAction += BadEndGameAction;
            _gameHudController.BadEndGameEvent -= ShowBadEndGameScreen;
        }
        private void BadEndGameAction()
        {
            _endGameController.ShowBadEndGame();
            _endGameController.RestartButtonPressedEvent += RestartGamePressed;
        }
        private void ShowGoodEndGameScreen()
        {
            _isEndGame = true;
            _gameHudView.gameObject.SetActive(false);
            EndGameAction += GoodEndGameAction;
            _gameHudController.GoodEndGameEvent -= ShowGoodEndGameScreen;
        }
        private void GoodEndGameAction()
        {
            _endGameController.ShowGoodEndGame();
            _endGameController.RestartButtonPressedEvent += RestartGamePressed;
        }
        private void RestartGamePressed()
        {
            _mainUiView.Canvas.gameObject.SetActive(false);
            _endGameView.gameObject.SetActive(false);
            _endGameView.BadEndPanelView.gameObject.SetActive(false);
            _endGameView.GoodEndPanelView.gameObject.SetActive(false);
            _endGameView.ContinuetButton.gameObject.SetActive(false);
            Dispose();
        }
        private void EndGameTimer(float time, Action action)
        {
            _endGameCounter -= time;
            if (_endGameCounter <= 0)
            {
                _endGameView.gameObject.SetActive(true);
                action?.Invoke();
                DataController.GameState = GameState.GAME_END;
                _endGameCounter = _endGameTime;
                _isEndGame = false;
            }
        }
    }
}