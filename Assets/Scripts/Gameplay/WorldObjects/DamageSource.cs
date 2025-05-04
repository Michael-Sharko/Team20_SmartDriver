using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class DamageSource : MonoBehaviour, IDamageSource
{
    [FormerlySerializedAs("_damage")]
    public float damage;

    [SerializeField]
    private float _damageThresholdCollisionForce = 1.2f;

    [SerializeField]
    private float _multiplierDamageThresholdCollisionForce = 10;

    [SerializeField]
    private SoundOnEvent soundsOnDamage;

    [SerializeField, Readonly]
    private float _lastCollisionForce;

    public event Action OnDealDamage;

    private void Awake()
    {
        soundsOnDamage.Init(ref OnDealDamage, GetComponent<AudioSource>());
    }

    public void DealDamage(IBreakable breakable)
    {
        if (breakable.TakeDamage(damage))
            OnDealDamage?.Invoke();
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

