using Interfaces;
using System.Collections.Generic;

namespace Controllers
{
    public class PolicemansController : BaseController
    {
        private List<IView> _policemanViews;
        private List<IController> _policemanControllers = new List<IController>();

        public PolicemansController(List<IView> policemans)
        {
            _policemanViews = policemans;
            Init();
        }

        public override void Update(float time)
        {
            foreach (var policemanController in _policemanControllers)
            {
                policemanController.Update(time);
            }
        }

        public override void FixedUpdate(float time)
        {
            foreach (var policemanController in _policemanControllers)
            {
                policemanController.FixedUpdate(time);
            }
        }

        private void Init()
        {
            foreach (var policemanView in _policemanViews)
            {
                _policemanControllers.Add(policemanView.Controller);
            }
        }
    }
}