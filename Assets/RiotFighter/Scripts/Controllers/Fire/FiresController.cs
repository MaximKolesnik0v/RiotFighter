using Interfaces;
using System.Collections.Generic;

namespace Controllers.Fire
{
    public class FiresController : BaseController
    {
        private List<IView> _fireViews;
        private List<IController> _fireControllers;

        public FiresController(List<IView> fires)
        {
            _fireViews = fires;
            _fireControllers = new List<IController>();

            Init();
        }

        public override void Update(float time)
        {
            foreach (var fireController in _fireControllers)
            {
                fireController.Update(time);
            }
        }

        public override void FixedUpdate(float time)
        {
            foreach (var fireController in _fireControllers)
            {
                fireController.FixedUpdate(time);
            }
        }

        private void Init()
        {
            foreach (var fire in _fireViews)
            {
                _fireControllers.Add(fire.Controller);
            }
        }
    }
}
