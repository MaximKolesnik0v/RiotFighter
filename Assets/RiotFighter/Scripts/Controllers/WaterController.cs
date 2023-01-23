using Controllers;
using Models;
using ScriptableObjects;
using UnityEngine;

public class WaterController : BaseController
{
    private GameObject _prefab;
    private WaterView _waterView;
    private WaterModel _waterModel;

    public WaterModel Model
    {
        get => _waterModel;
        set => _waterModel = value;
    }

    public WaterController() { }
    public WaterController(GameObject prefab, WaterData data)
    {
        _prefab = prefab;
        _waterView = prefab.GetComponent<WaterView>();
        _waterView.Controller = this;
        _waterModel = new WaterModel(data);
    }
    
    public override void FixedUpdate(float time = 0)
    {
        Move();
    }

    private void Move()
    {
        _prefab.GetComponent<Rigidbody>().velocity = _waterView.transform.TransformDirection(new Vector3(0, 0, _waterModel.ShotSpeed));
    }
}
