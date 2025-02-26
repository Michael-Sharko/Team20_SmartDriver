using Shark.Gameplay.Player;
using Shark.Gameplay.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable : IWorldObject
{
    public event Action OnPickUp;
    public void PickUp(IPlayer player);
}
