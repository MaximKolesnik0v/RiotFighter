using Controllers;
using Controllers.Enemy;
using Enums;
using Interfaces.Enemy;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Enemy;

public class ExplosionButtonView : MonoBehaviour
{
    [SerializeField] private float _buttonLife = 100f;
    [SerializeField] private List<Transform> _explosionPoints = new List<Transform>();

    private EffectsData _effectsData;
    private bool _isExplosionOnce = false;

    public event Action ExplosionEvent;

    public float ButtonLife
    {
        get => _buttonLife;
        set => _buttonLife = value;
    }

    public List<Transform> ExplosionPoints
    {
        get => _explosionPoints;
    }

    void Start()
    {
        _effectsData = DataController.SoDataItems
            .Where(i => i.type == Enums.SoDataType.EFFECTS_DATA)
            .Select(i => i.data)
            .FirstOrDefault() as EffectsData;
    }

    void OnCollisionEnter(Collision collision)
    {
        var interactiveObject = collision.gameObject.GetComponent<IInteractiveObject>();

        if (interactiveObject is WaterView waterView)
        {
            if (_buttonLife > 0f)
            {
                var controller = (WaterController)waterView.Controller;
                _buttonLife -= controller.Model.Damage;

                if (_buttonLife <= 0)
                {
                    _isExplosionOnce = true;
                }
            } else
            {
                if (_isExplosionOnce)
                {
                    _isExplosionOnce = false;
                    foreach (var explosionPoint in _explosionPoints)
                    {
                        Explosion(explosionPoint.position);
                    }
                    ExplosionEvent?.Invoke();
                }
            }
        }
    }

    private void Explosion(Vector3 bombPos)
    {
        var radius = 15f;
        var power = 10_000f;
        var colliders = Physics.OverlapSphere(bombPos, radius);
        foreach (Collider hit in colliders)
        {
            var enemyView = hit.GetComponent<IEnemyView>();

            if (enemyView is CitizenView citizenView)
            {
                var controller = (CitizenController)citizenView.Controller;
                controller.Model.CurrentHealth = 0f;
                controller.Death();
            }
            if (enemyView is ProvocateurView provocateurView)
            {
                var controller = (ProvocateurController)provocateurView.Controller;
                controller.Model.CurrentHealth = 0f;
                controller.Death();
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(power, bombPos, radius, 3.0F);
            }
        }
        //var explosionEffectPrefab = _effectsData.EffectList
        //    .Where(i => i.Type == EffectType.CAR_EXPLOSION)
        //    .Select(i => i.Prefab)
        //    .FirstOrDefault();
        //var explosionEffect = Instantiate(
        //    explosionEffectPrefab,
        //    bombPos,
        //    Quaternion.identity,
        //    DataController.GameObjectRoots[GameObjectRoot.GAMEITEMS_ROOT]);

        //Destroy(explosionEffect, 4f);
    }
}
