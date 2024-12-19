using Shark.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shark.Gameplay.UI
{
    public class UICarControler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _speedometer;

        [SerializeField]
        private Slider _fuel;

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

        private void Initialize()
        {
            _car = FindFirstObjectByType<CarController>();
        }

        private void Update()
        {
            if (!_car) return;

            UpdateSpeedometer();
            UpdateFuelSlider();
            
            if (_fuelGauge != null)
                _fuelGauge.SetFuelLevel(_fuel.value, _fuel.maxValue);
        }

        private void UpdateSpeedometer()
        {
            _speedometer.text = $"Speedometer: {_car.SpeedKmh:F0} Km/h";
        }

        private void UpdateFuelSlider()
        {
            _fuel.maxValue = _car.fuelCapacity;
            _fuel.value = _car.currentFuel;
        }
    }
}