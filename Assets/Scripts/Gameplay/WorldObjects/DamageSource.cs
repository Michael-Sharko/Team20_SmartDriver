using Shark.Gameplay.WorldObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour, IDamageSource
{
    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _damageThresholdCollisionForce = 1.2f;

    [SerializeField]
    private float _multiplierDamageThresholdCollisionForce = 10;

#if UNITY_EDITOR
    [field: SerializeField, Header("Debug-Constant")]
    private float LastCollisionForce { get; set; }
    private float _lastCollisionForce;
#endif

#if UNITY_EDITOR
    private void Update()
    {
        LastCollisionForce = _lastCollisionForce;
    }
#endif

    public void DealDamage(IBreakable breakable)
    {
        breakable.TakeDamage(_damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IBreakable breakable) &&
            collision.transform.TryGetComponent(out Rigidbody rigidbody))
        {            
            _lastCollisionForce = collision.impulse.magnitude / rigidbody.mass * _multiplierDamageThresholdCollisionForce;
            if (_lastCollisionForce >= _damageThresholdCollisionForce)
            {
                DealDamage(breakable);
            }
        }
    }
}
