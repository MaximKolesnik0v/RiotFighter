using Controllers;
using Interfaces;
using Markers;
using Others;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public class CarView : BaseView
    {
        private CarController _controller;

        public event Action LevelEndTriggerEvent;

        public List<AxleInfo> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;
        public Rigidbody rigidbody;
        public Transform centerOfMass;
        
        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new CarController(Vector3.zero);
                }
                return _controller;
            }
            set => _controller = value as CarController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var levelEndPositionTrigger = other.gameObject.GetComponent<PlayerEndPositionMarker>();

            if (levelEndPositionTrigger != null)
            {
                LevelEndTriggerEvent?.Invoke();
            }
        }
    }
}