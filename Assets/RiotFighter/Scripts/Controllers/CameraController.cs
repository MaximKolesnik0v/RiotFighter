using ScriptableObjects;
using System.Linq;
using UnityEngine;
using Views;

namespace Controllers
{
    public class CameraController
    {
        private Transform _target;
        private Camera _camera;
        private CameraData _cameraData;

        public CameraController()
        {
            _camera = Object.FindObjectOfType<Camera>();
            _target = Object.FindObjectOfType<CarView>().transform;
            _cameraData = DataController.SoDataItems
                .Where(i => i.type == Enums.SoDataType.CAMERA_DATA)
                .Select(i => i.data)
                .FirstOrDefault() as CameraData;
            _camera.transform.position = _target.position + _cameraData.PositionOffset;
        }

        public void LateUpdate()
        {
            _camera.transform.position = _target.position + _cameraData.PositionOffset;
            _camera.transform.rotation = Quaternion.Euler(_cameraData.RotationOffset);
        }
    }
}