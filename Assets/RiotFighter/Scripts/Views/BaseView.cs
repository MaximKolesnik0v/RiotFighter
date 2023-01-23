using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class BaseView : MonoBehaviour, IView, IInteractiveObject
    {
        public abstract IController Controller { get; set; }
    }
}