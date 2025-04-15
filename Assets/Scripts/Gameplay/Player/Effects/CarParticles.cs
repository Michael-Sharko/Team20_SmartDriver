using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarParticles
    {
        protected List<ParticleSystem> GetParticles(GameObject car, ParticleID iD)
        {
            var result = new List<ParticleSystem>();

            var particles = car.GetComponentsInChildren<ParticleWithID>();

            foreach (var particle in particles)
            {
                if (particle.Id == iD)
                {
                    result.Add(particle.GetComponent<ParticleSystem>());
                }
            }
            return result;
        }
    }
}