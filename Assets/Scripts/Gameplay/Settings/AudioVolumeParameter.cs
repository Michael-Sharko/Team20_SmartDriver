using System;
using UnityEngine;

public class AudioVolumeParameter
{
    public event Action<AudioVolumeParameter> OnChangeVolume;

    public readonly string key;

    public AudioVolumeParameter(string key)
    {
        this.key = key;
    }

    public float Volume
    {
        get
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
            return 1;
        }

        set
        {
            PlayerPrefs.SetFloat(key, value);
            OnChangeVolume?.Invoke(this);
        }
    }
}
