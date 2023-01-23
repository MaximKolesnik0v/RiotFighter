using Interfaces;
using System;
using UnityEngine;
using Views.Enemy;

namespace Views.GameItem
{
    public class ActiveZoneView : MonoBehaviour, IInteractiveObject
    {
        public event Action<IView> ProvokeCitizenEvent;

        private void OnTriggerEnter(Collider other)
        {
            var interactiveObject = other.gameObject.GetComponent<IInteractiveObject>();

            if (interactiveObject is CitizenView enemyView)
            {
                ProvokeCitizenEvent?.Invoke(enemyView);
            }    
        }
    }
}