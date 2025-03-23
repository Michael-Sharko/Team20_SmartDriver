using UnityEngine;

[CreateAssetMenu(fileName = "SingleSound", menuName = "Settings/Sounds/SingleSound")]
public class SingleSound : BaseGetSound
{
    [SerializeField] private AudioClip sound;

    public override AudioClip GetClip()
    {
        return sound;
    }
}
