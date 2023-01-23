using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForTestingScene
{
    [Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheelCollider;
        public WheelCollider rightWheelCollider;
        public Transform leftWheelModel;
        public Transform rightWheelModel;
        public bool motor;
        public bool steering;
    }

    public class CarController : MonoBehaviour
    {
        [SerializeField] private List<AxleInfo> _axleInfos;
        [SerializeField] private float _maxMotorTorque;
        [SerializeField] private float _maxSteeringAngle;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _centerOfMass;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            float motor = _maxMotorTorque * Input.GetAxis("Vertical");
            float steering = _maxSteeringAngle * Input.GetAxis("Horizontal");

            Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            _rigidbody.centerOfMass = _centerOfMass.localPosition;
            //_rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * 2f);

            foreach (AxleInfo axleInfo in _axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheelCollider.steerAngle = steering;
                    axleInfo.rightWheelCollider.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheelCollider.motorTorque = motor;
                    axleInfo.rightWheelCollider.motorTorque = motor;
                }
                SetWheelModel(axleInfo.leftWheelCollider, axleInfo.leftWheelModel);
                SetWheelModel(axleInfo.rightWheelCollider, axleInfo.rightWheelModel);
            }
        }
        private void SetWheelModel(WheelCollider collider, Transform model)
        {
            Vector3 pos;
            Quaternion rot;
            collider.GetWorldPose(out pos, out rot);

            model.transform.position = pos;
            model.transform.rotation = rot;
        }
    }
}