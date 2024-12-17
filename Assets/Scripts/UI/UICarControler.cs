using Shark.Gameplay.Miscelious;
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

        private CarController _car;
        private Canvas _canvas;

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
            _canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            UpdateSpeedometer();
            UpdateFuelSlider();
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