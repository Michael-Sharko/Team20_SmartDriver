using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MovingEnemys : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject target;
    private Vector3 targetPosition => target.transform.position;

    [SerializeField, Range(1f, 20f)]
    private float moveSpeed = 9f;

    [SerializeField]
    SeekingTrigger sTrigger;
    [SerializeField]
    AggressionTrigger aTrigger;

    private bool onAtack => aTrigger.onAtack;
    private bool onSeeking => sTrigger.onSeeking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (onAtack)
            agent.destination = targetPosition;

        if (!onSeeking)
            agent.destination = agent.transform.position;
    }
}
