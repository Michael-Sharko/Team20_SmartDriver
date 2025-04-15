using System;
using System.Linq;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    [Serializable]
    public class SurfaceChecker
    {
        [SerializeField] private Texture2D[] textures;

        public bool IsTouching
            => textures.Any(t => t == _textureChecker.TextureUnderWheel);

        private TextureUnderWheelsCheker _textureChecker;

        public void Init(TextureUnderWheelsCheker textureChecker)
        {
            _textureChecker = textureChecker;
        }
    }
}