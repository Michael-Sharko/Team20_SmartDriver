using Shark.Gameplay.Player;
using UnityEngine;

namespace Shark.Gameplay.UI
{
    public class UICarControler : MonoBehaviour
    {
        [SerializeField]
        private UICarStrength _strength;

        [SerializeField]
        private UICarSpeedometer _speedometer;

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
            if (_car != null) _car.CarStrength.OnDamageReceived += OnUpdatedStrengthIcon;
        }

        private void OnDisable()
        {
            if (_car != null) _car.CarStrength.OnDamageReceived -= OnUpdatedStrengthIcon;
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
            _speedometer.Update(_car.CarPhysics.SpeedKmh);
        }

        private void UpdateFuelGauge()
        {
            if (_fuelGauge != null)
                _fuelGauge.SetFuelLevel(_car.CarFuel.CurrentFuel.Value, _car.CarFuel.fuelCapacity);
        }

        private void UpdateStrength()
        {
            _strength?.UpdateStrengthRotation(_car.CarPhysics.SpeedKmh);
        }

        private void OnUpdatedStrengthIcon()
        {
            _strength?.UpdateStrengthView(_car.CarStrength.currentStrength, _car.CarStrength.maxStrength);
        }
    }
}