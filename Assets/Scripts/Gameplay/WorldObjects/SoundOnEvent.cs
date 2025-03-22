using System;
using UnityEngine;

public abstract class SoundOnEvent
{
    [SerializeField, Range(0, 1)] private float volume = 1;

    public void Init(ref Action @event)
    {
        @event += OnActivate;
    }
    protected abstract AudioClip GetClip();

    private void OnActivate()
    {
        var prefab = Resources.Load<PlaySoundsAndDestroy>("Prefabs/Sound");
        PlaySoundsAndDestroy sound = UnityEngine.Object.Instantiate(prefab);
        sound.Init(GetClip(), volume);
    }
}
[Serializable]
public class RandomSoundFromArray : SoundOnEvent
{
    [SerializeField] private AudioClip[] sounds;

    protected override AudioClip GetClip()
    {
        var randomIndex = UnityEngine.Random.Range(0, sounds.Length);
        return sounds[randomIndex];
    }
}

[Serializable]
public class SingleSound : SoundOnEvent
{
    [SerializeField] private AudioClip sound;

    protected override AudioClip GetClip()
    {
        return sound;
    }
}
