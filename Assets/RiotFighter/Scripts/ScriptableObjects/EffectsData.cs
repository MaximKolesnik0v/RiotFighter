using Others;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/Effects/EffectsData")]
    public class EffectsData : ScriptableObject
    {
        [Header("Список эффектов (система частиц)")]
        [SerializeField] private List<EffectSetting> _effectList;

        public List<EffectSetting> EffectList
        {
            get => _effectList;
            set => _effectList = value;
        }
    }
}