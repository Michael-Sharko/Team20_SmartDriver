using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SeekingTrigger : MonoBehaviour
{
    public bool IsTouching { get; private set; } = false;

    [SerializeField]
    private float seekingRadius = 80f;

    private BoxCollider seekingTrigger => gameObject.GetComponent<BoxCollider>();

    private void OnValidate()
    {
        seekingTrigger.size = new(seekingRadius, seekingRadius, seekingRadius);
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
