using UnityEngine;

namespace ForTestingScene
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Camera _camera;

        private Vector3 _positionOffset;
        private Vector3 _rotationOffset;

        private void Start()
        {
            _positionOffset = new Vector3(0, 7.27f, -9.97f);
            _rotationOffset = new Vector3(15f, 0, 0);
            _camera.transform.position = _target.position + _positionOffset;
        }

        public void LateUpdate()
        {
            _camera.transform.position = _target.position + _positionOffset;
            _camera.transform.rotation = Quaternion.Euler(_rotationOffset);
        }
    }
}