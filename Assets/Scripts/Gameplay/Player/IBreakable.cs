using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable
{
    public void TakeDamage(float damage);
    public bool IsBroken { get; }
}
