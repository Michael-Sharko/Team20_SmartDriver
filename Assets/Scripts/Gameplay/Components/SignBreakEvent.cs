using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay.Components
{
    public class SignBreakEvent : MonoBehaviour
    {
        [Min(1)] public float breakForce = 10;
        public UnityEvent onJointBreak;

        private Rigidbody rb;
        private bool isBroken;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (isBroken || collision.impulse.magnitude < breakForce)
                return;

            isBroken = true;
            rb.isKinematic = false;
            onJointBreak?.Invoke();
        }
        //private void OnJointBreak(float breakForce)
        //{
        //    onJointBreak?.Invoke();
        //}
    }
}