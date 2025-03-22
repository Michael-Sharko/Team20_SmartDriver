using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class DamageSource : MonoBehaviour, IDamageSource
{
    [Serializable]
    public class PlaySoundOnDamage
    {
        [FormerlySerializedAs("dealDamageSound")]
        [SerializeField] private AudioClip sound;
        [SerializeField, Min(0)] private float volume = 0.4f;

        private AudioSource source;

        public void Init(AudioSource source, ref Action onDamage)
        {
            this.source = source;
            onDamage += Play;
        }
        private void Play()
        {
            if (!sound || !source)
                return;

            source.PlayOneShot(sound, volume);
        }
    }
    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _damageThresholdCollisionForce = 1.2f;

    [SerializeField]
    private float _multiplierDamageThresholdCollisionForce = 10;

    [SerializeField]
    private PlaySoundOnDamage soundsOnDamage;

#if UNITY_EDITOR
    [field: SerializeField, Header("Debug-Constant")]
    private float LastCollisionForce { get; set; }
#endif

    private float _lastCollisionForce;

    public event Action OnDealDamage;

#if UNITY_EDITOR
    private void Update()
    {
        LastCollisionForce = _lastCollisionForce;
    }
#endif

    private void Awake()
    {
        soundsOnDamage.Init(GetComponent<AudioSource>(), ref OnDealDamage);
    }

    public void DealDamage(IBreakable breakable)
    {
        if (breakable.TakeDamage(_damage))
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

