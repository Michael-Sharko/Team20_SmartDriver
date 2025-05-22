using System.Collections.Generic;
using Scripts.Gameplay.Tags;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarParticles
    {
        protected ParticleSystem GetParticle<T>(GameObject car) where T : Tag
        {
            var tagObject = car.GetComponentInChildren<T>();

            return tagObject.GetComponent<ParticleSystem>();
        }
        protected List<ParticleSystem> GetParticles<T>(GameObject car) where T : Tag
        {
            var result = new List<ParticleSystem>();

            var tagObjects = car.GetComponentsInChildren<T>();

            foreach (var particle in tagObjects)
            {
                result.Add(particle.GetComponent<ParticleSystem>());
            }
            return result;
        }
    }
}