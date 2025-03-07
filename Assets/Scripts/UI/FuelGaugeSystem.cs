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
        private RectTransform _arrowAnchor;

        [SerializeField, Range(0, 1)] 
        private float _arrowRotationSmoothness = 0.05f;

        [SerializeField]
        private float _minRotation = 90, _maxRotation = -90;

        private float _fuelLevel;

        public FuelGaugeSystem(RectTransform arrow, float minRotation, float maxRotation, float initialiFuel)
        {
            _arrowAnchor = arrow;
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
            if (_arrowAnchor == null) return;

            var rotationZ = Mathf.Lerp(_minRotation, _maxRotation, _fuelLevel);
            var targetRotation = Quaternion.Euler(0, 0, rotationZ);
            var lerpedRotation = Quaternion.Lerp(_arrowAnchor.rotation, targetRotation, _arrowRotationSmoothness);
            _arrowAnchor.rotation = lerpedRotation;
        }
    }
}