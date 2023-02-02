using Controllers;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class TowerView : BaseView
    {
        private TowerController _controller;

        public override IController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new TowerController(null);
                }
                return _controller;
            }
            set => _controller = value as TowerController;
        }
    }
}