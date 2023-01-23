using Controllers.Enemy;
using Enums;
using Interfaces;
using Interfaces.Enemy;
using UnityEngine;
using Views;
using Views.Enemy;

namespace Controllers
{
    public class PolicemanController : BaseController
    {
        private PolicemanView _view;
        private Rigidbody _rigidbody;
        private Transform _playerIsTarget;
        private bool _isMove = false;

        public PolicemanController(IView view)
        {
            _view = (PolicemanView)view;
            _view.AttackEnemyEvent += AttackEnemy;
            _rigidbody = _view.GetComponent<Rigidbody>();
            _playerIsTarget = Object.FindObjectOfType<CarView>().transform;
        }

        public override void Update(float time)
        {
            CheckDistance();
            Move(time);
        }

        public override void FixedUpdate(float time)
        {
        }

        private void CheckDistance()
        {
            var playerOffsetPos = new Vector3(
                _view.transform.position.x,
                _view.transform.position.y,
                _playerIsTarget.position.z);
            var distanceBetweenCar = Vector3.Distance(_view.transform.position, playerOffsetPos);

            _isMove = distanceBetweenCar >= 10f;
        }

        private void Move(float time)
        {
            if (_isMove)
            {
                _view.transform.Translate(Vector3.forward * 2 * time);
            }
        }

        private void AttackEnemy(IEnemyView view)
        {
            if (view is CitizenView citizenView)
            {
                var citizenController = (CitizenController)citizenView.Controller;
                var citizenModel = citizenController.Model;

                if (citizenView.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.enabled = false;
                }
                if (citizenView.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody.freezeRotation = false;
                    rigidbody.mass = 5f;
                }
                citizenModel.IsMove = false;
                citizenModel.CurrentHealth = 0;
                citizenController.CitizenState = CitizenState.DIE;
                citizenController.ChangeMaterial(MaterialType.DEAD_ENEMY_GRAY);
            }
        }
    }
}