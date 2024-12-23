using Shark.Gameplay.Player;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shark.Gameplay.UI
{
    public class UICarControler : MonoBehaviour
    {
        [Serializable]
        private class UICarStrength
        {
            [Serializable]
            private struct StrengthState
            {
                [SerializeField]
                public Texture texture;

                [SerializeField]
                public float checkpoint;
            }

            [SerializeField]
            private StrengthState[] _states;

            [SerializeField]
            private RawImage _strengthIcon;

            [SerializeField]
            private float _rotationSpeed = 30;

            public void UpdateStrengthIcon(float strength)
            {
                if (_strengthIcon == null || strength < 0) return;

                foreach (var state in _states)
                {
                    if (strength >= state.checkpoint)
                    {
                        _strengthIcon.texture = state.texture;
                        break;
                    }
                }
                if (strength < 0) _rotationSpeed = 0;
            }

            public void UpdateStrengthRotation()
            {
                if (_strengthIcon == null) return;

                _strengthIcon.transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
            }
        }
        [SerializeField]
        private UICarStrength _strength;

        [SerializeField]
        private TextMeshProUGUI _speedometer;

        [SerializeField]
        private FuelGaugeSystem _fuelGauge;

        private CarController _car;

        private void Start()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (_car != null) _car.OnDamageReceived += OnUpdatedStrengthIcon;
        }

        private void OnDisable()
        {
            if (_car != null) _car.OnDamageReceived -= OnUpdatedStrengthIcon;
        }

        private void Initialize()
        {
            _car = FindFirstObjectByType<CarController>();
        }

        private void Update()
        {
            if (!_car) return;

            UpdateSpeedometer();
            UpdateFuelGauge();
            UpdateStrength();
        }

        private void UpdateSpeedometer()
        {
            if (_speedometer != null)
                _speedometer.text = $"Speedometer: {_car.SpeedKmh:F0} Km/h";
        }

        private void UpdateFuelGauge()
        {
            if (_fuelGauge != null)
                _fuelGauge.SetFuelLevel(_car.currentFuel, _car.fuelCapacity);
        }
        private void UpdateStrength()
        {
            if (_strength != null)
                _strength.UpdateStrengthRotation();
        }

        private void OnUpdatedStrengthIcon()
        {
            if (_strength != null)
                _strength.UpdateStrengthIcon(_car.currentStrength);
        }
    }
}