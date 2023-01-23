using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class GameHUDView : MonoBehaviour
    {
        [SerializeField] private Image _healthSlider;
        [SerializeField] private Text _healthValue;
        [SerializeField] private Text _enemyKillCount;
        [SerializeField] private Text _scoreCount;

        public Image HealthSlider
        {
            get => _healthSlider;
            set => _healthSlider = value;
        }
        public Text HealthValue
        {
            get => _healthValue;
            set => _healthValue = value;
        }
        public Text EnemyKillCount
        {
            get => _enemyKillCount;
            set => _enemyKillCount = value;
        }
        public Text ScoreCount
        {
            get => _scoreCount;
            set => _scoreCount = value;
        }
    }
}