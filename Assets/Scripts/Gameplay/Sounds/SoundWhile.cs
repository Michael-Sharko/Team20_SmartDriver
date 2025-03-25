using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SoundWhile
{
    [SerializeField] private BaseGetSound getSound;

    private Func<bool> _soundWhileTrue;
    private AudioSource _source;
    private MonoBehaviour _behaviour;

    public void Init(ref Action @event, Func<bool> soundWhileTrue, AudioSource source, MonoBehaviour behaviour)
    {
        @event += OnActivate;
        _soundWhileTrue = soundWhileTrue;
        _source = source;
        _behaviour = behaviour;
    }
    private void OnActivate()
    {
        _source.clip = getSound.GetClip();
        _behaviour.StartCoroutine(PlaySound());
    }
    private IEnumerator PlaySound()
    {
        _source.loop = true;
        _source.Play();

        yield return new WaitWhile(_soundWhileTrue);

        _source.loop = false;

        yield return new WaitWhile(() => _source.isPlaying);

        _source.Stop();
    }
}
