using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public interface IRefuelable
    {
        public void Refuel(float value);
    }
}