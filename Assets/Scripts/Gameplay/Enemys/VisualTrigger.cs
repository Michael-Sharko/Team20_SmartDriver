using UnityEngine;

public class VisualTrigger : PlayerInTrigger
{
    [SerializeField, Min(1)] private float radius = 50f;

    private void OnValidate()
    {
        transform.localScale = new(radius, radius, radius);
    }
    private void ForbitChangingColliderSizeAndCenter()
    {
        var collider = GetComponent<Collider>();

        if (collider is SphereCollider sphere)
        {
            sphere.radius = 0.5f;
            sphere.center = Vector3.zero;
        }
        else if (collider is BoxCollider box)
        {
            box.size = Vector3.one;
            box.center = Vector3.zero;
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        // вызывается в OnDrawGizmos, т.к. OnValidate не вызывается при изменении размера коллайдера
        // а нужно чтобы сразу значение не обновлялось
        ForbitChangingColliderSizeAndCenter();
    }
#endif

#if !UNITY_EDITOR
    private void Awake()
    {
        DestroyVisual(); 
    }

    private void DestroyVisual()
    {
        if (TryGetComponent(out MeshRenderer renderer))
        {
            Destroy(renderer);
        }
        if (TryGetComponent(out MeshFilter filter))
        {
            Destroy(filter);
        }
    }
#endif
}