using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        public Button StartButton
        {
            get => _startButton;
            set => _startButton = value;
        }
    }
}