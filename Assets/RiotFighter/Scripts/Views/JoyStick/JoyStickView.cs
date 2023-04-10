using UnityEngine;

namespace Views.JoyStick
{
    public class JoyStickView : MonoBehaviour
    {
        [SerializeField] private GameObject _joyStick;
        [SerializeField] private GameObject _touchMarker;
        [SerializeField] private GameObject _target;

        public GameObject JoyStick => _joyStick;
        public GameObject TouchMarker => _touchMarker;
        public GameObject Target => _target;
    }
}