using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class TextureUnderWheelsCheker
    {
        public Texture2D TextureUnderWheel { get; private set; }

        public void Update(WheelHit hit)
        {
            TextureUnderWheel = TerrainHelper.TextureUnderWheel(hit);
        }
    }
}