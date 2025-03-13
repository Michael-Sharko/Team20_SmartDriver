using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamageSource))]
[SelectionBase]
public class Moose : MonoBehaviour
{
    private class State
    {
        public Vector3 targetRotation;
        public Vector3 startPosition;
        public Vector3 endPosition;

        public void Init(Vector3 targetRotation, Vector3 startPosition, Vector3 endPosition)
        {
            this.targetRotation = targetRotation;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
    }
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float delayBeforeTurn;
    [SerializeField] private float delayAfterCollisionWithPlayer;

    [Space]
    [SerializeField] private Transform targetPosition;

    private State _state = new();
    private bool _moveToTargetPosition = true;
    private bool _collisionWithPlayer;
    private Rigidbody _rigidbody;
    private Vector3 _spaswnPos;
    private Vector3 _targetPos;

    private void Awake()
    {
        _spaswnPos = transform.position;
        _targetPos = targetPosition.position;

        SwitchState();

        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<DamageSource>().OnDealDamage += Moose_OnDealDamage;

        StartCoroutine(Move());
    }

    private void Moose_OnDealDamage()
    {
        _collisionWithPlayer = true;
    }
    private IEnumerator LateBeforeTurn()
    {
        yield return new WaitForSeconds(delayBeforeTurn);

        StartCoroutine(AnimateTurn());
    }
    private void SwitchState()
    {
        if (_moveToTargetPosition)
        {
            _state.Init(_spaswnPos - _targetPos, _spaswnPos, _targetPos);
        }
        else
        {
            _state.Init(_targetPos - _spaswnPos, _targetPos, _spaswnPos);
        }
        _moveToTargetPosition = !_moveToTargetPosition;
    }
    private IEnumerator AnimateTurn()
    {
        var targetRotation = Quaternion.LookRotation(_state.targetRotation);
        while (transform.rotation != targetRotation)
        {
            if (_collisionWithPlayer)
            {
                _collisionWithPlayer = false;
                yield return new WaitForSeconds(delayAfterCollisionWithPlayer);
            }
            var newRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
            _rigidbody.MoveRotation(newRotation);

            yield return null;
        }

        SwitchState();

        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        while (_rigidbody.position != _state.endPosition)
        {
            if (_collisionWithPlayer)
            {
                _collisionWithPlayer = false;
                yield return new WaitForSeconds(delayAfterCollisionWithPlayer);
            }

            var newPos = Vector3.MoveTowards(_rigidbody.position, _state.endPosition,
                speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(LateBeforeTurn());
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (UnityEditor.EditorApplication.isPlaying)
            Gizmos.DrawLine(_spaswnPos, _targetPos);
        else
            Gizmos.DrawLine(targetPosition.position, transform.position);
    }
#endif
}