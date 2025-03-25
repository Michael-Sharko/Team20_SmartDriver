using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Tumbleweed : MonoBehaviour, IActivatable
{
    [SerializeField, Range(0, 360)] private float directionAngle;
    [SerializeField] private float speed;
    [SerializeField] private SoundOnEvent soundOnActivate;

    private bool _isTriggered;
    private Rigidbody _rigidbody;
    private ConstantForce _constantForce;

    public event Action OnActivate;

    public void Activate()
    {
        if (_isTriggered)
            return;

        OnActivate?.Invoke();

        _isTriggered = true;
        _rigidbody.isKinematic = false;

        _constantForce.enabled = true;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _constantForce = GetComponent<ConstantForce>();

        soundOnActivate.Init(ref OnActivate, GetComponent<AudioSource>());

        var xDirection = Mathf.Cos(directionAngle * Mathf.Deg2Rad);
        var yDirection = Mathf.Sin(directionAngle * Mathf.Deg2Rad);
        var direction = new Vector3(xDirection, 0, yDirection);
        _constantForce.force = direction * speed;

        _rigidbody.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider)
            return;

        _rigidbody.isKinematic = true;
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