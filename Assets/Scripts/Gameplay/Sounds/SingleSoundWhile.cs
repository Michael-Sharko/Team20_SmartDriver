using System;
using UnityEngine;

[Serializable]
public class SingleSoundWhile : AbstractSoundWhile
{
    [SerializeField] private AudioClip sound;

    protected override AudioClip GetClip()
    {
        return sound;
    }
}
