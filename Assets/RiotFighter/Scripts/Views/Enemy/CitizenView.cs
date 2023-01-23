using Controllers.Enemy;
using Interfaces;
using Interfaces.Enemy;
using System;
using UnityEngine;

namespace Views.Enemy
{
    public class CitizenView : BaseView, IEnemyView
    {
        private CitizenController _controller;

        public event Action<IView> TakeWaterDamageEvent;

        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new CitizenController(this);
                }
                return _controller;
            }
            set => _controller = value as CitizenController;
        }

        void OnCollisionEnter(Collision collision)
        {
            var interactiveObject = collision.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is WaterView waterView)
            {
                TakeWaterDamageEvent?.Invoke(waterView);
            }
        }
    }
}