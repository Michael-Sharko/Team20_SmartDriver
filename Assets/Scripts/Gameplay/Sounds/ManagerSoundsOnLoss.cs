using Scripts.Gameplay.Tags;
using Shark.Gameplay.Player;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Sounds
{
    public class ManagerSoundsOnLoss : MonoBehaviour
    {
        private CarController _car;

        void Start()
        {
            _car = FindObjectOfType<CarController>();

            _car.CarFuel.OnCarFuelRanOut += GameOver;
            _car.CarStrength.OnCarBroken += GameOver;
        }
        private void OnDestroy()
        {
            _car.CarFuel.OnCarFuelRanOut -= GameOver;
            _car.CarStrength.OnCarBroken -= GameOver;
        }

        private void GameOver()
        {
            PlaySound2D.GetSource("Low Level").mute = true;

            if (Tags.TryGetTag(out LevelMusicTag tag))
                tag.gameObject.Off();
        }
    }
}