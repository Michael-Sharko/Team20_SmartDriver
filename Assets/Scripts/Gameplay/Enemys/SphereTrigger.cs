using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereTrigger : PlayerInTrigger
{
    [SerializeField]
    private float aggressionRadius = 50f;

    private SphereCollider aggressionTrigger => gameObject.GetComponent<SphereCollider>();

    private void OnValidate()
    {
        aggressionTrigger.radius = aggressionRadius;
    }
}
