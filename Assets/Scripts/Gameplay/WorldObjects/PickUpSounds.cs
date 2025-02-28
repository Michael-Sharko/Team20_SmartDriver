using System;
using UnityEngine;

[Serializable]
public class PickUpSounds {

    [SerializeField] private AudioClip pickUp;
    [SerializeField, Min(0)] private float volume = 1;

    private AudioSource audioSource;
    private IPickupable pickupable;

    public void Init(IPickupable pickupable, AudioSource audioSource) {
        this.audioSource = audioSource;

        if (this.pickupable != null) {
            this.pickupable.OnPickUp -= PlaySoundPickUp;
        }

        this.pickupable = pickupable;
        pickupable.OnPickUp += PlaySoundPickUp;
    }
    private void PlaySoundPickUp() {
        audioSource.PlayOneShot(pickUp, volume);
    }
}