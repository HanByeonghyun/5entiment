using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Google.Protobuf.WellKnownTypes;

public class SettingSound : MonoBehaviour
{
    public Slider masterSlider;  // 전체 음량 슬라이더
    public Slider bgmSlider;     // 배경음 슬라이더
    public Slider sfxSlider;     // 효과음 슬라이더

    public Toggle mastermuteToggle;    // 전체 음소거 토글
    public Toggle bgmmuteToggle;    // BGM 음소거 토글
    public Toggle sfxmuteToggle;    // 효과음 음소거 토글

    public AudioMixer audioMixer;   // Unity AudioMixer
    public List<AudioSource> bgmSources; // 배경음 오디오 소스 리스트
    public List<AudioSource> sfxSources; // 효과음 오디오 소스 리스트

    public const string MasterVolumeParam = "MasterVolume";
    public const string BgmVolumeParam = "BgmVolume";
    public const string SfxVolumeParam = "SfxVolume";

    public float lastMasterVolume;     // 음소거 전 마지막 볼륨
    public float lastBgmVolume;
    public float lastSfxVolume;

    public const float MinDecibel = -80f; // 완전 음소거 데시벨
    public const float MaxDecibel = 0f;  // 최대 데시벨

    public void Start()
    {
        // 슬라이더 초기화
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);

        // 음소거 초기화
        mastermuteToggle.isOn = PlayerPrefs.GetInt("IsMasterMuted", 0) == 1;
        bgmmuteToggle.isOn = PlayerPrefs.GetInt("IsBgmMuted", 0) == 1;
        sfxmuteToggle.isOn = PlayerPrefs.GetInt("IsSfxMuted", 0) == 1;

        // 이벤트 연결
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        mastermuteToggle.onValueChanged.AddListener(SetMasterMute);
        bgmmuteToggle.onValueChanged.AddListener(SetBgmMute);
        sfxmuteToggle.onValueChanged.AddListener(SetSfxMute);

        ApplySettings();
    }

    // 전체 음량 설정
    public void SetMasterVolume(float value)
    {
        if (mastermuteToggle.isOn)
        {
            // 음소거 상태에서는 -80dB로 고정
            audioMixer.SetFloat(MasterVolumeParam, MinDecibel);
            return;
        }
        // MasterVolume 슬라이더 값을 -80dB에서 0dB로 변환
        float masterDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(MasterVolumeParam, masterDecibelValue);

        // MasterVolume에 따라 BgmVolume 및 SfxVolume을 상대적으로 조정
        float bgmValue = bgmSlider.value * value; // 배경음 볼륨 비율 조정
        float bgmDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, bgmValue);
        audioMixer.SetFloat(BgmVolumeParam, bgmDecibelValue);

        float sfxValue = sfxSlider.value * value; // 효과음 볼륨 비율 조정
        float sfxDecibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, sfxValue);
        audioMixer.SetFloat(SfxVolumeParam, sfxDecibelValue);

        // MasterVolume 값 저장
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    // 배경음 볼륨 설정
    public void SetBgmVolume(float value)
    {
        if (bgmmuteToggle.isOn || mastermuteToggle.isOn)
        {
            // 음소거 상태에서는 -80dB로 고정
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
            return;
        }
        float decibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(BgmVolumeParam, decibelValue);
        PlayerPrefs.SetFloat("BgmVolume", value);
        PlayerPrefs.Save();
    }

    // 효과음 볼륨 설정
    public void SetSfxVolume(float value)
    {
        if (sfxmuteToggle.isOn || mastermuteToggle.isOn)
        {
            // 음소거 상태에서는 -80dB로 고정
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
            return;
        }
        float decibelValue = Mathf.Lerp(MinDecibel, MaxDecibel, value);
        audioMixer.SetFloat(SfxVolumeParam, decibelValue);
        PlayerPrefs.SetFloat("SfxVolume", value);
        PlayerPrefs.Save();
    }

    // 전체 음소거 설정
    public void SetMasterMute(bool isMuted)
    {
        if (isMuted)
        {
            // Master 음소거: -80dB로 설정
            audioMixer.SetFloat(MasterVolumeParam, MinDecibel);
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
        }
        else
        {
            // Master 음소거 해제: 각 슬라이더 값에 따라 볼륨 복원
            SetMasterVolume(masterSlider.value);
        }

        // 음소거 상태 저장
        PlayerPrefs.SetInt("IsMasterMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 배경음 음소거 설정
    public void SetBgmMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat(BgmVolumeParam, MinDecibel);
        }
        else if (!mastermuteToggle.isOn)
        {
            // Master 음소거가 아닐 때만 복원
            SetBgmVolume(bgmSlider.value);
        }

        PlayerPrefs.SetInt("IsBgmMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 효과음 음소거 설정
    public void SetSfxMute(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat(SfxVolumeParam, MinDecibel);
        }
        else if (!mastermuteToggle.isOn)
        {
            // Master 음소거가 아닐 때만 복원
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
