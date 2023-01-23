using Controllers;
using Interfaces;
using Interfaces.Enemy;
using System;
using UnityEngine;
using Views.Enemy;

namespace Views
{
    public class PolicemanView : BaseView, IPolicemanView
    {
        private PolicemanController _controller;
        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new PolicemanController(this);
                }
                return _controller;
            }
            set => _controller = value as PolicemanController;
        }

        public event Action<IEnemyView> AttackEnemyEvent;

        //void OnCollisionEnter(Collision collision)
        //{
        //    var interactiveObject = collision.gameObject.GetComponent<IInteractiveObject>();

        //    if (interactiveObject is IEnemyView enemyView)
        //    {
        //        AttackEnemyEvent?.Invoke(enemyView);
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            var interactiveObject = other.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is IEnemyView enemyView)
            {
                AttackEnemyEvent?.Invoke(enemyView);
            }
        }
    }
}