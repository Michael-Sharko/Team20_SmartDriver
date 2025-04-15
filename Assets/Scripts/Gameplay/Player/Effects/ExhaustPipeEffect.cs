using System;
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
            _smokeParticle = GetParticles(car, ParticleID.ExhaustPipe)[0];

            UpdateRateOfSmoke();
        }
        public void UpdateRateOfSmoke()
        {
            var main = _smokeParticle.main;
            main.startLifetime = _smokeLifeTime;
        }
    }
}