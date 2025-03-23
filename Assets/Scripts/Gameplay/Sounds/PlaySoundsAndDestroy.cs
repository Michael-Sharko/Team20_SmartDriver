using System.Collections;
using UnityEngine;

public class PlaySoundsAndDestroy : MonoBehaviour
{
    private AudioSource _source;

    public void Play(AudioClip clip, float volume = 1)
    {
        _source = GetComponent<AudioSource>();

        _source.clip = clip;
        _source.volume = volume;
        _source.Play();

        StartCoroutine(DestroyOnEndClip());
    }
    private IEnumerator DestroyOnEndClip()
    {
        yield return new WaitWhile(() => _source.isPlaying);

        Destroy(gameObject);
    }
}
