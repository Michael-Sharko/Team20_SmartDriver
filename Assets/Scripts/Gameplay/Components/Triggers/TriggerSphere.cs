using UnityEngine;

namespace Scripts.Gameplay.Components
{
    public class TriggerSphere : TriggerBase
    {
        //[SerializeField, Min(1)] private float radius = 50f;

        protected override void ForbitChangingColliderSizeAndCenter()
        {
            var collider = GetComponent<SphereCollider>();

            collider.radius = 0.5f;
            collider.center = Vector3.zero;

            var x = transform.localScale.x;
            var y = transform.localScale.y;
            var z = transform.localScale.z;

            if (x != y && y == z)
            {
                transform.localScale = new(x, x, x);
            }
            if (y != x && x == z)
            {
                transform.localScale = new(y, y, y);
            }
            if (z != x && x == y)
            {
                transform.localScale = new(z, z, z);
            }
        }
#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
        }
#endif
    }
}