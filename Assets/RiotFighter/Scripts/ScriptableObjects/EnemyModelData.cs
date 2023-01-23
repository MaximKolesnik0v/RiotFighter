using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyModelData", menuName = "ScriptableObjects/Enemy/EnemyModelData")]
    public class EnemyModelData : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _damage;
        [SerializeField] private float _hitScore;
        [SerializeField] private bool _isMove;
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
    }
}