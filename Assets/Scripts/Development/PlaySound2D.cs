using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class PlaySound2D
{
    private const string TARGET_MIXER_GROUP_PATH = "Master/Sounds";

    private static AudioSource _source;
    private static Dictionary<string, AudioSource> _newSources = new();

    public static AudioSource Source
    {
        get
        {
            if (_source == null)
                _source = Init("Main");

            return _source;
        }
    }
    // по хорошему надо запихивать в пул сорсы, а не просто выдавать новый при обращении
    // но пока впадлу это делать
    public static AudioSource GetNewSource(string namePostfix)
    {
        var newSource = Init(namePostfix);

        if (_newSources.ContainsKey(namePostfix))
            _newSources[namePostfix] = newSource;
        else
            _newSources.Add(namePostfix, newSource);

        return newSource;
    }
    public static AudioSource GetSource(string namePostfix)
    {
        return _newSources[namePostfix];
    }

    public static void Play(AudioClip clip, float volume = 1)
    {
        Source.PlayOneShot(clip, volume);
    }

    private static AudioSource Init(string postfix)
    {
        var source = CreateGameObjectObject(postfix);
        AddMixerGroup(source);

        return source;
    }
    private static AudioSource CreateGameObjectObject(string namePostfix)
    {
        var sourceGO = new GameObject("PlaySound2D " + namePostfix);
        sourceGO.transform.parent = DynamicSpawn.Parent;
        var source = sourceGO.AddComponent<AudioSource>();
        source.playOnAwake = false;

        return source;
    }
    private static void AddMixerGroup(AudioSource source)
    {
        var mixer = Resources.Load<AudioMixer>(PathResources.Audio.mixer);
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(TARGET_MIXER_GROUP_PATH)[0];
    }
}