using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeController
{
    private const string MIXER_PATH = "Audio/mixer";
    private const string SETTING_PATH = "Audio/AudioSetting";

    private readonly AudioMixer mixer;
    private readonly AudioSetting setting;


    public AudioVolumeController()
    {
        setting = Resources.Load<AudioSetting>(SETTING_PATH);
        mixer = Resources.Load<AudioMixer>(MIXER_PATH);

        for (AudioVolumeID i = 0; i < AudioVolumeID.Count; i++)
        {
            var parameter = setting.GetParameter(i);
            UpdateMixerParameter(parameter);
            parameter.OnChangeVolume += UpdateMixerParameter;
        }
    }

    private void UpdateMixerParameter(AudioVolumeParameter obj)
    {
        mixer.SetFloat(obj.key, NormalizeValueToVolume(obj.Volume));
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
}