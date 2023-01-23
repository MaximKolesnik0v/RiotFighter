using Controllers.Enemy;
using Interfaces;
using Interfaces.Enemy;
using System;
using UnityEngine;

namespace Views.Enemy
{
    public class ProvocateurView : BaseView, IEnemyView
    {
        private ProvocateurController _controller;

        public event Action<IView> TakeWaterDamageEvent;

        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new ProvocateurController(this);
                }
                return _controller;
            }
            set => _controller = value as ProvocateurController;
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