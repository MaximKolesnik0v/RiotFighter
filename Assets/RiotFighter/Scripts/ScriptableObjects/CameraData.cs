using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "ScriptableObjects/Camera/CameraData")]
    public class CameraData : ScriptableObject
    {
        [Header("Положение камеры относительно машины")]
        [SerializeField] private Vector3 _positionOffset;

        [Header("Поворот камеры относительно машины")]
        [SerializeField] private Vector3 _rotationOffset;

        public Vector3 PositionOffset
        {
            get => _positionOffset;
            set => _positionOffset = value;
        }
        public Vector3 RotationOffset
        {
            get => _rotationOffset;
            set => _rotationOffset = value;
        }
    }
}