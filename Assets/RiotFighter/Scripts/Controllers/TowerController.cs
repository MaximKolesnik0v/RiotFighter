using UnityEngine;
using Object = UnityEngine.Object;
using Markers;

namespace Controllers
{
    public class TowerController
    {
        private Camera _camera;
        private GameObject _muzzle;
        private WaterJetController _waterJetController;

        public TowerController(GameObject muzzle)
        {
            _camera = Object.FindObjectOfType<Camera>();
            _waterJetController = new WaterJetController();

            Init(muzzle);
        }

        public void Update(float time)
        {
            MuzzleRotate();
            _waterJetController.Update(time);
        }

        public void FixedUpdate()
        {
            _waterJetController.FixedUpdate();
        }

        private void Init(GameObject muzzle)
        {
            _muzzle = muzzle;

            if (_muzzle == null) return;

            _waterJetController.ShotPoint = _muzzle.GetComponentInChildren<ShotPointMarker>().transform;
        }

        private void MuzzleRotate()
        {
            var cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit))
            {
                _muzzle.transform.LookAt(hit.point);
            }
            //Debug.DrawLine(cameraRay.origin, hit.point, Color.red);
        }
    }
}
