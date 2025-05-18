using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSoundFromArray", menuName = "Settings/Sounds/RandomSoundFromArray")]
public class RandomSoundFromArray : BaseGetSound
{
    [SerializeField, PathResources] private string[] soundPaths;

    private Dictionary<string, AudioClip> sounds = new();

    public override AudioClip GetClip()
    {
        var randomIndex = Random.Range(0, soundPaths.Length);
        var neededClipPath = soundPaths[randomIndex];
        if (sounds.TryGetValue(neededClipPath, out var clip))
        {
            return clip;
        }
        var neededClip = LoadClip(neededClipPath);
        sounds.Add(neededClipPath, neededClip);
        return neededClip;
    }
}
