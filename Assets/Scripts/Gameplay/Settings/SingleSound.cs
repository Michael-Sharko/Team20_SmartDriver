using UnityEngine;

[CreateAssetMenu(fileName = "SingleSound", menuName = "Settings/Sounds/SingleSound")]
public class SingleSound : BaseGetSound
{
    [SerializeField] private string soundPath;

    private AudioClip clip;

    public override AudioClip GetClip()
    {
        return clip != null ? clip : clip = LoadClip(soundPath);
    }
}
