using Cinemachine;
using UnityEngine;

namespace Scripts.Development
{
#if UNITY_EDITOR
    public class CameraGizmos : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            var collider = GetComponent<CinemachineCollider>();
            if (collider)
            {
                Gizmos.DrawSphere(transform.position, collider.m_CameraRadius);
            }
        }
    } 
#endif
}