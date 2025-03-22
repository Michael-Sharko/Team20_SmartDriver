using System;
using System.Collections;
using Shark.Gameplay.Player;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class Varan : MonoBehaviour
{
    [SerializeField, Min(0)] private float walkAnimationSpeed = 1;
    [SerializeField, Min(0)] private float attackAnimationSpeed = 1;
    [SerializeField, Min(0)] private float moveSpeed = 9f;
    [SerializeField, Min(0)] private float damage = 10;
    [SerializeField, Min(0)] private float attackDistance = 1;
    [SerializeField, Min(0)] private float delayAfterAttack = 1f;

    [SerializeField] SingleSound detectionPlayerSound;
    [SerializeField] SingleSound attackPlayerSound;
    [SerializeField] SingleSoundWhile movementSound;

    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] Transform attackAnchor;
    [SerializeField] PlayerInTrigger sTrigger;
    [SerializeField] PlayerInTrigger aTrigger;

    private Action _onAgroPlayer;
    private Action _onAttackPlayer;
    private Action _onStartMovement;

    private static readonly int WalkKey = Animator.StringToHash("isWalking");
    private static readonly int AttackKey = Animator.StringToHash("attack");

    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;
    private IPlayer player;
    private bool _isMoving;

    private Vector3 targetPosition => target.transform.position;
    private bool playerEnterAgroZone => aTrigger.IsTouching;
    private bool playerLeaveSeekZone => !sTrigger.IsTouching;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
            return;

        GetComponent<Animator>().SetFloat("walkSpeedMultiplier", walkAnimationSpeed);
        GetComponent<Animator>().SetFloat("attackAnimationSpeed", attackAnimationSpeed);
        GetComponent<NavMeshAgent>().speed = moveSpeed;
    }
#endif
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("walkSpeedMultiplier", walkAnimationSpeed);
        animator.SetFloat("attackAnimationSpeed", attackAnimationSpeed);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        target = GameObject.FindGameObjectWithTag("Player");

        detectionPlayerSound.Init(ref _onAgroPlayer);
        attackPlayerSound.Init(ref _onAttackPlayer);
        movementSound.Init(ref _onStartMovement, () => _isMoving);

        StartCoroutine(Idle());
    }
    private void UpdateSpeedByRotation()
    {
        // чтобы варан сначала развернулся в сторону игрока,
        // и только потом начал ускоряться в его сторону

        var dot = Vector3.Dot(agent.velocity.normalized, transform.forward);
        var lerp = Mathf.InverseLerp(-1, 1, dot);

        agent.speed = Mathf.Lerp(0, moveSpeed, lerp);
    }
    private IEnumerator Idle()
    {
        agent.destination = agent.transform.position;
        animator.SetBool(WalkKey, false);
        sTrigger.gameObject.SetActive(false);

        _isMoving = false;

        yield return new WaitUntil(() => playerEnterAgroZone);

        _onAgroPlayer?.Invoke();

        StartCoroutine(Seek());
    }
    private IEnumerator Seek()
    {
        animator.SetBool(WalkKey, true);
        sTrigger.gameObject.SetActive(true);

        _isMoving = true;

        _onStartMovement?.Invoke();

        // чтобы триггеры обновились, иначе начинается вакханалия
        yield return new WaitForFixedUpdate();

        while (true)
        {
            yield return null;

            agent.destination = targetPosition;

            UpdateSpeedByRotation();

            if (playerLeaveSeekZone)
            {
                StartCoroutine(Idle());
                yield break;
            }
            if (Physics.SphereCast(attackAnchor.position, 0.7f, transform.forward, out RaycastHit hit, attackDistance, whatIsPlayer))
            {
                if (hit.transform.root.TryGetComponent(out IPlayer player))
                {
                    this.player = player;

                    Attack();

                    StartCoroutine(LateAfterAttack());
                    yield break;
                }
            }
        }
    }
    private void Attack()
    {
        animator.SetTrigger(AttackKey);

        _onAttackPlayer?.Invoke();
    }
    public void OnAttackAnimationEnd()
    {
        player.TakeDamage(damage);
    }
    private IEnumerator LateAfterAttack()
    {
        _isMoving = false;

        agent.destination = agent.transform.position;
        animator.SetBool(WalkKey, false);

        yield return new WaitForSeconds(delayAfterAttack);

        StartCoroutine(Seek());
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(attackAnchor.position, 0.7f);
    }
#endif
}
