using System;
using UnityEngine;

[Serializable]
public class RandomSoundFromArray : AbstractSoundOnEvent
{
    [SerializeField] private AudioClip[] sounds;

    protected override AudioClip GetClip()
    {
        var randomIndex = UnityEngine.Random.Range(0, sounds.Length);
        return sounds[randomIndex];
    }
}
