using System;
using Scripts.Gameplay.Tags;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class ExhaustPipeEffect : CarParticles
    {
        [SerializeField] private float _smokeLifeTime = 0.8f;

        private ParticleSystem _smokeParticle;

        public void Init(GameObject car)
        {
            _smokeParticle = GetParticle<ExhaustPipeParticlesTag>(car);

            UpdateRateOfSmoke();
        }
        public void UpdateRateOfSmoke()
        {
            var main = _smokeParticle.main;
            main.startLifetime = _smokeLifeTime;
        }
    }
}