using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class GoodEndView : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        public Text ScoreText
        {
            get => _scoreText;
            set => _scoreText = value;
        }
    }
}