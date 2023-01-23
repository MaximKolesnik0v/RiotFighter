using Models;
using ScriptableObjects;
using System;
using System.Linq;
using Views;
using Views.UI;
using Object = UnityEngine.Object;

namespace Controllers.UI
{
    public class GameHUDController : BaseController, IDisposable
    {
        private GameHUDView _gameHudView;
        private CarView _carView;
        private CarModel _carModel;
        private CarController _carController;
        private GameScoreData _gameScoreData;

        public event Action BadEndGameEvent;
        public event Action GoodEndGameEvent;

        public GameHUDController(GameHUDView gameHubView)
        {
            _gameHudView = gameHubView;
            _gameHudView.gameObject.SetActive(true);
            _carView = Object.FindObjectOfType<CarView>();
            _carController = (CarController)_carView.Controller;
            _carModel = _carController.Model;

            _carView.LevelEndTriggerEvent += LevelEndTrigger;
            _carModel.ChangeHealthEvent += ChangeHealth;
            _carModel.DeathEvent += Death;

            ChangeHealth(0f);

            _gameScoreData = DataController.SoDataItems
                .Where(i => i.type == Enums.SoDataType.GAME_SCORE_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as GameScoreData;
            _gameScoreData.ScoreChangeEvent += ScoreChange;
            _gameScoreData.EnemyKillCountChangeEvent += EnemyKillCountChange;
        }
        public void Dispose()
        {
            _carView.LevelEndTriggerEvent -= LevelEndTrigger;
            _carModel.ChangeHealthEvent -= ChangeHealth;
            _carModel.DeathEvent -= Death;
            _gameScoreData.ScoreChangeEvent -= ScoreChange;
            _gameScoreData.EnemyKillCountChangeEvent -= EnemyKillCountChange;
        }
        private void ChangeHealth(float damage)
        {
            _gameHudView.HealthSlider.fillAmount = (_carModel.CurrentHealth / _carModel.MaxHealth);
            _gameHudView.HealthValue.text = _carModel.CurrentHealth.ToString();
        }
        private void Death()
        {
            ChangeHealth(0f);
            _carController.CarExplosion();
            BadEndGameEvent?.Invoke();
            _carController.IsEndGame = true;
            Dispose();
        }
        private void LevelEndTrigger()
        {
            _carController.IsCarMove = false;
            GoodEndGameEvent?.Invoke();
            _carController.IsEndGame = true;
            Dispose();
        }
        private void ScoreChange()
        {
            _gameHudView.ScoreCount.text = $"Набрано очков: {_gameScoreData.Score}";
        }
        private void EnemyKillCountChange()
        {
            _gameHudView.EnemyKillCount.text = $"Ликвидировано: {_gameScoreData.EnemyKillCount}";
        }
    }
}