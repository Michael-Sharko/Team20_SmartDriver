using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Components
{
    [System.Serializable]
    public class NamedSound
    {
        [SerializeField] private bool loop;
        [SerializeField] private AudioClip clip;

        public bool Loop => loop;
        public AudioClip Clip => clip;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundSequense : MonoBehaviour
    {
        [SerializeField] private NamedSound[] _allSounds;

        private int index;
        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.loop = false;
            StartCoroutine(PlaySound());
        }
        private IEnumerator PlaySound()
        {
            while (true)
            {
                if (index >= _allSounds.Length) yield break;

                source.clip = _allSounds[index].Clip;
                source.Play();

                yield return new WaitWhile(() => source.isPlaying);

                if (_allSounds[index].Loop)
                {
                    source.loop = true;
                    source.Play();

                    yield break;
                }

                index++;
            }
        }
    }
}