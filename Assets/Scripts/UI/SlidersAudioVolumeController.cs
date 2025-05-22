using Scripts.UI.Widgets;
using UnityEngine;

public class SlidersAudioVolumeController : MonoBehaviour
{
    [SerializeField] private AudioSliderWidget[] widgets;

    private AudioSetting setting;

    private void Start()
    {
        setting = Resources.Load<AudioSetting>(PathResources.Audio.AudioSetting);

        foreach (var widget in widgets)
        {
            widget.Slider.value = setting.GetParameter(widget.ID).Volume;

            widget.Slider.onValueChanged.AddListener(
                (value) => setting.GetParameter(widget.ID).Volume = value);
        }
    }
}