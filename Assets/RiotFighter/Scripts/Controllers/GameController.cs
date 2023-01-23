using Constants;
using Controllers.Enemy;
using Controllers.UI;
using Enums;
using Others;
using ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.UI;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("UI Canvas")]
        [SerializeField] private MainUIView _mainUIView;

        [Header("Данные игры (ScriptableObjects)")]
        [SerializeField] private List<SoDataSetting> _soData;

        private CameraController _cameraController;
        private LevelController _levelController;
        private CarController _carController;
        private EnemiesController _enemiesController;
        private PolicemansController _policemansController;
        private UIController _uIController;

        void Start()
        {
            DataController.SoDataItems = _soData;
        }
        void Update()
        {
            switch(DataController.GameState)
            {
                case GameState.START_PRESETS:
                    InitGameObjectRoots();
                    InitControllers();

                    _carController.Update(Time.deltaTime);
                    _enemiesController.Update(Time.deltaTime);
                    _policemansController.Update(Time.deltaTime);
                    _uIController.Update(Time.deltaTime);
                    
                    ResetGameScore();
                    DataController.GameState = GameState.GAME_MENU;
                    break;
                case GameState.GAME_MENU:
                    break;
                case GameState.GAME_PLAY:
                    _carController.Update(Time.deltaTime);
                    _enemiesController.Update(Time.deltaTime);
                    _policemansController.Update(Time.deltaTime);
                    _uIController.Update(Time.deltaTime);
                    break;
                case GameState.GAME_END:
                    break;
                case GameState.RESTART_GAME:
                    ClearControllers();
                    DestroyGameObjects();
                    DataController.GameObjectRoots.Clear();
                    DataController.GameState = GameState.START_PRESETS;
                    break;
                default:
                    break;
            }
        }
        void FixedUpdate()
        {
            switch (DataController.GameState)
            {
                case GameState.GAME_PLAY:
                    _carController.FixedUpdate();
                    _enemiesController.FixedUpdate(Time.deltaTime);
                    _policemansController.FixedUpdate(Time.deltaTime);
                    break;
                default:
                    break;
            }
        }
        void LateUpdate()
        {
            switch (DataController.GameState)
            {
                case GameState.GAME_PLAY:
                    _cameraController.LateUpdate();
                    break;
                default:
                    break;
            }
        }
        private void InitControllers()
        {
            _levelController = new LevelController(LevelNumber.CURRENT_LEVEL);
            _carController = new CarController(_levelController.PLayerStartPosition);
            _enemiesController = new EnemiesController(_levelController.Enemies);
            _policemansController = new PolicemansController(_levelController.Policemans);
            _cameraController = new CameraController();
            _uIController = new UIController(_mainUIView);
        }
        private void ClearControllers()
        {
            _levelController = null;
            _carController = null;
            _enemiesController = null;
            _policemansController = null;
            _cameraController = null;
            _uIController = null;
        }
        private void InitGameObjectRoots()
        {
            DataController.GameObjectRoots.Add(GameObjectRoot.LEVEL_ROOT, new GameObject(GameConstants.LevelRoot).transform);
            DataController.GameObjectRoots.Add(GameObjectRoot.CAR_ROOT, new GameObject(GameConstants.CarRoot).transform);
            DataController.GameObjectRoots.Add(GameObjectRoot.WATER_ROOT, new GameObject(GameConstants.WaterRoot).transform);
            DataController.GameObjectRoots.Add(GameObjectRoot.GAMEITEMS_ROOT, new GameObject(GameConstants.GameItemsRoot).transform);
        }
        private void DestroyGameObjects()
        {
            foreach (var gameObject in DataController.GameObjectRoots)
            {
                Destroy(gameObject.Value.gameObject);
            }
        }
        private void ResetGameScore()
        {
            (DataController.SoDataItems
                .Where(i => i.type == SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData).Reset();
        }
    }
}


