using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class CarFuel
    {
        public event Action OnCarFuelRanOut;

        [field: SerializeField] public float fuelCapacity { get; private set; } = 100;
        [SerializeField] private float _fuelConsuptionMultiplier = 0.01f;

        // просто обертка, для того чтобы ее передать и у получателя были актуальные значения топлива
        public Get<float> CurrentFuel { get; private set; }
        public bool HasFuel => currentFuel > 0;

        private float currentFuel;
        private bool _isFuelOut;

        public void Init()
        {
            CurrentFuel = new(() => currentFuel);
        }
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