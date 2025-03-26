using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class Immunable
    {
        [SerializeField] private float immunableTime = 0.5f;

        private Coroutine _immunableRoutine;
        private MonoBehaviour _monoBehaviour;

        public bool IsImmunable { get; private set; }

        public void Init(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }
        public void MakeImmunable()
        {
            if (_immunableRoutine != null)
            {
                _monoBehaviour.StopCoroutine(_immunableRoutine);
            }

            IsImmunable = true;

            _immunableRoutine = _monoBehaviour.LateAndInvoke(immunableTime, () =>
            {
                IsImmunable = false;
                _immunableRoutine = null;
            });
        }
    }
}