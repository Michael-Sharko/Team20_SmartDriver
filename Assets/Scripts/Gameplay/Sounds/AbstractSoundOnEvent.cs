using System;
using UnityEngine;

public abstract class AbstractSoundOnEvent
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
