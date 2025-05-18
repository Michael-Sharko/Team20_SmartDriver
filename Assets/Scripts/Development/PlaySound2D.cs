using UnityEngine;
using UnityEngine.Audio;

public static class PlaySound2D
{
    private const string TARGET_MIXER_GROUP_PATH = "Master/Sounds";

    private static AudioSource _source;

    public static AudioSource Source
    {
        get
        {
            if (_source == null) Init();

            return _source;
        }
    }

    public static void Play(AudioClip clip, float volume = 1)
    {
        Source.PlayOneShot(clip, volume);
    }

    private static void Init()
    {
        CreateGameObjectObject();
        AddMixerGroup();
    }
    private static void CreateGameObjectObject()
    {
        var sourceGO = new GameObject("PlaySound2D");
        sourceGO.transform.parent = DynamicSpawn.Parent;
        _source = sourceGO.AddComponent<AudioSource>();
        _source.playOnAwake = false;
    }
    private static void AddMixerGroup()
    {
        var mixer = Resources.Load<AudioMixer>(PathResources.Audio.mixer);
        _source.outputAudioMixerGroup = mixer.FindMatchingGroups(TARGET_MIXER_GROUP_PATH)[0];
    }
}