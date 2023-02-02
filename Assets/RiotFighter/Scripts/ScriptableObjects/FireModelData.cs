using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "FireModelData", menuName = "ScriptableObjects/Fire/FireModelData")]
    public class FireModelData : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _hitScore;
        [SerializeField] private float _percantagePlayerHealthRecovery;

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public float HitScore
        {
            get => _hitScore;
            set => _hitScore = value;
        }

        public float PercantagePlayerHealthRecovery
        {
            get => _percantagePlayerHealthRecovery;
            set => _percantagePlayerHealthRecovery = value;
        }
    }
}
