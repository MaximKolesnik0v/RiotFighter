using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WaterJetData", menuName = "ScriptableObjects/WaterJet/WaterJetData")]
    public class WaterJetData : ScriptableObject
    {
        [SerializeField] private float _timeBetweenShot;
        [SerializeField] private WaterData _waterData;

        public float TimeBetweenShot
        {
            get => _timeBetweenShot;
            set => _timeBetweenShot = value;
        }
        public WaterData WaterData
        {
            get => _waterData;
            set => _waterData = value;
        }
    }
}