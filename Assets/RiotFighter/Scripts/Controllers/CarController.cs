using Enums;
using Markers;
using Models;
using Others;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class CarController : BaseController
    {
        private GameObject _car;
        private GameObject _muzzleGo;
        private CarView _view;
        private CarModel _model;
        private Transform _carExplosion;
        private Transform _carTower;
        private List<Transform> _carWheels;
        private List<Transform> _carDoors;
        private TowerController _towerController;
        private CarData _carData;
        private EffectsData _effectsData;
        private Rigidbody _rigidbody;
        private bool _isCarMove;
        private bool _isCarExplosion = false;
        private bool _isEndGame = false;
        private Vector3 _initPosition;
        private float _previousOffsetPosX;

        public Transform CarTransform => _car.transform;
        public CarModel Model
        {
            get => _model;
            set => _model = value;
        }
        public bool IsCarMove
        {
            get => _isCarMove;
            set => _isCarMove = value;
        }
        public bool IsEndGame
        {
            get => _isEndGame;
            set => _isEndGame = value;
        }

        public CarController(Vector3 playerPos)
        {
            _effectsData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.EFFECTS_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as EffectsData;
            _carData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.CAR_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as CarData;
            var levelData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.LEVEL_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as LevelsData;
            _model = new CarModel(_carData);
            _isCarMove = _carData.IsCarMove;

            Init(playerPos);

            _carExplosion = _view.GetComponentInChildren<CarExplosionPositionMarker>().transform;
            _towerController = new TowerController(_muzzleGo);
            _carTower = _view.GetComponentInChildren<CarTowerMarker>().transform;
            _carWheels = _view.GetComponentsInChildren<CarWheelMarker>().Select(i => i.transform).ToList();
            _carDoors = _view.GetComponentsInChildren<CarDoorMarker>().Select(i => i.transform).ToList();
        }

        public override void Update(float time)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isCarExplosion = true;
            }
            _towerController.Update(time);
        }

        public override void FixedUpdate(float time = 0)
        {
            MotorMove();
            _towerController.FixedUpdate();
            if (_isCarExplosion)
            {
                _isCarExplosion = false;
                CarExplosion();
            }
        }
        public void CarExplosion()
        {
            _isCarMove = false;
            var radius = 10f;
            var power = 150_000f;
            var explosionPos = _carExplosion.position;
            _carTower.GetComponent<Rigidbody>().isKinematic = false;
            foreach (var carWheel in _carWheels)
            {
                carWheel.GetComponent<Rigidbody>().isKinematic = false;
            }
            foreach (var carDoor in _carDoors)
            {
                carDoor.GetComponent<Rigidbody>().isKinematic = false;
            }
            var colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                }
            }
            var molotovExplosionEffectPrefab = _effectsData.EffectList
                .Where(i => i.Type == EffectType.CAR_EXPLOSION)
                .Select(i => i.Prefab)
                .FirstOrDefault();
            var molotovExplosionEffect = Object.Instantiate(
                molotovExplosionEffectPrefab,
                _carExplosion.position,
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);
            
            Object.Destroy(molotovExplosionEffect, 2f);
        }
        private void Init(Vector3 position)
        {
            _car = Object.Instantiate(_model.Prefab, position, Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.CAR_ROOT]);
            _initPosition = position;
            _view = _car.GetComponent<CarView>();
            _rigidbody = _car.GetComponent<Rigidbody>();
            _view.Controller = this;
            _muzzleGo = _car.GetComponentInChildren<CarMuzzleMarker>().gameObject;
        }
        private void Move(float time)
        {
            //float motor = _view.maxMotorTorque;

            //foreach (AxleInfo axleInfo in _view.axleInfos)
            //{
            //    axleInfo.leftWheel.motorTorque = motor;
            //    axleInfo.rightWheel.motorTorque = motor;
            //}
            //_rigidbody.velocity = Vector3.forward * _model.MoveSpeed * time;
            //_rigidbody.MovePosition(_car.transform.position * _model.MoveSpeed * time);
            //_car.transform.Translate(Vector3.forward * _model.MoveSpeed * time);
            MotorMove();

        }

        private void MotorMove()
        {
            _rigidbody.centerOfMass = _view.centerOfMass.localPosition;
            //float motor = _view.maxMotorTorque * Input.GetAxis("Vertical");
            float motor = _view.maxMotorTorque;
            //float steering = _view.maxSteeringAngle * Input.GetAxis("Horizontal");

            foreach (AxleInfo axleInfo in _view.axleInfos)
            {
                //if (axleInfo.steering)
                //{
                //    axleInfo.leftWheelCollider.steerAngle = steering;
                //    axleInfo.rightWheelCollider.steerAngle = steering;
                //}
                KeepLane(axleInfo);
                if (axleInfo.motor)
                {
                    axleInfo.leftWheelCollider.motorTorque = motor;
                    axleInfo.rightWheelCollider.motorTorque = motor;
                }
                SetWheelModel(axleInfo.leftWheelCollider, axleInfo.leftWheelModel);
                SetWheelModel(axleInfo.rightWheelCollider, axleInfo.rightWheelModel);
            }
        }
        private void SetWheelModel(WheelCollider collider, Transform model)
        {
            Vector3 pos;
            Quaternion rot;
            collider.GetWorldPose(out pos, out rot);

            model.transform.position = pos;
            model.transform.rotation = rot;
        }

        private void KeepLane(AxleInfo axleInfo)
        {
            if (axleInfo.steering)
            {
                float currentOffsetPosX;
                float offsetCoef = 8f;
                float rateCoef = 1000f;
                currentOffsetPosX = _view.transform.position.x - _initPosition.x;
                var rateOffset = _previousOffsetPosX - currentOffsetPosX;
                _previousOffsetPosX = currentOffsetPosX;
                var steerAngle = -currentOffsetPosX * offsetCoef + rateOffset * rateCoef;
                if (Math.Abs(steerAngle) > _view.maxSteeringAngle)
                {
                    steerAngle = _view.maxSteeringAngle * Math.Sign(steerAngle);
                }
                axleInfo.leftWheelCollider.steerAngle = steerAngle;
                axleInfo.rightWheelCollider.steerAngle = steerAngle;
                currentOffsetPosX = 0f;
            }
        }
    }
}