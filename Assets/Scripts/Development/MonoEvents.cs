using System;
using UnityEngine;

namespace Assets.Scripts.Development
{
    [DefaultExecutionOrder(-100)]
    public class MonoEvents : MonoBehaviour
    {
        public event Action OnAwake;
        public event Action OnStart;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        private void Awake()
        {
            OnAwake?.Invoke();
        }
        void Start()
        {
            OnStart?.Invoke();
        }
        void Update()
        {
            OnUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}