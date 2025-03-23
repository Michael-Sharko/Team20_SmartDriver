using System;
using System.Collections;
using UnityEngine;

public class PlaySoundsWhileConditionAndDestroy : MonoBehaviour
{
    private AudioSource _source;
    private Func<bool> _soundWhileTrue;

    public void Play(AudioClip clip, Func<bool> soundWhileTrue, float volume = 1)
    {
        _source = GetComponent<AudioSource>();

        _soundWhileTrue = soundWhileTrue;
        _source.clip = clip;
        _source.volume = volume;
        _source.loop = true;
        _source.Play();

        StartCoroutine(DestroyOnEndClip());
    }
    private IEnumerator DestroyOnEndClip()
    {
        yield return new WaitWhile(_soundWhileTrue);

        _source.loop = false;

        yield return new WaitWhile(() => _source.isPlaying);

        Destroy(gameObject);
    }
}