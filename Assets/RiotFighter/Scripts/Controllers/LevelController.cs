using Enums;
using Interfaces;
using Interfaces.Enemy;
using Markers;
using ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class LevelController : BaseController
    {
        private LevelsData _levelsData;
        private Vector3 _playerStartPosition;
        private LevelNumber _levelNumber;
        private List<IView> _enemies = new List<IView>();
        private List<IView> _policemans = new List<IView>();

        public Vector3 PLayerStartPosition => _playerStartPosition;
        public List<IView> Enemies => _enemies;
        public List<IView> Policemans => _policemans;

        public LevelController(LevelNumber levelNumber)
        {
            _levelsData = DataController.SoDataItems
                    .Where(i => i.type == SoDataType.LEVEL_DATA)
                    .Select(i => i.data)
                    .FirstOrDefault() as LevelsData;
            _levelNumber = levelNumber;
            Init();
        }

        private void Init()
        {
            //var levelPrefab = _levelsData.Levels
            //    .Where(i => i.LevelNumber == _levelNumber)
            //    .Select(i => i.Prefab)
            //    .FirstOrDefault();
            var levelPrefab = _levelsData.CurrentLevel.Prefab;

            var level = Object.Instantiate(
                levelPrefab, 
                Vector3.zero, 
                Quaternion.identity,
                DataController.GameObjectRoots[GameObjectRoot.LEVEL_ROOT]);
            _playerStartPosition = level.GetComponentInChildren<PlayerStartPositionMarker>().transform.position;

            var views = level.GetComponentsInChildren<IView>().ToList();
            foreach (var view in views)
            {
                if (view is IEnemyView)
                {
                    _enemies.Add(view);
                }
                if (view is IPolicemanView)
                {
                    _policemans.Add(view);
                }
            }
        }
    }
}