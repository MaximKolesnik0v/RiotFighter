using Others;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Игровые элементы")]
        [SerializeField] private List<EnemyItems> _enemyItems;

        [Header("Префабы и данные моделей врагов")]
        [SerializeField] private List<EnemySetting> _enemySetting;

        public List<EnemyItems> EnemyItems
        {
            get => _enemyItems;
            set => _enemyItems = value;
        }

        public List<EnemySetting> Enemies
        {
            get => _enemySetting;
            set => _enemySetting = value;
        }
    }
}