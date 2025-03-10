using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Google.Protobuf.WellKnownTypes;

public class SettingSound : MonoBehaviour
{
    public Slider masterSlider;  // ��ü ���� �����̴�
    public Slider bgmSlider;     // ����� �����̴�
    public Slider sfxSlider;     // ȿ���� �����̴�

    public Toggle mastermuteToggle;    // ��ü ���Ұ� ���
    public Toggle bgmmuteToggle;    // BGM ���Ұ� ���
    public Toggle sfxmuteToggle;    // ȿ���� ���Ұ� ���

    public AudioMixer audioMixer;   // Unity AudioMixer
    public List<AudioSource> bgmSources; // ����� ����� �ҽ� ����Ʈ
    public List<AudioSource> sfxSources; // ȿ���� ����� �ҽ� ����Ʈ

    public const string MasterVolumeParam = "MasterVolume";
    public const string BgmVolumeParam = "BgmVolume";
    public const string SfxVolumeParam = "SfxVolume";

    public float lastMasterVolume;     // ���Ұ� �� ������ ����
    public float lastBgmVolume;
    public float lastSfxVolume;

    public const float MinDecibel = -80f; // ���� ���Ұ� ���ú�
    public const float MaxDecibel = 0f;  // �ִ� ���ú�

    public void Start()
    {
        // �����̴� �ʱ�ȭ
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);

        // ���Ұ� �ʱ�ȭ
        mastermuteToggle.isOn = PlayerPrefs.GetInt("IsMasterMuted", 0) == 1;
        bgmmuteToggle.isOn = PlayerPrefs.GetInt("IsBgmMuted", 0) == 1;
        sfxmuteToggle.isOn = PlayerPrefs.GetInt("IsSfxMuted", 0) == 1;

        // �̺�Ʈ ����
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        mastermuteToggle.onValueChanged.AddListener(SetMasterMute);
        bgmmuteToggle.onValueChanged.AddListener(SetBgmMute);
        sfxmuteToggle.onValueChanged.AddListener(SetSfxMute);

        ApplySettings();
    }

    // ��ü ���� ����
    public void SetMasterVolume(float value)
    {
        if (mastermuteToggle.isOn)
        {
            // ���Ұ� ���¿����� -80dB�� ����
            audioMixer.SetFloat(MasterVolumeParam, MinDecibel);
            return;
        }
        // MasterVolume �����̴� ���� -80dB���� 0dB�� ��ȯ
        float masterDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(MasterVolumeParam, masterDecibelValue);

        // MasterVolume�� ���� BgmVolume �� SfxVolume�� ��������� ����
        float bgmValue = bgmSlider.value * value; // ����� ���� ���� ����
        float bgmDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, bgmValue);
        audioMixer.SetFloat(BgmVolumeParam, bgmDecibelValue);

        float sfxValue = sfxSlider.value * value; // ȿ���� ���� ���� ����
        float sfxDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, sfxValue);
        audioMixer.SetFloat(SfxVolumeParam, sfxDecibelValue);

        // MasterVolume �� ����
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    // ����� ���� ����
    public void SetBgmVolume(float value)
    {
        if (bgmmuteToggle.isOn || mastermuteToggle.isOn)
        {
            // ���Ұ� ���¿����� -80dB�� ����
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
            return;
        }
        float decibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(BgmVolumeParam, decibelValue);
        PlayerPrefs.SetFloat("BgmVolume", value);
        PlayerPrefs.Save();
    }

    // ȿ���� ���� ����
    public void SetSfxVolume(float value)
    {
        if (sfxmuteToggle.isOn || mastermuteToggle.isOn)
        {
            // ���Ұ� ���¿����� -80dB�� ����
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
            return;
        }
        float decibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(SfxVolumeParam, decibelValue);
        PlayerPrefs.SetFloat("SfxVolume", value);
        PlayerPrefs.Save();
    }

    // ��ü ���Ұ� ����
    public void SetMasterMute(bool isMuted)
    {
        if (isMuted)
        {
            // Master ���Ұ�: -80dB�� ����
            audioMixer.SetFloat(MasterVolumeParam, MinDecibel);
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
        }
        else
        {
            // Master ���Ұ� ����: �� �����̴� ���� ���� ���� ����
            SetMasterVolume(masterSlider.value);
        }

        // ���Ұ� ���� ����
        PlayerPrefs.SetInt("IsMasterMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ����� ���Ұ� ����
    public void SetBgmMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
        }
        else if (!mastermuteToggle.isOn)
        {
            // Master ���ҰŰ� �ƴ� ���� ����
            SetBgmVolume(bgmSlider.value);
        }

        PlayerPrefs.SetInt("IsBgmMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ȿ���� ���Ұ� ����
    public void SetSfxMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
        }
        else if (!mastermuteToggle.isOn)
        {
            // Master ���ҰŰ� �ƴ� ���� ����
            SetSfxVolume(sfxSlider.value);
        }
        PlayerPrefs.SetInt("IsSfxMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplySettings()
    {
        SetMasterVolume(masterSlider.value);
        SetBgmVolume(bgmSlider.value);
        SetSfxVolume(sfxSlider.value);

        SetMasterMute(mastermuteToggle.isOn);
        SetBgmMute(bgmmuteToggle.isOn);
        SetSfxMute(sfxmuteToggle.isOn);
    }
}
