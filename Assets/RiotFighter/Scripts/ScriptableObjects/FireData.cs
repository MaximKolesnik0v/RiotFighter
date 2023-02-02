using Serializables;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "FireData", menuName = "ScriptableObjects/Fire/FireData")]
    public class FireData : ScriptableObject
    {
        [Header("Префабы и данные моделей огня")]
        [SerializeField] private List<FireSettings> _fireSettings;

        public List<FireSettings> Fires
        {
            get => _fireSettings;
            set => _fireSettings = value;
        }
    }
}
