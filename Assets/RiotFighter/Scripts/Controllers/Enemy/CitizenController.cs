using Enums;
using Interfaces;
using Markers;
using Models.Enemy;
using ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using Views;
using Views.Enemy;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Controllers.Enemy
{
    public class CitizenController : BaseController
    {
        private CitizenView _view;
        private MolotovView _molotovView;
        private CitizenModel _model;
        private EnemyModelData _enemyModelData;
        private MaterialsData _materialsData;
        private EffectsData _effectsData;
        private GameScoreData _gameScoreData;
        private Rigidbody _rigidbody;
        private Random _rand;
        private Transform _molotovSpawn;
        private Transform _playerIsTarget;
        private GameObject _molotovPrefab;
        private GameObject _molotovGo;
        private GameObject _molotovExplosionEffectPrefab;
        private Vector3 _moveDirection;
        private Vector3 _toCarMoveDir;
        private CitizenState _citizenState = CitizenState.WALK;
        private MaterialType _currentMaterial;
        private float _moveCounter;
        private float _timeBetweenChangeMoveDirection = 2f;
        private float _molotovDropCounter;
        private float _molotovDropTime = 2f;
        private float _changeMaterialCounter;
        private float _changeMaterialTime = 3f;

        public CitizenModel Model
        {
            get => _model;
            set => _model = value;
        }

        public CitizenState CitizenState
        {
            get => _citizenState;
            set => _citizenState = value;
        }

        public MaterialType CurrentMaterial
        {
            get => _currentMaterial;
            set => _currentMaterial = value;
        }

        public CitizenController(IView view)
        {
            _playerIsTarget = Object.FindObjectOfType<CarView>().transform;
            _view = view as CitizenView;
            _molotovSpawn = _view.GetComponentInChildren<MolotovSpawnPositionMarker>().transform;
            _view.TakeWaterDamageEvent += TakeWaterDamage;

            var enemyData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.ENEMY_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as EnemyData;
            _molotovPrefab = enemyData.EnemyItems
                .Where(i => i.itemType == EnemyItemType.MOLOTOV)
                .Select(i => i.prefab)
                .FirstOrDefault();
            _enemyModelData = enemyData.Enemies
                .Where(i => i.type == EnemyType.CITIZEN)
                .Select(i => i.enemyModel)
                .FirstOrDefault();
            _effectsData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.EFFECTS_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as EffectsData;
            _molotovExplosionEffectPrefab = _effectsData.EffectList
                .Where(i => i.Type == EffectType.MOLOTOV_EXPLOSION)
                .Select(i => i.Prefab)
                .FirstOrDefault();
            _materialsData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.MATERIALS_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as MaterialsData;
            _gameScoreData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData;

            _model = new CitizenModel(_enemyModelData);
            _rand = new Random(DateTime.Now.Millisecond);
            _rigidbody = _view.GetComponent<Rigidbody>();

            _moveCounter = _timeBetweenChangeMoveDirection;
            _molotovDropCounter = _molotovDropTime;
            _changeMaterialCounter = _changeMaterialTime;

            _moveDirection = new Vector3(0, _rigidbody.velocity.y, _model.MoveSpeed);
        }

        public override void Update(float time)
        {
            GetToCarMoveDirection();
            CitizenAI(time);
        }

        private void CitizenAI(float time)
        {
            switch (_citizenState)
            {
                case CitizenState.WALK:
                    if (_model.IsMove && _model.CurrentHealth > 0)
                    {
                        ChangeDirectionTimer(time);
                        Move();
                    }
                    break;

                case CitizenState.MEET_PROVOCATEUR:
                    _model.IsMove = false;
                    _citizenState = CitizenState.WAIT_BEFORE_CHANGE;
                    break;

                case CitizenState.WAIT_BEFORE_CHANGE:
                    _changeMaterialCounter -= time;
                    if (_changeMaterialCounter <= 0)
                    {
                        _changeMaterialCounter = _changeMaterialTime;
                        _citizenState = CitizenState.CHANGE_TO_ENEMY;
                    }
                    break;

                case CitizenState.CHANGE_TO_ENEMY:
                    ChangeMaterial(MaterialType.CITIZEN_ENEMY_RED);
                    _citizenState = CitizenState.MOVE_TO_CAR;
                    break;

                case CitizenState.MOVE_TO_CAR:
                    _rigidbody.velocity = _toCarMoveDir * _model.MoveSpeed;
                    _citizenState = CitizenState.CHECK_CAR_DISTANCE;
                    break;

                case CitizenState.CHECK_CAR_DISTANCE:
                    var enemyPos = new Vector3(
                        _view.transform.position.x,
                        _view.transform.position.y + 1.5f,
                        _view.transform.position.z);
                    var ray = new Ray(enemyPos, _toCarMoveDir * 10f);
                    if (Physics.Raycast(ray, out RaycastHit hit, (_toCarMoveDir * 10f).magnitude))
                    {
                        var obj = hit.collider.gameObject.GetComponent<IInteractiveObject>();
                        if (obj is CarView)
                        {
                            _citizenState = CitizenState.WAIT_BEFORE_DROP_MOLOTOV;
                        }
                    } else
                    {
                        _citizenState = CitizenState.MOVE_TO_CAR;
                    }
                    break;

                case CitizenState.WAIT_BEFORE_DROP_MOLOTOV:
                    _molotovDropCounter -= time;
                    if (_molotovDropCounter <= 0)
                    {
                        _molotovDropCounter = _molotovDropTime;
                        _citizenState = CitizenState.DROP_MOLOTOV;
                    }
                    break;
                case CitizenState.DROP_MOLOTOV:
                    _molotovGo = Object.Instantiate(
                        _molotovPrefab,
                        _molotovSpawn.position,
                        Quaternion.identity,
                        DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);
                    _molotovView = _molotovGo.GetComponent<MolotovView>();
                    _molotovView.MolotoHitObjectEvent += MolotovHitObject;
                    _molotovGo.GetComponent<Rigidbody>().velocity = _toCarMoveDir * 50f;
                    _citizenState = CitizenState.CHECK_CAR_DISTANCE;
                    break;
                case CitizenState.DIE:
                    break;
                    
                case CitizenState.DEFENDED:
                    _model.IsMove = false;
                    _view.TakeWaterDamageEvent -= TakeWaterDamage;
                    break;

                case CitizenState.UNDEFENDED:
                    _view.TakeWaterDamageEvent += TakeWaterDamage;
                    _model.IsMove = true;
                    _citizenState = CitizenState.WALK;
                    break;

                default:
                    throw new Exception($"Не найден тип перечисления {nameof(_citizenState)}");
            }
        }

        private void Move()
        {
            _rigidbody.velocity = _view.transform.TransformDirection(_moveDirection);
        }

        public void ChangeMaterial(MaterialType materialType)
        {
            var newMaterial = _materialsData.Materials
                .Where(i => i.Type == materialType)
                .Select(i => i.Material)
                .FirstOrDefault();
            var meshRenderers = _view.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = newMaterial;
            }
        }

        private void MolotovHitObject(GameObject gameObj)
        {
            var interactiveObject = gameObj.GetComponent<IInteractiveObject>();
            if (interactiveObject is CarView carView)
            {
                var carController = (CarController)carView.Controller;
                var carModel = carController.Model;
                carModel.SetNewHealth(_model.Damage);
            }
            Vector3 hitPosition = _molotovGo.transform.position;
            _molotovView.MolotoHitObjectEvent -= MolotovHitObject;
            var molotovExplosionEffect = Object.Instantiate(
                _molotovExplosionEffectPrefab,
                hitPosition,
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);
            Object.Destroy(molotovExplosionEffect, 0.5f);
            Object.Destroy(_molotovGo);
        }

        private void TakeWaterDamage(IView waterView)
        {
            var waterController = (WaterController)waterView.Controller;
            var waterModel = waterController.Model;
            _model.CurrentHealth -= waterModel.Damage;
            _model.IsMove = false;
            _gameScoreData.Score += _model.HitScore;

            if (_model.CurrentHealth <= 0)
            {
                _gameScoreData.EnemyKillCount += 1;
                _view.TakeWaterDamageEvent -= TakeWaterDamage;

                if (_view.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.enabled = false;
                }
                _rigidbody.freezeRotation = false;
                _model.IsMove = false;
                _citizenState = CitizenState.DIE;
                ChangeMaterial(MaterialType.DEAD_ENEMY_GRAY);
            }
        }

        private void GetToCarMoveDirection()
        {
            _toCarMoveDir = (_playerIsTarget.position - _view.transform.position).normalized;
        }

        private void ChangeDirectionTimer(float time)
        {
            _moveCounter -= time;
            if (_moveCounter <= 0)
            {
                _moveCounter = _timeBetweenChangeMoveDirection;
                ChangeDirection();
            }
        }

        private void ChangeDirection(Vector3 newDirection = default)
        {
            _moveDirection = new Vector3(_rand.Next(-1, 2), _rigidbody.velocity.y, _rand.Next(-1, 2)) * _model.MoveSpeed;
        }
    }
}