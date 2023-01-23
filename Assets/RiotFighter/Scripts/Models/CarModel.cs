using ScriptableObjects;
using System;
using UnityEngine;

namespace Models
{
    public class CarModel
    {
        private GameObject _prefab;
        private float _moveSpeed;
        private float _maxHealth;
        private float _currentHealth;

        public event Action DeathEvent;
        public event Action<float> ChangeHealthEvent;

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

        public CarModel(CarData data)
        {
            _prefab = data.Prefab;
            _moveSpeed = data.MoveSpeed;
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
        }

        public void SetNewHealth(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth > 0)
            {
                ChangeHealthEvent?.Invoke(damage);
            } else
            {
                DeathEvent?.Invoke();
            }
        }
    }
}
