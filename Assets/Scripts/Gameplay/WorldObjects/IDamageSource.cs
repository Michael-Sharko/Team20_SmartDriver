using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shark.Gameplay.WorldObjects
{
    public interface IDamageSource : IWorldObject
    {
        public void DealDamage(IBreakable breakable);
    }
}