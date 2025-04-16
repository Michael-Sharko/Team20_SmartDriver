using System;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class SandEffect : SurfaceParticles
    {
        protected override ParticleID GetId => ParticleID.Sand;
    }
}