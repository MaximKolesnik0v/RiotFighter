using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/Car/CarData")]
    public class CarData : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private bool _isCarMove;
        
        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }
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
        public bool IsCarMove
        {
            get => _isCarMove;
            set => _isCarMove = value;
        }
    }
}