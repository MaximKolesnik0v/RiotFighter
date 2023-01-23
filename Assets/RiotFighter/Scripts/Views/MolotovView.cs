using System;
using UnityEngine;

namespace Views
{
    public class MolotovView : MonoBehaviour, IInteractiveObject
    {
        public event Action<GameObject> MolotoHitObjectEvent;

        void OnCollisionEnter(Collision collision)
        {
            MolotoHitObjectEvent?.Invoke(collision.gameObject);
        }
    }
}