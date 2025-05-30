using System;
using UnityEngine;

[Serializable]
public class ManualSound
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private BaseGetSound getSound;

    private AudioSource _source;

    public void Init()
    {
        _source = PlaySound2D.Source;
    }
    public void Init(AudioSource source)
    {
        _source = source;
    }
    public void Play()
    {
        _source.PlayOneShot(getSound.GetClip(), volume);
    }
}