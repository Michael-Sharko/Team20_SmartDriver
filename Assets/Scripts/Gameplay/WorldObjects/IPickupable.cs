using Shark.Gameplay.Player;
using Shark.Gameplay.WorldObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable : IWorldObject
{
    public void PickUp(IPlayer player);
}
