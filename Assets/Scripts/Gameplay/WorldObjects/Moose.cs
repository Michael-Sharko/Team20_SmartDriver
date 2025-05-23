﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamageSource))]
[SelectionBase]
public class Moose : MonoBehaviour
{
    private class Path
    {
        public Vector3 startPosition;
        public Vector3 endPosition;

        public void Init(Vector3 startPosition, Vector3 endPosition)
        {
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

    private Path _path = new();
    private bool _moveToTargetPosition = true;
    private bool _collisionWithPlayer;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private Vector3 newPos;

    private void Awake()
    {
        _startPos = transform.position;
        _targetPos = targetPosition.position;

        SwitchPath();

        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        GetComponent<DamageSource>().OnDealDamage += Moose_OnDealDamage;

        StartCoroutine(Move());
    }

    private void Moose_OnDealDamage()
    {
        _collisionWithPlayer = true;
    }
    private IEnumerator LateBeforeTurn()
    {
        _animator.SetTrigger("turn");

        yield return new WaitForSeconds(delayBeforeTurn);

        StartCoroutine(AnimateTurn());
    }
    private void SwitchPath()
    {
        if (_moveToTargetPosition)
        {
            _path.Init(_startPos, _targetPos);
        }
        else
        {
            _path.Init(_targetPos, _startPos);
        }
        _moveToTargetPosition = !_moveToTargetPosition;
    }
    private IEnumerator AnimateTurn()
    {
        var directionToTarget = _path.startPosition - _path.endPosition;
        directionToTarget.Normalize();

        while (Vector3.Dot(directionToTarget, transform.forward) < .98f)
        {
            if (_collisionWithPlayer)
            {
                _collisionWithPlayer = false;
                yield return new WaitForSeconds(delayAfterCollisionWithPlayer);
            }

            var newRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0)
                * transform.forward;

            _rigidbody.MoveRotation(Quaternion.LookRotation(newRotation));

            yield return null;
        }

        SwitchPath();

        //_animator.SetTrigger("turn-end");

        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        while (_rigidbody.position != _path.endPosition)
        {
            if (_collisionWithPlayer)
            {
                _collisionWithPlayer = false;
                yield return new WaitForSeconds(delayAfterCollisionWithPlayer);
            }

            newPos = Vector3.MoveTowards(_rigidbody.position, _path.endPosition,
               speed * Time.fixedDeltaTime);

            yield return null;
        }

        StartCoroutine(LateBeforeTurn());
    }
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(newPos);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (UnityEditor.EditorApplication.isPlaying)
            Gizmos.DrawLine(_startPos, _targetPos);
        else
            Gizmos.DrawLine(targetPosition.position, transform.position);
    }
#endif
}