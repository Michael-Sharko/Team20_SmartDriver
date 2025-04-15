using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class CarStrength
    {
        public event Action OnDamageReceived;
        public event Action OnCarBroken;

        [field: SerializeField] public float maxStrength { get; private set; } = 100;
        public float currentStrength { get; private set; }
        public bool IsBroken => currentStrength <= 0;
        [SerializeField] private Immunable immunable;


        public void Init(MonoBehaviour coroutineOwner)
        {
            immunable.Init(coroutineOwner);
        }
        public void SetStrengthMax()
        {
            currentStrength = maxStrength;
        }
        public bool TakeDamage(float damage)
        {
            if (IsBroken)
                return false;
            if (immunable.IsImmunable)
                return false;

            immunable.MakeImmunable();

            currentStrength -= damage;
            OnDamageReceived?.Invoke();

            if (IsBroken)
            {
                OnCarBroken?.Invoke();
            }

            return true;
        }
    }
}