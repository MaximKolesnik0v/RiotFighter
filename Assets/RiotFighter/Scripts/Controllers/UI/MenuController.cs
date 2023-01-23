using Enums;
using System;
using UnityEngine;
using Views.UI;

namespace Controllers.UI
{
    public class MenuController : IDisposable
    {
        private MenuView _menuView;

        public event Action PlayButtonPressedEvent;

        public MenuController(MenuView menuView)
        {
            _menuView = menuView;
            _menuView.gameObject.SetActive(true);
            _menuView.StartButton.onClick.AddListener(StartButtonClick);
        }

        public void Dispose()
        {
            _menuView.StartButton.onClick.RemoveAllListeners();
        }

        private void StartButtonClick()
        {
            DataController.GameState = GameState.GAME_PLAY;
            _menuView.gameObject.SetActive(false);
            PlayButtonPressedEvent?.Invoke();
        }
    }
}