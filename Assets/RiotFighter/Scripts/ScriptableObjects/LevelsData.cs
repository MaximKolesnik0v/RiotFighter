using Others;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/Level/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        [Header("Список UI-префабов уровня")]
        [SerializeField] private List<LevelUISetting> _levelUIItems;

        [Header("Текущий уровень")]
        [SerializeField] private LevelSetting _currentLevel;

        [Header("Префабы уровней")]
        [SerializeField] private List<LevelSetting> _levelSetting;

        public List<LevelUISetting> LevelUIItems
        {
            get => _levelUIItems;
            set => _levelUIItems = value;
        }

        public LevelSetting CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }

        public List<LevelSetting> Levels
        {
            get => _levelSetting;
            set => _levelSetting = value;
        }
    }
}