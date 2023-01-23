using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private GoodEndView _goodEndPanelView;
        [SerializeField] private BadEndView _badEndPanelView;

        public Button RestartButton
        {
            get => _restartButton;
            set => _restartButton = value;
        }
        public Button ContinuetButton
        {
            get => _continueButton;
            set => _continueButton = value;
        }
        public GoodEndView GoodEndPanelView
        {
            get => _goodEndPanelView;
            set => _goodEndPanelView = value;
        }
        public BadEndView BadEndPanelView
        {
            get => _badEndPanelView;
            set => _badEndPanelView = value;
        }
    }
}