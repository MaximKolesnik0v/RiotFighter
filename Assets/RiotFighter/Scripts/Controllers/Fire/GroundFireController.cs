using Enums;
using Enums.Enemy;
using Enums.Fire;
using Interfaces;
using Models.Enemy;
using ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using Views;
using Views.Enemy;

namespace Controllers.Fire
{
    public class GroundFireController : BaseController
    {
        private FireData _fireData;
        private GameScoreData _gameScoreData;

        private FireModel _model;
        private GroundFireView _view;
        private FireState _fireState = FireState.FIRE;
        private Vector3 _startingTransformScale;
        private Vector3 _tempScale;
        private float _reduceMultiplayerOfTransformScale = 0.98f;

        private AudioController _audioController;

        private CarView _carView;

        public GroundFireController(IView view)
        {
            _audioController = new AudioController();
            _view = view as GroundFireView;
            _startingTransformScale = _view.transform.localScale;
            _view.TakeWaterDamageEvent += TakeWaterDamage;
            _fireData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.FIRE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as FireData;
            var fireModelData = _fireData.Fires
                .Where(i => i.type == FireType.Ground)
                .Select(i => i.fireModel)
                .FirstOrDefault();
            _gameScoreData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData;

            _model = new FireModel(fireModelData);
            _carView = UnityEngine.Object.FindObjectOfType<CarView>();

            if (_view.IsSetCustomHealth)
            {
                _model.CurrentHealth = _view.CustomHealth;
            }

            if (_view.IsSetCustomPercantagePlayerHealthRecovery)
            {
                _model.PercantagePlayerHealthRecovery = _view.CustomPercantagePlayerHealthRecovery;
            }
        }

        private void TakeWaterDamage(IView waterView)
        {
            var waterController = (WaterController)waterView.Controller;
            var waterModel = waterController.Model;

            _model.CurrentHealth -= waterModel.Damage;
            _gameScoreData.Score += _model.HitScore;

            if (_view.transform.localScale == _startingTransformScale)
            {
                _view.transform.localScale = _reduceMultiplayerOfTransformScale * _startingTransformScale;
                _tempScale= _view.transform.localScale;
            }
            else
            {
                _view.transform.localScale = _reduceMultiplayerOfTransformScale * _tempScale;
                _tempScale = _view.transform.localScale;
            }
        }

        public override void Update(float time)
        {
            switch (_fireState)
            {
                case FireState.FIRE:
                    if (_model.CurrentHealth < 0)
                    {
                        _fireState = FireState.DIE;
                    }

                break;

                case FireState.DIE:
                    if (_view.gameObject.activeSelf)
                    {
                        _view.gameObject.SetActive(false);
                        _view.TakeWaterDamageEvent -= TakeWaterDamage;
                        RecoverCarHealth(_carView);
                    }
                break;

                default:
                    throw new Exception($"Не найден тип перечисления {nameof(_fireState)}");
            }
        }

        private void RecoverCarHealth(IView carView)
        {
            var carController = (CarController)carView.Controller;
            var carModel = carController.Model;

            if (carModel.CurrentHealth > 0 && carModel.CurrentHealth < carModel.MaxHealth)
            { 
                var healthRecovered = carModel.MaxHealth * _model.PercantagePlayerHealthRecovery / 100;
                carModel.SetNewHealth(healthRecovered);
            }
        }
    }
}
