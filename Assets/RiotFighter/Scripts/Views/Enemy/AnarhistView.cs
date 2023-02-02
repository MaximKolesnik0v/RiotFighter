using Controllers.Enemy;
using Interfaces;
using Interfaces.Enemy;
using System;
using UnityEngine;
using Views;

public class AnarhistView : BaseView, IEnemyView
{
    [Header("Может двигаться?")]
    [SerializeField] private bool _isMove = true;
    [Header("Установить кастомное здоровье?")]
    [SerializeField] private bool _isSetCustomHealth = false;
    [Header("Значение кастомного здоровья")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _firstAttackDistanceToCheckCar = 15f;
    [SerializeField] private float _defaultDistanceToCheckCar = 8f;
    [Header("Значения урона огня")]
    [SerializeField] private float _damagePerSecond = 1;
    [SerializeField] private float _fireDuration = 5;

    private AnarhistController _controller;

    public bool IsMove
    {
        get => _isMove;
        set => _isMove = value;
    }
    public bool IsSetCustomHealth
    {
        get => _isSetCustomHealth;
        set => _isSetCustomHealth = value;
    }
    public float Health
    {
        get => _health;
        set => _health = value;
    }
    public float FirstAttackDistanceToCheckCar
    {
        get => _firstAttackDistanceToCheckCar;
        set => _firstAttackDistanceToCheckCar = value;
    }
    public float DefaultDistanceToCheckCar
    {
        get => _defaultDistanceToCheckCar;
        set => _defaultDistanceToCheckCar = value;
    }
    public float DamagePerSecond
    {
        get => _damagePerSecond;
        set => _damagePerSecond = value;
    }
    public float FireDuration
    {
        get => _fireDuration;
        set => _fireDuration = value;
    }

    public event Action<IView> TakeWaterDamageEvent;

    public override IController Controller
    {
        get
        {
            if (_controller == null)
            {
                _controller = new AnarhistController(this);
            }
            return _controller;
        }
        set => _controller = value as AnarhistController;
    }


    void OnCollisionEnter(Collision collision)
    {
        var interactiveObject = collision.gameObject.GetComponent<IInteractiveObject>();

        if (interactiveObject is WaterView waterView)
        {
            TakeWaterDamageEvent?.Invoke(waterView);
        }
    }
}