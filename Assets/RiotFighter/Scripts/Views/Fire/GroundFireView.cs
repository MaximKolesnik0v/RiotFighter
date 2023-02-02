using Controllers.Fire;
using Interfaces;
using Interfaces.Fire;
using System;
using UnityEngine;

namespace Views.Enemy
{
    public class GroundFireView : BaseView, IFireView
    {
        private GroundFireController _controller;

        [Header("Установить кастомное здоровье?")]
        [SerializeField] private bool _isSetCustomHealth = false;

        [Header("Значение кастомного здоровья")]
        [SerializeField] private float _customHealth = 50f;

        [Header("Установить кастомный процент восстанавливаемого здоровья игрока?")]
        [SerializeField] private bool _isSetCustomPercantagePlayerHealthRecovery = false;

        [Header("Процент восстанавливаемого здоровья игрока"), Range(0, 100)]
        [SerializeField] private float _customPercantagePlayerHealthRecovery = 1f;

        public event Action<IView> TakeWaterDamageEvent;

        public bool IsSetCustomHealth
        {
            get => _isSetCustomHealth;
            set => _isSetCustomHealth = value;
        }
        public float CustomHealth
        {
            get => _customHealth;
            set => _customHealth = value;
        }

        public bool IsSetCustomPercantagePlayerHealthRecovery
        {
            get => _isSetCustomPercantagePlayerHealthRecovery;
            set => _isSetCustomPercantagePlayerHealthRecovery = value;
        }

        public float CustomPercantagePlayerHealthRecovery
        {
            get => _customPercantagePlayerHealthRecovery;
            set => _customPercantagePlayerHealthRecovery = value;
        }

        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new GroundFireController(this);
                }
                return _controller;
            }
            set => _controller = value as GroundFireController;
        }

        private void OnTriggerEnter(Collider collider)
        {
            var interactiveObject = collider.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is WaterView waterView)
            {
                TakeWaterDamageEvent?.Invoke(waterView);
            }
        }
    }
}