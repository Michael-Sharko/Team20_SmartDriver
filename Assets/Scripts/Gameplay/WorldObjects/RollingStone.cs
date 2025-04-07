using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(AudioSource), typeof(Rigidbody), typeof(DamageSource))]
public class RollingStone : MonoBehaviour, IActivatable
{
    [SerializeField, Range(0, 360)] private float directionAngle;
    [SerializeField] private float speed;
    [SerializeField] private float forceToVehicleOnCollision = 10;
    [SerializeField] private SoundOnEvent soundOnActivate;

    private bool _isTriggered;
    private Vector3 _forceDirection;
    private Rigidbody _rigidbody;

    public event Action OnTriggered;
    private Action fixedUpdateTick;

    [ContextMenu(nameof(Activate))]
    public void Activate()
    {
        if (_isTriggered)
            return;

        // хоть я и удаляю коллайдер триггер, почему то при столкновении машинки вызывается этот метод
        Destroy(GetComponent<SphereCollider>());

        OnTriggered?.Invoke();

        fixedUpdateTick = Move;

        _isTriggered = true;
        _rigidbody.isKinematic = false;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        soundOnActivate.Init(ref OnTriggered, GetComponent<AudioSource>());

        var xDirection = Mathf.Cos(directionAngle * Mathf.Deg2Rad);
        var zDirection = Mathf.Sin(directionAngle * Mathf.Deg2Rad);
        _forceDirection = new Vector3(xDirection, 0, zDirection);

        _rigidbody.isKinematic = true;
    }
    private void FixedUpdate()
    {
        fixedUpdateTick?.Invoke();
    }
    private void Move()
    {
        _rigidbody.AddForce(_forceDirection * speed, ForceMode.Acceleration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider)
            return;

        var point = collision.contacts[0].point;
        fixedUpdateTick = null;

        var force = -collision.impulse.normalized * forceToVehicleOnCollision;
        collision.rigidbody.AddForceAtPosition(force, point, ForceMode.VelocityChange);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var direction = new Vector3(Mathf.Cos(directionAngle * Mathf.Deg2Rad), 0, Mathf.Sin(directionAngle * Mathf.Deg2Rad));
        var rotation = Quaternion.LookRotation(direction);
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, rotation, 10, EventType.Repaint);
    }
#endif
}