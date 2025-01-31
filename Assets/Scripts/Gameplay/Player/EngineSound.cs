using Shark.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
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
    private float volumeIndling = 0.3f;

    private float pitchForvard = 1f;
    private float pitchIndlong = 0.9f;
    private float pitchBack = 0.85f;    

    private int audioClipState;

    [Header("Audio curve")]
    [SerializeField]
    private AnimationCurve curve;

    private AudioSource audioSrc => gameObject.GetComponent<AudioSource>();

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
            case 0: audioSrc.volume = volumeForvard; audioSrc.pitch = curve.Evaluate(carController.vInput * 1.2f); break;
            case 1: audioSrc.volume = volumeBack; audioSrc.pitch = curve.Evaluate(carController.vInput * -1f); break;
            case 2: audioSrc.volume = volumeIndling; audioSrc.pitch = pitchIndlong; break;
        }
    }    
}
