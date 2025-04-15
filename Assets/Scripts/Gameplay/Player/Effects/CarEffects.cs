using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class CarEffects
    {
        [SerializeField] private ExhaustPipeEffect exhaustPipeEffect;
        [SerializeField] private DirtEffect dirtEffect;

        public void Init(MonoBehaviour car, Get<float> speed, TextureUnderWheelsCheker textureChecker)
        {
            exhaustPipeEffect.Init(car.gameObject);
            dirtEffect.Init(car, speed, textureChecker);
        }
        public void UpdateValues()
        {
            exhaustPipeEffect?.UpdateRateOfSmoke();
            dirtEffect?.UpdateLifeTime();
        }
    }
}