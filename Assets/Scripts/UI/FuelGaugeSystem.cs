using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.UI
{
    [Serializable]
    public class FuelGaugeSystem
    {
        [SerializeField]
        private RectTransform _arrow;

        [SerializeField]
        private float _minRotation = 60, _maxRotation = -60;

        private float _fuelLevel;

        public void FuelGaugeSystem2(RectTransform arrow, float minRotation, float maxRotation, float initialiFuel)
        {
            _arrow = arrow;
            _minRotation = minRotation;
            _maxRotation = maxRotation;
            _fuelLevel = Mathf.Clamp01(initialiFuel);

            UpdateArrowRotation();
        }

        public void SetFuelLevel(float fuelAmount, float fuelCapacity)
        {
            float fuelLevel = fuelAmount / fuelCapacity;
            _fuelLevel = Mathf.Clamp01(fuelLevel);

            UpdateArrowRotation();
        }

        public void DecreaseFuel(float amount, float fuelCapacity)
        {
            SetFuelLevel(_fuelLevel - amount, fuelCapacity);
        }

        private void UpdateArrowRotation()
        {
            if (_arrow == null) return;

            float targetRotation = Mathf.Lerp(_minRotation, _maxRotation, _fuelLevel);
            _arrow.localRotation = Quaternion.Euler(0, 0, targetRotation);
        }
    }
}