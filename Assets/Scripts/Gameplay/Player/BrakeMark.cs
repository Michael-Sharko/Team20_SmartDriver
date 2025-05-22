using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class BrakeMark
{
    [SerializeField] private BrakeMarkData data;

    private TrailRenderer[] _renderers;
    private Func<bool> _whenToMark;

    public void Init(MonoBehaviour behaviour, Func<bool> whenToMark)
    {
        _renderers = behaviour.GetComponentsInChildren<TrailRenderer>();
        _whenToMark = whenToMark;

        foreach (var renderer in _renderers)
        {
            renderer.startWidth = data.Width;
            renderer.time = data.Time;
            renderer.material = data.Material;
        }

        Off();

        if (_whenToMark != null)
            behaviour.StartCoroutine(UpdateEmitting());
    }
    private IEnumerator UpdateEmitting()
    {
        while (true)
        {
            if (_whenToMark())
            {
                On();
            }
            else
            {
                Off();
            }

            yield return null;
        }
    }
    //[ContextMenu("On")]
    public void On()
    {
        foreach (var renderer in _renderers)
        {
            renderer.emitting = true;
        }
    }
    //[ContextMenu("Off")]
    public void Off()
    {
        foreach (var renderer in _renderers)
        {
            renderer.emitting = false;
        }
    }
}
