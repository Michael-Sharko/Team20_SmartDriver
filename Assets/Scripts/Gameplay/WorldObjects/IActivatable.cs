using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.WorldObjects
{
    public interface IActivatable : IWorldObject
    {
        public void Activate();
    }
}