using System;
using Shark.Gameplay.WorldObjects;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Tumbleweed : MonoBehaviour, IActivatable
{
    [SerializeField, Range(0, 360)] private float directionAngle;
    [SerializeField] private float speed;
    [SerializeField] private SoundOnEvent soundOnActivate;

    private Rigidbody _rigidbody;
    private ConstantForce _constantForce;
    private Vector3 currentDirection;

    public event Action OnActivate;

    public void Activate()
    {
        OnActivate?.Invoke();

        _rigidbody.isKinematic = false;
        _constantForce.enabled = true;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _constantForce = GetComponent<ConstantForce>();

        soundOnActivate.Init(ref OnActivate, GetComponent<AudioSource>());

        SetDirectionAtAngle();

        _rigidbody.isKinematic = true;
    }
    private void SetDirectionAtAngle()
    {
        var xDirection = Mathf.Cos(directionAngle * Mathf.Deg2Rad);
        var yDirection = Mathf.Sin(directionAngle * Mathf.Deg2Rad);

        currentDirection = new Vector3(xDirection, 0, yDirection);

        _constantForce.force = currentDirection * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider)
            return;

        ChangeDirection(collision);
    }
    private void ChangeDirection(Collision collision)
    {
        var contacts = collision.contacts;

        currentDirection = transform.position - contacts[0].point;
        currentDirection.y = 0;

        _constantForce.force = currentDirection.normalized * speed;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var direction = new Vector3(
            Mathf.Cos(directionAngle * Mathf.Deg2Rad),
            0,
            Mathf.Sin(directionAngle * Mathf.Deg2Rad));

        var isPlayMode = UnityEditor.EditorApplication.isPlaying;

        var rotation = Quaternion.LookRotation(isPlayMode ? currentDirection : direction);
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, rotation, 10, EventType.Repaint);
    }
#endif
}