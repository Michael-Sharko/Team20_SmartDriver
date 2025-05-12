using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(AudioSetting), menuName = "Game/AudioSetting")]
public class AudioSetting : ScriptableObject
{
    private List<AudioVolumeParameter> _parameters;

    public AudioVolumeParameter GetParameter(AudioVolumeID id)
    {
        if (_parameters == null)
            InitParameters();

        string key = Enum.GetName(typeof(AudioVolumeID), id);

        return _parameters.
            First((x) => x.key == key);
    }
    private void InitParameters()
    {
        _parameters = new List<AudioVolumeParameter>();

        foreach (var name in Enum.GetNames(typeof(AudioVolumeID)))
        {
            _parameters.Add(new(name));
        }
    }
}