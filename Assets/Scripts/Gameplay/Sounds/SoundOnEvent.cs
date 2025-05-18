using System;
using UnityEngine;

[Serializable]
public class SoundOnEvent
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private BaseGetSound getSound;

    private AudioSource _source;

    public void Init(ref Action @event)
    {
        _source = PlaySound2D.Source;
        @event += OnActivate;
    }
    public void Init(ref Action @event, AudioSource source = null)
    {
        _source = source;
        @event += OnActivate;
    }
    private void OnActivate()
    {
        _source.PlayOneShot(getSound.GetClip(), volume);
    }
}