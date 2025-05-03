using UnityEngine;

public class VisualTrigger : PlayerInTrigger
{
    [SerializeField, Min(1)] private float radius = 50f;

    private void OnValidate()
    {
        transform.localScale = new(radius, radius, radius);
    }

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