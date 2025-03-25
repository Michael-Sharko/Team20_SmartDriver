using UnityEngine;

[CreateAssetMenu(fileName = "RandomSoundFromArray", menuName = "Settings/Sounds/RandomSoundFromArray")]
public class RandomSoundFromArray : BaseGetSound
{
    [SerializeField] private AudioClip[] sounds;

    public override AudioClip GetClip()
    {
        var randomIndex = UnityEngine.Random.Range(0, sounds.Length);
        return sounds[randomIndex];
    }
}
