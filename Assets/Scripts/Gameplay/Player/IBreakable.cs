using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable
{
    public bool TakeDamage(float damage);
    public bool IsBroken { get; }
}
