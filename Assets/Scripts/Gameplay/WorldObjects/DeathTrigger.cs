using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBreakable breakable))
        {
            breakable.TakeDamage(float.MaxValue);
        }
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
