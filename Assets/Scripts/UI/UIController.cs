using Shark.Gameplay.Player;
using TMPro;
using UnityEngine;

namespace Shark.Gameplay.UI
{
    public class UIControler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _speedometer;

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
            UpdateSpeedometer();
        }

        private void UpdateSpeedometer()
        {
            _speedometer.text = $"Speedometer: {_car.SpeedKmh:F0} Km/h";
        }
    }
}