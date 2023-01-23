using Interfaces;

namespace Controllers
{
    public class BaseController : IController
    {
        public virtual void Update(float time) { }

        public virtual void FixedUpdate(float time) { }

        public virtual void LateUpdate(float time) { }
    }
}
