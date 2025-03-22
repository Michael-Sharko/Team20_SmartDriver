using System;
using UnityEngine;

public abstract class AbstractSoundWhile
{
    [SerializeField, Range(0, 1)] private float volume = 1;

    private Func<bool> _soundWhileTrue;

    public void Init(ref Action @event, Func<bool> soundWhileTrue)
    {
        @event += OnActivate;
        _soundWhileTrue = soundWhileTrue;
    }
    protected abstract AudioClip GetClip();

    private void OnActivate()
    {
        var prefab = Resources.Load<PlaySoundsWhileConditionAndDestroy>("Prefabs/SoundWhile");
        PlaySoundsWhileConditionAndDestroy sound = UnityEngine.Object.Instantiate(prefab);
        sound.Init(GetClip(), _soundWhileTrue, volume);
    }
}
