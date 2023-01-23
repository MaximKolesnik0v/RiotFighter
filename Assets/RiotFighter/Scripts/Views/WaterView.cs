using Interfaces;
using Views;

public class WaterView : BaseView
{
    private WaterController _controller;

    public override IController Controller
    {
        get
        {
            if (_controller == null)
            {
                return new WaterController();
            }
            return _controller;
        }
        set => _controller = value as WaterController;
    }
}
