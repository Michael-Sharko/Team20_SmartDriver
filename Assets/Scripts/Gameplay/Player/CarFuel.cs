using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class Get<T>
    {
        public T Value { get; set; }
    }
    [Serializable]
    public class CarFuel
    {
        public event Action OnCarFuelRanOut;

        [field: SerializeField] public float fuelCapacity { get; private set; } = 100;
        [SerializeField] private float _fuelConsuptionMultiplier = 0.01f;

        // просто обертка, для того чтобы ее передать и у получателя были актуальные значения топлива
        public Get<float> CurrentFuel { get; private set; } = new();
        private float currentFuel
        {
            get => CurrentFuel.Value;
            set => CurrentFuel.Value = value;
        }
        public bool HasFuel => currentFuel > 0;
        private bool _isFuelOut;

        public void SetFuelMax()
        {
            currentFuel = fuelCapacity;
        }
        public void Refuel(float value)
        {
            currentFuel = Math.Min(currentFuel + value, fuelCapacity);
        }

        public void Update(float carSpeed)
        {
            ApplyFuelConsumption(carSpeed);
        }
        private void ApplyFuelConsumption(float carSpeed)
        {
            currentFuel -= carSpeed * _fuelConsuptionMultiplier * Time.fixedDeltaTime;

            if (currentFuel < 0 && !_isFuelOut)
            {
                _isFuelOut = true;
                OnCarFuelRanOut?.Invoke();
            }
        }
    }
}