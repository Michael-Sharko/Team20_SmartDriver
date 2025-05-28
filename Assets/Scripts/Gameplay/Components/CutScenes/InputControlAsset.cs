using UnityEngine;
using UnityEngine.Playables;

public class InputControlAsset : PlayableAsset
{
    public float h = 0;
    public float v = 0;
    public bool s;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ForInput>.Create(graph);

        var ManualInputController = playable.GetBehaviour();
        ManualInputController.h = h;
        ManualInputController.v = v;
        ManualInputController.s = s;

        return playable;
    }
}
