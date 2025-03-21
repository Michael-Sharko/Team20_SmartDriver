using Shark.Gameplay.Player;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : CarSounds
{
    [SerializeField]
    private CarController carController;

    [SerializeField, Range(0f, 1f)]
    private float volumeForvard;
    [SerializeField, Range(0f, 1f)]
    private float volumeBack;
    [SerializeField, Range(0f, 1f)]
    private float volumeIndling;

    [SerializeField] private float maxEnginePitch = 1.35f;
    [SerializeField] private float minEnginePitch = 0.75f;


    private float indlingEnginePitch = 0.9f;
    private float currentEnginePitch;

    private int audioClipState;

    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        SoundStateDetection();
        AudioClipChang(audioClipState);

        if (audioSrc.clip == null)
            Debug.Log("Не указана ссылка на скрипт со звуком, для двигателя");
    }

    private int SoundStateDetection()
    {
        if (carController.vInput > 0f)
            audioClipState = 0;
        else if (carController.vInput < 0f)
            audioClipState = 1;
        else
            audioClipState = 2;
        return audioClipState;
    }

    private void AudioClipChang(int state)
    {
        switch (state)
        {
            case 0:
            audioSrc.volume = volumeForvard;
            audioSrc.pitch = Mathf.Clamp(audioSrc.pitch += 0.08f * Time.deltaTime, audioSrc.pitch, maxEnginePitch);
            break;

            case 1:
            audioSrc.volume = volumeBack;
            audioSrc.pitch = Mathf.Clamp(audioSrc.pitch -= 0.08f * Time.deltaTime, minEnginePitch, audioSrc.pitch);
            break;

            case 2:
            audioSrc.volume = volumeIndling;

            if (audioSrc.pitch > indlingEnginePitch)
                audioSrc.pitch = Mathf.Clamp(audioSrc.pitch -= 0.3f * Time.deltaTime, indlingEnginePitch, maxEnginePitch);
            else if (audioSrc.pitch < indlingEnginePitch)
                audioSrc.pitch = Mathf.Clamp(audioSrc.pitch += 0.3f * Time.deltaTime, audioSrc.pitch, indlingEnginePitch);

            break;
        }
    }
}
