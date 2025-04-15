using System;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class CarEffects
    {
        [SerializeField] private ExhaustPipeEffect exhaustPipeEffect;

        public void Init(GameObject car)
        {
            exhaustPipeEffect.Init(car);
        }
        public void UpdateValues()
        {
            exhaustPipeEffect?.UpdateRateOfSmoke();
        }
    }
}