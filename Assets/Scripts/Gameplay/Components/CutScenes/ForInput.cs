using Shark.Gameplay.Player;
using UnityEngine;
using UnityEngine.Playables;

public class ForInput : PlayableBehaviour
{
    private ManualInput input;
    public float h = 0;
    public float v = 0;
    public bool s;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (input == null)
        {
            input = Object.FindObjectOfType<ManualInput>();
        }

        input.HInput = h;
        input.VInput = v;
        input.SpaceInput = s;
    }
}
