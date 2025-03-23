using System;
using UnityEngine;

[Serializable]
public class SoundOnEventWithoutSource
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private BaseGetSound getSound;

    public void Init(ref Action @event)
    {
        @event += OnActivate;
    }
    private void OnActivate()
    {
        var prefab = Resources.Load<PlaySoundsAndDestroy>("Prefabs/Sound");
        PlaySoundsAndDestroy sound = UnityEngine.Object.Instantiate(prefab);
        sound.Play(getSound.GetClip(), volume);
    }
}
