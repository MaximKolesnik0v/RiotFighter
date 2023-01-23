using Controllers.Enemy;
using Interfaces;
using Interfaces.Enemy;
using System;
using UnityEngine;

namespace Views.Enemy
{
    public class DefenderView : BaseView, IEnemyView
    {
        private DefenderController _controller;

        public event Action<IView> TakeWaterDamageEvent;

        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new DefenderController(this);
                }
                return _controller;
            }
            set => _controller = value as DefenderController;
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