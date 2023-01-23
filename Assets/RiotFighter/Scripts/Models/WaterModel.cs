using ScriptableObjects;
using UnityEngine;

namespace Models
{
    public class WaterModel
    {
        private GameObject _prefab;
        public float _damage;
        private float _shotSpeed;
        public float _lifeTime;

        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }
        public float ShotSpeed
        {
            get => _shotSpeed;
            set => _shotSpeed = value;
        }
        public float LifeTime
        {
            get => _lifeTime;
            set => _lifeTime = value;
        }

        public WaterModel(WaterData data)
        {
            _prefab = data.Prefab;
            _damage = data.Damage;
            _shotSpeed = data.ShotSpeed;
            _lifeTime = data.LifeTime;
        }
    }
}