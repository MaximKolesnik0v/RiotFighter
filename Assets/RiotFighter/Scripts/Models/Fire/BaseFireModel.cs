using Interfaces.Fire;
using ScriptableObjects;

namespace Models.Fire
{
    public class BaseFireModel : IFireModel
    {
        private float _maxHealth;
        private float _currentHealth;
        private float _hitScore;
        private float _percantagePlayerHealthRecovery;

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

        public BaseFireModel(FireModelData data)
        {
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
            _hitScore= data.HitScore;
            _percantagePlayerHealthRecovery = data.PercantagePlayerHealthRecovery;
        }
    }
}
