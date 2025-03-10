using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip attackSound;     // 공격 소리
    public AudioClip hitSound;        // 피격 소리
    public AudioClip jumpSound;       // 점프 소리
    public AudioClip rollSound;       // 구르기 소리

    [Header("Audio Settings")]
    public AudioMixer audioMixer;         // AudioMixer 참조
    public const string SfxVolumeParam = "SfxVolume"; // AudioMixer의 SFX 볼륨 파라미터

    private AudioSource audioSource;

    public void Start()
    {
        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 플레이어 오브젝트에 없습니다!");
        }

        // AudioMixer 그룹 설정
        if (audioSource != null && audioMixer != null)
        {
            var sfxGroup = audioMixer.FindMatchingGroups("SFX")[0];
            audioSource.outputAudioMixerGroup = sfxGroup;
        }
    }

    // 공격 시 효과음 재생
    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    // 피격 시 효과음 재생
    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    // 점프 시 효과음 재생
    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    // 구르기 시 효과음 재생
    public void PlayRollSound()
    {
        if (rollSound != null)
        {
            audioSource.PlayOneShot(rollSound);
        }
    }
}
