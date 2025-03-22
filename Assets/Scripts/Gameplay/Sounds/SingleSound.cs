using System;
using UnityEngine;

[Serializable]
public class SingleSound : AbstractSoundOnEvent
{
    [SerializeField] private AudioClip sound;

    protected override AudioClip GetClip()
    {
        return sound;
    }
}
