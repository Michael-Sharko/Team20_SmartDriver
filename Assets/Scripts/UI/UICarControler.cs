using System;
using System.Collections;
using Shark.Gameplay.Player;
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
            [Serializable]
            private struct StrengthArrowRotationState
            {

                public float rotation;
                public float checkpoint;

            }

            [SerializeField]
            public RawImage _strengthIcon;

            [SerializeField]
            private Transform _strengthArrowAnchor;

            [SerializeField]
            private Transform _strengthArrow;

            [SerializeField]
            private StrengthState[] _states;

            [SerializeField]
            private StrengthArrowRotationState[] _arrowStates;

            [Header("Settings"), SerializeField]
            public float _gearRotationSpeed = 4;

            [SerializeField]
            private float _arrowSpeed = 1;

            private MonoBehaviour _monoBehaviour;
            private Coroutine _animRoutine;

            public void UpdateStrengthView(float strength)
            {
                UpdateStrengthArrow(strength);
                UpdateStrengthIcon(strength);
            }
            private void UpdateStrengthArrow(float strength)
            {
                var rotation = -90f;
                foreach (var state in _arrowStates)
                {
                    if (strength <= state.checkpoint)
                    {
                        rotation = state.rotation;
                    }
                    else
                    {
                        break;
                    }
                }
                var targetRotation = Quaternion.Euler(0, 0, rotation);

                if (_animRoutine != null)
                    _monoBehaviour.StopCoroutine(_animRoutine);
                _animRoutine = _monoBehaviour.StartCoroutine(AnimateStrengthArrowRotation(targetRotation));
            }
            private void UpdateStrengthIcon(float strength)
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
                if (strength < 0) _gearRotationSpeed = 0;
            }
            private IEnumerator AnimateStrengthArrowRotation(Quaternion targetRotation)
            {
                var startRotation = _strengthArrowAnchor.rotation;
                float lerp = 0;

                do
                {
                    lerp += Time.deltaTime * _arrowSpeed;
                    _strengthArrowAnchor.rotation = Quaternion.Lerp(startRotation, targetRotation, lerp);

                    yield return null;

                } while (lerp <= 1);

                _animRoutine = null;
            }
            public void Init(MonoBehaviour monoBehaviour)
            {
                _monoBehaviour = monoBehaviour;
            }
        }
        [Serializable]
        private class Speedometer
        {
            [SerializeField] private TextMeshProUGUI _text;
            [SerializeField] private RectTransform _arrowAnchor;
            [SerializeField, Range(0, 1)] private float _arrowRotationSmoothness = 0.05f;
            [SerializeField] private float _minRotation = 101.5f;
            [SerializeField] private float _maxRotation = -101.5f;

            public TextMeshProUGUI Text => _text;
            public RectTransform ArrowAnchor => _arrowAnchor;
            public float ArrowRotationSmoothness => _arrowRotationSmoothness;
            public float MinRotation => _minRotation;
            public float MaxRotation => _maxRotation;
        }

        [SerializeField]
        private UICarStrength _strength;

        [SerializeField]
        private Speedometer _speedometer;

        [SerializeField]
        private FuelGaugeSystem _fuelGauge;

        private CarController _car;

        private void Awake()
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
            _strength.Init(this);
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
            var min = _speedometer.MinRotation;
            var max = _speedometer.MaxRotation;
            var rotationZ = Mathf.Lerp(min, max, _car.SpeedKmh / 180);

            var targetRotation = Quaternion.Euler(0, 0, rotationZ);
            var lerpedRotation = Quaternion.Lerp(_speedometer.ArrowAnchor.rotation, targetRotation, _speedometer.ArrowRotationSmoothness);
            _speedometer.ArrowAnchor.rotation = lerpedRotation;

            if (_speedometer != null)
                _speedometer.Text.text = $"Speedometer: {_car.SpeedKmh:F0} Km/h";
        }

        private void UpdateFuelGauge()
        {
            if (_fuelGauge != null)
                _fuelGauge.SetFuelLevel(_car.currentFuel, _car.fuelCapacity);
        }

        public void UpdateStrengthRotation()
        {
            if (_strength._strengthIcon == null) return;

            _strength._strengthIcon.transform.Rotate(new Vector3(0, 0, _car.SpeedKmh * _strength._gearRotationSpeed * Time.deltaTime));
        }
        private void UpdateStrength()
        {
            if (_strength != null)
                UpdateStrengthRotation();
        }

        private void OnUpdatedStrengthIcon()
        {
            if (_strength != null)
                _strength.UpdateStrengthView(_car.currentStrength);
        }
    }
}