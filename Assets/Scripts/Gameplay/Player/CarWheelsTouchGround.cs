using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarWheelsTouchGround
    {
        public bool IsTouching { get; private set; }

        public void Update(WheelHit hit)
        {
            IsTouching = hit.collider != null;
        }
    }
}