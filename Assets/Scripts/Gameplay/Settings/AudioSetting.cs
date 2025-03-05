using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AudioSetting), menuName = "Game/AudioSetting")]
public class AudioSetting : ScriptableObject {

    public const string MUSIC_VOLUME_KEY = "musicVolume";
    public const string SOUNDS_VOLUME_KEY = "soundsVolume";

    public event Action<float> OnChangeMusicVolume;
    public event Action<float> OnChangeSoundsVolume;

    public float MusicVolume {
        get {

            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)) {

                return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);

            }
            return 1;
        }

        set {

            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
            OnChangeMusicVolume?.Invoke(value);

        }
    }
    public float SoundsVolume {
        get {

            if (PlayerPrefs.HasKey(SOUNDS_VOLUME_KEY)) {

                return PlayerPrefs.GetFloat(SOUNDS_VOLUME_KEY);

            }

            return 1;
        }

        set {

            PlayerPrefs.SetFloat(SOUNDS_VOLUME_KEY, value);
            OnChangeSoundsVolume?.Invoke(value);
        }
    }
}