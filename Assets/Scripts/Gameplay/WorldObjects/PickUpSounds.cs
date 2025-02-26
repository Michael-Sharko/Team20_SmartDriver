using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickUpSounds : MonoBehaviour {

    [SerializeField] private AudioClip pickUp;
    [SerializeField, Min(0)] private float volume = 1;

    private IPickupable pickupable;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        if (TryGetComponent(out pickupable)) pickupable.OnPickUp += PlaySoundPickUp;
    }
    private void OnDestroy() {
        if (pickupable != null) pickupable.OnPickUp -= PlaySoundPickUp;
    }

    public void PlaySoundPickUp() {
        audioSource.PlayOneShot(pickUp, volume);
    }
}