namespace Interfaces
{
    public interface IController
    {
        void Update(float time);
        void FixedUpdate(float time);
        void LateUpdate(float time);
    }
}