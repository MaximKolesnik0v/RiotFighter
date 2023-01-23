using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScoreData", menuName = "ScriptableObjects/HUD/GameScoreData")]
    public class GameScoreData : ScriptableObject
    {
        [SerializeField] private float _score;
        [SerializeField] private float _enemyKillCount;

        public event Action EnemyKillCountChangeEvent;
        public event Action ScoreChangeEvent;

        public float Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChangeEvent?.Invoke();
            }
        }
        public float EnemyKillCount
        {
            get => _enemyKillCount;
            set
            {
                _enemyKillCount = value;
                EnemyKillCountChangeEvent?.Invoke();
            }
        }
        public void Reset()
        {
            Score = 0;
            EnemyKillCount = 0;
        }
    }
}