using System;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class DirtEffect : SurfaceParticles
    {
        protected override ParticleID GetId => ParticleID.Dirt;
    }
}