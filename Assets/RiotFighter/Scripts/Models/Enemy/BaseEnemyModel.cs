using Interfaces.Enemy;
using ScriptableObjects;

namespace Models.Enemy
{
    public class BaseEnemyModel : IEnemyModel
    {
        private float _maxHealth;
        private float _currentHealth;
        private float _moveSpeed;
        private float _damage;
        private float _hitScore;
        private bool _isMove;

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
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }
        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }
        public float HitScore
        {
            get => _hitScore;
            set => _hitScore = value;
        }
        public bool IsMove
        {
            get => _isMove;
            set => _isMove = value;
        }

        public BaseEnemyModel(EnemyModelData data)
        {
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
            _moveSpeed = data.MoveSpeed;
            _damage = data.Damage;
            _hitScore = data.HitScore;
            _isMove = data.IsMove;
        }
    }
}