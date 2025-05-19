using UnityEngine;

namespace Scripts.Gameplay.Components
{
    public class TriggerBox : TriggerBase
    {
        protected override void ForbitChangingColliderSizeAndCenter()
        {
            var collider = GetComponent<BoxCollider>();

            collider.size = Vector3.one;
            collider.center = Vector3.zero;
        }
#if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            var prewMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            //Gizmos.DrawWireCube(transform.position, transform.localScale);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = prewMatrix;
        }
#endif
    }
}