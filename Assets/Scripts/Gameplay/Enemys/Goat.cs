using System;
using System.Collections;
using Scripts.Gameplay.Enemys.Animations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource))]
[SelectionBase]
public class Goat : MonoBehaviour
{
    public event Action OnDamage;
    public event Action OnIdle;
    public event Action OnSeeking;


    [SerializeField, Min(0)] private float moveSpeed = 9f;
    [SerializeField, Min(0)] private float rotationSpeed = 50f;
    [SerializeField, Min(0)] private float damage = 10;
    [SerializeField, Min(0)] private float delayAfterAttack = 1f;
    [SerializeField] private float forceToVehicleOnCollision = 10;
    [SerializeField] private GoatAnimator goatAnimator;

    [Space]
    [SerializeField] private LayerMask whatIsPlayer;

    [Space]
    [SerializeField] PlayerInTrigger seekingTrigger;
    [SerializeField] PlayerInTrigger attackTrigger;

    private float currentSpeed;
    private bool isCollision;
    private NavMeshAgent agent;
    private DamageSource damageSource;
    private new Rigidbody rigidbody;
    private GameObject target;
    private NavMeshPath path;
    private bool isSeeking;
    private Vector3 currentPos;
    private Vector3 previousPos;

    private Vector3 targetPosition => target.transform.position;
    private bool playerEnterAgroZone => attackTrigger.IsTouching;
    private bool playerLeaveSeekZone => !seekingTrigger.IsTouching;


    private void Start()
    {
        path = new();

        currentSpeed = moveSpeed;

        goatAnimator.Init(this);

        damageSource = GetComponent<DamageSource>();
        damageSource.damage = damage;
        damageSource.OnDealDamage += DamageSource_OnDealDamage;

        rigidbody = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        target = GameObject.FindGameObjectWithTag("Player");

        var audioSource = GetComponent<AudioSource>();

        StartCoroutine(Idle());
    }

#if UNITY_EDITOR
    private void Update()
    {
        goatAnimator.UpdateValuesInEditor();
    }
#endif

    private void DamageSource_OnDealDamage()
    {
        isCollision = true;

        OnDamage?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider)
            return;
        if (!collision.rigidbody)
            return;

        ApplyForceToTarget(collision);
    }
    private void ApplyForceToTarget(Collision collision)
    {
        var point = collision.contacts[0].point;

        var force = -collision.impulse.normalized * forceToVehicleOnCollision;
        collision.rigidbody.AddForceAtPosition(force, point, ForceMode.VelocityChange);
    }

    private void UpdateSpeedByRotation()
    {
        // чтобы сначала развернулся в сторону игрока,
        // и только потом начал ускоряться в его сторону

        var velocity = (currentPos - previousPos).normalized;
        var dot = Vector3.Dot(velocity, transform.forward);
        var lerp = Mathf.InverseLerp(-1, 1, dot);

        currentSpeed = Mathf.Lerp(0, moveSpeed, lerp);
    }
    private IEnumerator Idle()
    {
        OnIdle?.Invoke();

        seekingTrigger.gameObject.Off();

        yield return new WaitUntil(() => playerEnterAgroZone);

        StartCoroutine(Seek());
    }
    private IEnumerator Seek()
    {
        OnSeeking?.Invoke();

        seekingTrigger.gameObject.On();

        // чтобы триггеры обновились, иначе начинается вакханалия
        yield return new WaitForFixedUpdate();

        isSeeking = true;

        while (true)
        {
            yield return null;

            UpdateSpeedByRotation();

            if (isCollision)
            {
                isCollision = false;
                isSeeking = false;

                StartCoroutine(LateAfterAttack());

                yield break;
            }
            if (playerLeaveSeekZone)
            {
                isSeeking = false;

                StartCoroutine(Idle());
                yield break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isSeeking)
            MoveToTarget();
    }
    private void MoveToTarget()
    {
        agent.CalculatePath(targetPosition, path);

        if (path.corners.Length <= 1)
            return;

        previousPos = currentPos;

        currentPos = Vector3.MoveTowards(
            rigidbody.position,
            path.corners[1],
            Time.fixedDeltaTime * currentSpeed);

        var newRotation = Quaternion.RotateTowards(
            rigidbody.rotation,
            Quaternion.LookRotation(path.corners[1] - rigidbody.position),
            Time.fixedDeltaTime * rotationSpeed);

        rigidbody.Move(currentPos, newRotation);
    }
    private IEnumerator LateAfterAttack()
    {
        yield return new WaitForSeconds(delayAfterAttack);

        StartCoroutine(Seek());
    }
}
