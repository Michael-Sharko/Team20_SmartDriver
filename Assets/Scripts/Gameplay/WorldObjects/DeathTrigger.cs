using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_EDITOR
        DestroyVisual(); 
#endif
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBreakable breakable))
        {
            breakable.TakeDamage(float.MaxValue);
        }
    }
}
