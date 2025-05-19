using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SoundWhile
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private BaseGetSound getSound;

    private Func<bool> _soundWhileTrue;
    private AudioSource _source;
    private MonoBehaviour _coroutineOwner;
    private bool _dropSound;
    private bool _continuousOff;

    public void Init(Func<bool> soundWhileTrue, AudioSource source, MonoBehaviour coroutineOwner,
        bool dropSound = false, bool continuousOff = false)
    {
        _continuousOff = continuousOff;
        _dropSound = dropSound;
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
        _source.volume = volume;
        _source.loop = true;
        _source.Play();

        yield return new WaitWhile(_soundWhileTrue);

        _source.loop = false;

        if (!_dropSound)
            yield return new WaitWhile(() => _source.isPlaying);
        else if (_continuousOff)
        {
            var startTime = Time.time;
            var finishTime = Time.time + 0.5f;

            while (Time.time <= finishTime)
            {
                var lerp = Mathf.InverseLerp(startTime, finishTime, Time.time);
                _source.volume = Mathf.Lerp(volume, 0, lerp);

                yield return null;
            }
        }

        _source.Stop();
    }
}
