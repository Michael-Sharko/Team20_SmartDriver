using UnityEngine;
using UnityEngine.UI;

public class SlidersAudioVolumeController : MonoBehaviour {

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    private const string AUDIO_SETTING_PATH = "Audio/AudioSetting";

    private AudioSetting setting;


    private void Awake() {

        setting = Resources.Load<AudioSetting>(AUDIO_SETTING_PATH);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(SetSoundsVolume);

        musicSlider.value = setting.MusicVolume;
        soundsSlider.value = setting.SoundsVolume;

    }

    public void SetMusicVolume(float value) {

        setting.MusicVolume = value;

    }
    public void SetSoundsVolume(float value) {

        setting.SoundsVolume = value;

    }
}