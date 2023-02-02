using Enums;
using Interfaces;
using Markers;
using Models.Enemy;
using ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers.Enemy
{
    public class AnarhistController : BaseController
    {
        private AudioController _audioController; 
        private AnarhistView _view;
        private MolotovView _molotovView;
        private AnarhistModel _model;
        private EnemyModelData _enemyModelData;
        private MaterialsData _materialsData;
        private EffectsData _effectsData;
        private GameScoreData _gameScoreData;
        private Rigidbody _rigidbody;
        private Transform _molotovSpawn;
        private Transform _playerIsTarget;
        private GameObject _molotovPrefab;
        private GameObject _molotovGo;
        private GameObject _molotovExplosionEffectPrefab;
        private Vector3 _toCarMoveDir;
        private Vector3 _fromCarMoveDir;
        private CitizenState _citizenState = CitizenState.CHECK_CAR_DISTANCE;
        private float _molotovDropCounter;
        private float _molotovDropTime = 1f;
        private bool _isFirstAttack = true;

        public AnarhistController(IView view)
        {
            _audioController = new AudioController();
            _playerIsTarget = Object.FindObjectOfType<CarView>().transform;
            _view = view as AnarhistView;
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
                .Where(i => i.type == EnemyType.ANARHIST)
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

            _model = new AnarhistModel(_enemyModelData);
            if (_view.IsSetCustomHealth)
            {
                _model.CurrentHealth = _view.Health;
            }
            _rigidbody = _view.GetComponent<Rigidbody>();
            _molotovDropCounter = _molotovDropTime;
        }

        public override void Update(float time)
        {
            GetToCarMoveDirection();
            GetFromCarMoveDirection();
            AnarhistAI(time);
        }

        private void AnarhistAI(float time)
        {
            switch (_citizenState)
            {
                case CitizenState.CHECK_CAR_DISTANCE:
                    Vector3 hit;
                    if (_isFirstAttack)
                    {
                        hit = CheckRaycastHit(_view.FirstAttackDistanceToCheckCar);
                    }
                    else
                    {
                        hit = CheckRaycastHit(_view.DefaultDistanceToCheckCar);
                    }
                    var distance = Vector3.Distance(_view.transform.position, hit);

                    if (hit == Vector3.zero)
                    {
                        _citizenState = CitizenState.MOVE_TO_CAR;
                    }
                    else
                    {
                        if (distance < _view.DefaultDistanceToCheckCar)
                        {
                            _isFirstAttack = false;
                            _citizenState = CitizenState.MOVE_FROM_CAR;
                        }
                        else
                        {
                            _citizenState = CitizenState.WAIT_BEFORE_DROP_MOLOTOV;
                        }
                    }

                    break;

                case CitizenState.MOVE_TO_CAR:
                    if (_view.IsMove)
                    {
                        _rigidbody.velocity = _toCarMoveDir * _model.MoveSpeed;
                    }
                    _citizenState = CitizenState.CHECK_CAR_DISTANCE;
                    break;

                case CitizenState.MOVE_FROM_CAR:
                    if (_view.IsMove)
                    {
                        _rigidbody.velocity = _fromCarMoveDir * _model.MoveSpeed;
                    }
                    if (Vector3.Distance(_view.transform.position, _playerIsTarget.position) > _view.DefaultDistanceToCheckCar)
                    {

                        _citizenState = CitizenState.CHECK_CAR_DISTANCE;
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
                    _audioController.PlaySound(AudioClipType.MOLOTOV_DROP);
                    if (_model.IsDefended)
                    {
                        _citizenState = CitizenState.WAIT_BEFORE_DROP_MOLOTOV;
                    }
                    else
                    {
                        _citizenState = CitizenState.CHECK_CAR_DISTANCE;
                    }
                    break;
                case CitizenState.DIE:
                    break;

                default:
                    throw new Exception($"Не найден тип перечисления {nameof(_citizenState)}");
            }
        }

        private Vector3 CheckRaycastHit(float rayLength)
        {
            RaycastHit hit;
            float offsetToHead = 2.13f;
            var enemyPos = new Vector3(
                _view.transform.position.x,
                _view.transform.position.y + offsetToHead,
                _view.transform.position.z);
            var ray = new Ray(enemyPos, _toCarMoveDir * rayLength);
            //Debug.DrawRay(enemyPos, _toCarMoveDir * rayLength, Color.magenta);
            var layerMask = 1 << 11;
            if (Physics.Raycast(ray, out hit, (_toCarMoveDir * rayLength).magnitude, layerMask))
            {
                var obj = GetInteractiveObject(hit.collider.gameObject);
                if (obj is CarView carView)
                {
                    return hit.transform.position;
                }
            }
            return Vector3.zero;
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

        public void Death()
        {
            _gameScoreData.EnemyKillCount += 1;
            _view.TakeWaterDamageEvent -= TakeWaterDamage;
            _audioController.PlaySound(AudioClipType.ENEMY_DEATH);

            if (_view.TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = false;
            }
            _rigidbody.freezeRotation = false;
            _model.IsMove = false;
            _citizenState = CitizenState.DIE;
            ChangeMaterial(MaterialType.DEAD_ENEMY_GRAY);
        }

        private void MolotovHitObject(GameObject gameObj)
        {
            var interactiveObject = gameObj.GetComponent<IInteractiveObject>();
            if (interactiveObject is CarView carView)
            {
                var carController = (CarController)carView.Controller;
                var carModel = carController.Model;
                carModel.SetNewHealth(-_model.Damage);
            }

            Vector3 hitPosition = _molotovGo.transform.position;
            _molotovView.MolotoHitObjectEvent -= MolotovHitObject;

            var molotovExplosionEffect = Object.Instantiate(
                _molotovExplosionEffectPrefab,
                hitPosition,
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);
            _audioController.PlaySound(AudioClipType.MOLOTOV_EXPLOSION);
            Object.Destroy(molotovExplosionEffect, 0.5f);
            Object.Destroy(_molotovGo);
        }

        private void TakeWaterDamage(IView waterView)
        {
            var waterController = (WaterController)waterView.Controller;
            var waterModel = waterController.Model;
            _model.CurrentHealth -= waterModel.Damage;
            _view.IsMove = false;
            _gameScoreData.Score += _model.HitScore;

            if (_model.CurrentHealth <= 0)
            {
                Death();
            }
        }

        private IInteractiveObject GetInteractiveObject(GameObject obj)
        {
            var interactiveObject = obj.GetComponentInParent<IInteractiveObject>();
            if (interactiveObject == null)
            {
                interactiveObject = obj.GetComponentInChildren<IInteractiveObject>();
            }
            return interactiveObject;
        }

        private void GetToCarMoveDirection()
        {
            _toCarMoveDir = (_playerIsTarget.position - _view.transform.position).normalized;
        }

        private void GetFromCarMoveDirection()
        {
            _fromCarMoveDir = (_view.transform.position - _playerIsTarget.position).normalized;
        }
    }
}
