using Constants;
using Controllers;
using Enums;
using Models;
using ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class WaterJetController
{
    private WaterJetData _waterJetData;
    private WaterController _waterController;
    private Transform _shotPoint;
    private float _shotCounter;
    private bool _canShot;
    private bool _pressShotButton;

    public Transform ShotPoint
    {
        get => _shotPoint;
        set => _shotPoint = value;
    }

    public WaterJetController()
    {
        _waterJetData = DataController.SoDataItems
            .Where(i => i.type == SoDataType.WATER_JET_DATA)
            .Select(i => i.data)
            .FirstOrDefault() as WaterJetData;
    }

    public void Update(float time)
    {
        Timer(time);

        _pressShotButton = Input.GetMouseButton(0);
    }

    public void FixedUpdate()
    {
        if (_pressShotButton && _canShot)
        {
            Shot();
            _canShot = false;
        }
    }

    private void Timer(float time)
    {
        _shotCounter -= time;

        if (_shotCounter <= 0)
        {
            _shotCounter = _waterJetData.TimeBetweenShot;
            _canShot = true;
        }
    }

    private void Shot()
    {
        var bullet = Object.Instantiate(_waterJetData.WaterData.Prefab, _shotPoint.position, _shotPoint.rotation,
            DataController.GameObjectRoots[GameObjectRoot.WATER_ROOT]);
        var waterController = new WaterController(bullet, _waterJetData.WaterData);

        waterController.FixedUpdate();
        Object.Destroy(bullet, _waterJetData.WaterData.LifeTime);
    }
}
