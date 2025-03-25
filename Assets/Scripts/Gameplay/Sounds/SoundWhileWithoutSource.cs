using System;
using UnityEngine;

[Serializable]
public class SoundWhileWithoutSource
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private BaseGetSound getSound;

    private Func<bool> _soundWhileTrue;

    public void Init(ref Action @event, Func<bool> soundWhileTrue)
    {
        @event += OnActivate;
        _soundWhileTrue = soundWhileTrue;
    }
    private void OnActivate()
    {
        var prefab = Resources.Load<PlaySoundsWhileConditionAndDestroy>("Prefabs/SoundWhile");
        PlaySoundsWhileConditionAndDestroy sound = UnityEngine.Object.Instantiate(prefab);
        sound.Play(getSound.GetClip(), _soundWhileTrue, volume);
    }
}
