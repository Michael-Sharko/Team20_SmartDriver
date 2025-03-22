using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : PlayerInTrigger
{
    [SerializeField]
    private float seekingRadius = 80f;

    private BoxCollider seekingTrigger => gameObject.GetComponent<BoxCollider>();

    private void OnValidate()
    {
        seekingTrigger.size = new(seekingRadius, seekingRadius, seekingRadius);
    }
}
