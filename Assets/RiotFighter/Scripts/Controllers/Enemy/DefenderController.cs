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
    public class DefenderController : BaseController
    {
        private DefenderView _view;
        private DefenderModel _model;
        private EnemyData _enemyData;
        private MaterialsData _materialsData;
        private GameScoreData _gameScoreData;
        private GameObject _defendedZone;
        private DefendedZoneView _defendedZoneView;
        private GameObject _shield;
        private Rigidbody _rigidbody;
        private Vector3 _defendedZonePosition;
        private Dictionary<int, IView> _citizensList = new Dictionary<int, IView>();

        public DefenderController(IView view)
        {
            _view = view as DefenderView;
            _view.TakeWaterDamageEvent += TakeWaterDamage;
            _rigidbody = _view.GetComponent<Rigidbody>();
            _enemyData = DataController.SoDataItems
                .Where(i => i.type == SoDataType.ENEMY_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as EnemyData;
            var enemyModelData = _enemyData.Enemies
                .Where(i => i.type == EnemyType.DEFENDER)
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

            _model = new DefenderModel(enemyModelData);
            InitGameItems();
        }

        public override void Update(float time)
        {
            var defendedZoneOffset = _view.transform.position.z + 5;
            _defendedZonePosition = new Vector3(_view.transform.position.x, 0, defendedZoneOffset);
            _defendedZone.transform.position = _defendedZonePosition;
        }

        private void InitGameItems()
        {
            var defendedZone = _enemyData.EnemyItems
                .Where(i => i.itemType == EnemyItemType.DEFENDED_ZONE)
                .Select(i => i.prefab)
                .FirstOrDefault();

            _defendedZone = Object.Instantiate(
                defendedZone,
                _view.transform.position,
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);

            _defendedZoneView = _defendedZone.GetComponentInChildren<DefendedZoneView>();
            _defendedZoneView.DefendCitizenEvent += DefendCitizen;
            _defendedZoneView.UndefendCitizenEvent += UndefendCitizen;
            _shield = _view.GetComponentInChildren<ShieldView>().gameObject;
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
                _defendedZoneView.DefendCitizenEvent -= DefendCitizen;
                _defendedZoneView.UndefendCitizenEvent -= UndefendCitizen;
                _view.GetComponent<Animator>().enabled = false;
                _defendedZone.SetActive(false);
                _rigidbody.freezeRotation = false;
                _shield.GetComponent<Rigidbody>().isKinematic = false;

                ChangeMaterial(MaterialType.DEAD_ENEMY_GRAY);
                foreach (var citizen in _citizensList.Values)
                {
                    var citizenView = (CitizenView)citizen;
                    var citizenController = (CitizenController)citizenView.Controller;
                    var citizenModel = citizenController.Model;

                    if (citizenModel.CurrentHealth > 0)
                    {
                        citizenController.CitizenState = CitizenState.UNDEFENDED;
                        citizenController.ChangeMaterial(MaterialType.CITIZEN_WHITE);
                    }
                }
            }
        }

        private void DefendCitizen(IView view)
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
                citizenController.CitizenState = CitizenState.DEFENDED;
                citizenController.ChangeMaterial(MaterialType.DEFENDER_ENEMY_YELLOW);
            }
        }

        private void UndefendCitizen(IView view)
        {
            var citizenView = (CitizenView)view;
            var citizenController = (CitizenController)citizenView.Controller;
            citizenController.CitizenState = CitizenState.UNDEFENDED;
            citizenController.ChangeMaterial(MaterialType.CITIZEN_WHITE);
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