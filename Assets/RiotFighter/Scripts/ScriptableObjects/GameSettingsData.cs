using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettingsData", menuName = "ScriptableObjects/GameSettings/GameSettingsData")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField] private LevelNumber _levelNumber;

        public LevelNumber LevelNumber
        {
            get => _levelNumber;
            set => _levelNumber = value;
        }
    }
}