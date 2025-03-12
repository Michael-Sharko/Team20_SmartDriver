using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AggressionTrigger : MonoBehaviour
{
    public bool IsTouching { get; private set; } = false;

    [SerializeField]
    private float aggressionRadius = 50f;

    private SphereCollider aggressionTrigger => gameObject.GetComponent<SphereCollider>();

    private void Start()
    {
        aggressionTrigger.radius = aggressionRadius;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            IsTouching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            IsTouching = false;

    }
}
