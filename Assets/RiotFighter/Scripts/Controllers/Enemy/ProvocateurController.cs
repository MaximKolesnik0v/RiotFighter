using Enums;
using Interfaces;
using Models.Enemy;
using ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Enemy;
using Views.GameItem;
using Object = UnityEngine.Object;

namespace Controllers.Enemy
{
    public class ProvocateurController : BaseController
    {
        private ProvocateurView _view;
        private ProvocateurModel _model;
        private EnemyData _enemyData;
        private MaterialsData _materialsData;
        private GameScoreData _gameScoreData;
        private GameObject _activeZone;
        private ActiveZoneView _activeZoneView;
        private GameObject _namePlate;
        private Rigidbody _rigidbody;
        private Vector3 _activeZonePosition;
        private Dictionary<int, IView> _citizensList = new Dictionary<int, IView>();

        public ProvocateurController(IView view)
        {
            _view = view as ProvocateurView;
            _view.TakeWaterDamageEvent += TakeWaterDamage;
            _rigidbody = _view.GetComponent<Rigidbody>();
            _enemyData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.ENEMY_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as EnemyData;
            var enemyModelData = _enemyData.Enemies
                .Where(i => i.type == EnemyType.PROVOCATEUR)
                .Select(i => i.enemyModel)
                .FirstOrDefault();
            _materialsData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.MATERIALS_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as MaterialsData;
            _gameScoreData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData;

            _model = new ProvocateurModel(enemyModelData);
            InitGameItems();
        }

        public override void Update(float time)
        {
            _activeZonePosition = new Vector3(_view.transform.position.x, 0, _view.transform.position.z);
            _activeZone.transform.position = _activeZonePosition;
        }

        private void InitGameItems()
        {
            var activeZone = _enemyData.EnemyItems
                .Where(i => i.itemType == EnemyItemType.ACTIVE_ZONE)
                .Select(i => i.prefab)
                .FirstOrDefault();

            _activeZone = Object.Instantiate(
                activeZone, 
                _view.transform.position, 
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);

            _activeZoneView = _activeZone.GetComponentInChildren<ActiveZoneView>();
            _activeZoneView.ProvokeCitizenEvent += ProvokeCitizen;
            _namePlate = _view.GetComponentInChildren<NamePlateView>().gameObject;
        }

        private void TakeWaterDamage(IView waterView)
        {
            var waterController = (WaterController)waterView.Controller;
            var waterModel = waterController.Model;

            _model.CurrentHealth -= waterModel.Damage;
            _gameScoreData.Score += _model.HitScore;
            
            if (_model.CurrentHealth <= 0)
            {
                _gameScoreData.EnemyKillCount += 1;
                _view.TakeWaterDamageEvent -= TakeWaterDamage;
                _activeZoneView.ProvokeCitizenEvent -= ProvokeCitizen;
                _view.GetComponent<Animator>().enabled = false;
                _activeZone.SetActive(false);
                _rigidbody.freezeRotation = false;
                _namePlate.GetComponent<Rigidbody>().isKinematic = false;

                ChangeMaterial(MaterialType.DEAD_ENEMY_GRAY);
                foreach (var citizen in _citizensList.Values)
                {
                    var view = (CitizenView)citizen;
                    var controller = (CitizenController)view.Controller;
                    var model = controller.Model;

                    if (model.CurrentHealth > 0)
                    {
                        model.IsMove = true;
                        controller.CitizenState = CitizenState.WALK;
                        controller.ChangeMaterial(MaterialType.CITIZEN_WHITE);
                    }
                }
            }
        }

        private void ProvokeCitizen(IView view)
        {
            var citizenView = (CitizenView)view;
            var citizenController = (CitizenController)citizenView.Controller;
            var citizenModel = citizenController.Model;
            
            if (citizenModel.CurrentHealth > 0)
            {
                if (!_citizensList.ContainsKey(citizenView.gameObject.GetInstanceID()))
                {
                    _citizensList.Add(citizenView.gameObject.GetInstanceID(), citizenView);
                }
                citizenController.CitizenState = CitizenState.MEET_PROVOCATEUR;
            }
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
    }
}