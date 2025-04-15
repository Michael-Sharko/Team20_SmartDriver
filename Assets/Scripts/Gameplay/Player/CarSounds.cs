using System;
using Shark.Gameplay.Player;
using UnityEngine;

[Serializable]
public class CarSounds
{
    [SerializeField] private float _fuelValueForActivateLowLevelFuelSound = 30f;
    [SerializeField] private SoundWhile _lowLevelFuelSound;

    public void Init(MonoBehaviour coroutineOwner, AudioSource source, Get<float> currentFuel)
    {
        _lowLevelFuelSound.Init(
            () => currentFuel.Value <= _fuelValueForActivateLowLevelFuelSound,
            source,
            coroutineOwner);
    }


    //public AudioClip engineWorkClip;
    //public AudioClip[] collisionClips;
    //public AudioClip destroyClip;

    //public AudioClip[] roadClips;
    //public AudioClip[] sandClips;
    //public AudioClip[] stoneClips;

}



