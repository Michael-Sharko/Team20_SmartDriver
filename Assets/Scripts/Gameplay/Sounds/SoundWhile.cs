using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SoundWhile
{
    [SerializeField] private BaseGetSound getSound;

    private Func<bool> _soundWhileTrue;
    private AudioSource _source;
    private MonoBehaviour _coroutineOwner;

    public void Init(Func<bool> soundWhileTrue, AudioSource source, MonoBehaviour coroutineOwner)
    {
        _soundWhileTrue = soundWhileTrue;

        _source = source;
        _source.clip = getSound.GetClip();

        _coroutineOwner = coroutineOwner;

        _coroutineOwner.StartCoroutine(Update());
    }
    private IEnumerator Update()
    {
        while (true)
        {
            yield return new WaitUntil(_soundWhileTrue);

            yield return _coroutineOwner.StartCoroutine(PlaySound());
        }
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
