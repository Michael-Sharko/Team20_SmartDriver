using System;
using System.Collections;
using UnityEngine;


//
// при такой реализации просто добавить
// GetComponentInChildren<BrakeMark>().Init(() => _isBreaking);
// на старте машинки и усе
//
// думаю нет особо смысла делать из этой штуки не монобех
[Serializable]
public class BrakeMark
{
    public delegate bool WhenToMark(float minAngle, float minSpeed);

    [SerializeField, Min(0)] private float minAngle = 10f;
    [SerializeField, Min(0)] private float minSpeed = 30f;
    [SerializeField] private BrakeMarkData data;

    private TrailRenderer[] _renderers;
    private WhenToMark _whenToMark;

    public void Init(MonoBehaviour behaviour, WhenToMark whenToMark)
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
            if (_whenToMark(minAngle, minSpeed))
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
