using Interfaces;
using System;
using UnityEngine;
using Views.Enemy;

namespace Views.GameItem
{
    public class DefendedZoneView : MonoBehaviour, IInteractiveObject
    {
        public event Action<IView> DefendCitizenEvent;
        public event Action<IView> UndefendCitizenEvent;

        private void OnTriggerEnter(Collider other)
        {
            var interactiveObject = other.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is CitizenView enemyView)
            {
                DefendCitizenEvent?.Invoke(enemyView);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactiveObject = other.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is CitizenView enemyView)
            {
                UndefendCitizenEvent?.Invoke(enemyView);
            }
        }
    }
}
