using UnityEngine;

namespace Views.UI
{
    public class MainUIView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private GameHUDView _gameHUDView;
        [SerializeField] private EndGameView _endGameView;

        public Canvas Canvas
        {
            get => _canvas;
            set => _canvas = value;
        }
        public MenuView MenuView
        {
            get => _menuView;
            set => _menuView = value;
        }
        public GameHUDView GameHUDView
        {
            get => _gameHUDView;
            set => _gameHUDView = value;
        }
        public EndGameView EndGameView
        {
            get => _endGameView;
            set => _endGameView = value;
        }
    }
}