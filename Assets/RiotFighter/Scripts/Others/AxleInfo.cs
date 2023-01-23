using System;
using UnityEngine;

namespace Others
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
}
