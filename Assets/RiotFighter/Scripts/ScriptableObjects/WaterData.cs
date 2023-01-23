using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WaterData", menuName = "ScriptableObjects/Water/WaterData")]
    public class WaterData : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _shotSpeed;
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeTime;

        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }
        public float ShotSpeed
        {
            get => _shotSpeed;
            set => _shotSpeed = value;
        }
        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }
        public float LifeTime
        {
            get => _lifeTime;
            set => _lifeTime = value;
        }
    }
}