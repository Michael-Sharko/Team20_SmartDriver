using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class ManualInput : MonoBehaviour, IInput
    {
        public float HInput { get; set; }
        public float VInput { get; set; }
        public bool SpaceInput { get; set; }
    }
}