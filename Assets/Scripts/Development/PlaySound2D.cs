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
            if (_source == null)
                _source = Init("Main");

            return _source;
        }
    }
    // по хорошему надо менеджить сорсы, а не просто выдавать новый при обращении
    // но пока впадлу это делать
    public static AudioSource GetNewSource(string namePostfix = "")
    {
        return Init(namePostfix);
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