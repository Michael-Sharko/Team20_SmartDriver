using System;
using Shark.Gameplay.Player;
using UnityEngine;

[Serializable]
public class CarSounds
{
    [SerializeField] private float _fuelValueForActivateLowLevelFuelSound = 30f;
    [SerializeField] private SoundWhile _lowLevelFuelSound;

    [SerializeField] private SoundWhile _skidSound;

    public void Init(MonoBehaviour coroutineOwner, Get<float> currentFuel,
        Func<bool> whenToSound)
    {
        _lowLevelFuelSound.Init(
            () => currentFuel.Value <= _fuelValueForActivateLowLevelFuelSound,
            PlaySound2D.GetNewSource("Low Level"),
            coroutineOwner);

        _skidSound.Init(whenToSound, PlaySound2D.GetNewSource("Skid"), coroutineOwner, true, true);
    }
}



