using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeController
{

    private AudioMixer mixer;
    private AudioSetting setting;

    private const string MUSIC_MIXER_GROUP_NAME = "MusicVolume";
    private const string SOUNDS_MIXER_GROUP_NAME = "SoundsVolume";
    private const string MIXER_PATH = "Audio/mixer";
    private const string SETTING_PATH = "Audio/AudioSetting";


    public void ChangeMusicVolume(float normalizeValue)
    {

        mixer.SetFloat(MUSIC_MIXER_GROUP_NAME, NormalizeValueToVolume(normalizeValue));

    }
    public void ChangeSoundsVolume(float normalizeValue)
    {

        mixer.SetFloat(SOUNDS_MIXER_GROUP_NAME, NormalizeValueToVolume(normalizeValue));

    }

    // ��������� � ������� ���������� �� -80 �������(����� ���) �� 0 �������(���������� ����)
    // �������� � ��� ��� ��������� ����� ���������� ������������ (�.�. -20�� ���� ������ ����� �� 1/4 ����,
    // �� ��� ���������� ����� �� ������), ������� ����� �������� �������������� ���������, ��� ����� � 0��,
    // ��� ��������� ������������ � ��������������� 1, ���� ����������� �� �������:

    //       ^
    //  0��  |                               +         +
    //       |                       +    
    //       |                 +
    //       |              +
    //       |           +
    //       |        +
    //       |     +
    //       |   +
    //       |  +
    //       | +
    //       |+
    // -80�� |+
    //        ------------------------------------------>
    //       0                                          1
    private float NormalizeValueToVolume(float normalizeValue)
        => Mathf.Lerp(-80, 0, Mathf.Pow(normalizeValue, .1f));


    public AudioVolumeController()
    {

        setting = Resources.Load<AudioSetting>(SETTING_PATH);
        mixer = Resources.Load<AudioMixer>(MIXER_PATH);

        setting.OnChangeMusicVolume += ChangeMusicVolume;
        setting.OnChangeSoundsVolume += ChangeSoundsVolume;

        ChangeMusicVolume(setting.MusicVolume);
        ChangeSoundsVolume(setting.SoundsVolume);

    }
}