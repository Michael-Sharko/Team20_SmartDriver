using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Gameplay.Tags;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class SurfaceParticles<T> : CarParticles 
        where T : Tag
    {
        [SerializeField] private float _lifeTime = 0.8f;
        [SerializeField] private float emissionRateMultiplier = 1f;
        [SerializeField] private SurfaceChecker surfaceChecker;

        private List<ParticleSystem> _particles;
        private Get<float> _speed;
        private MonoBehaviour _coroutineOwner;

        public void Init(MonoBehaviour car,
            Get<float> speed,
            TextureUnderWheelsCheker textureUnderWheelsCheker)
        {
            _speed = speed;
            _coroutineOwner = car;
            _particles = GetParticles<T>(car.gameObject);
            surfaceChecker.Init(textureUnderWheelsCheker);

            car.StartCoroutine(DoEffect());

            UpdateLifeTime();
            OffEffect();
        }

        private void OffEffect()
        {
            foreach (var system in _particles)
            {
                var emission = system.emission;
                emission.rateOverTime = 0;
            }
        }
        public void UpdateLifeTime()
        {
            foreach (var system in _particles)
            {
                var main = system.main;
                main.startLifetime = _lifeTime;
            }
        }
        private IEnumerator DoEffect()
        {
            while (true)
            {
                yield return new WaitUntil(() => surfaceChecker.IsTouching);

                yield return _coroutineOwner.StartCoroutine(UpdateRateOverSpeed());

                OffEffect();
            }
        }
        private IEnumerator UpdateRateOverSpeed()
        {
            while (surfaceChecker.IsTouching)
            {
                foreach (var system in _particles)
                {
                    var emission = system.emission;
                    emission.rateOverTime = _speed.Value * emissionRateMultiplier;
                }

                yield return null;
            }
        }
    }
}