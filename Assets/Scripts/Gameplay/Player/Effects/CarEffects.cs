using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class CarEffects
    {
        [SerializeField] private BrakeMark brakeMark;
        [SerializeField] private ExhaustPipeEffect exhaustPipeEffect;
        [SerializeField] private DirtEffect dirtEffect;
        [SerializeField] private SandEffect sandEffect;

        public void Init(MonoBehaviour car, Get<float> speed, TextureUnderWheelsCheker textureChecker,
            Func<bool> whenToMark)
        {
            brakeMark.Init(car, whenToMark);
            exhaustPipeEffect.Init(car.gameObject);
            dirtEffect.Init(car, speed, textureChecker);
            sandEffect.Init(car, speed, textureChecker);
        }
        public void UpdateValues()
        {
            exhaustPipeEffect?.UpdateRateOfSmoke();
            dirtEffect?.UpdateLifeTime();
        }
    }
}