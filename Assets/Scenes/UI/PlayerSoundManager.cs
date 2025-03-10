using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip attackSound;     // ���� �Ҹ�
    public AudioClip hitSound;        // �ǰ� �Ҹ�
    public AudioClip jumpSound;       // ���� �Ҹ�
    public AudioClip rollSound;       // ������ �Ҹ�

    [Header("Audio Settings")]
    public AudioMixer audioMixer;         // AudioMixer ����
    public const string SfxVolumeParam = "SfxVolume"; // AudioMixer�� SFX ���� �Ķ����

    private AudioSource audioSource;

    public void Start()
    {
        // AudioSource �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource�� �÷��̾� ������Ʈ�� �����ϴ�!");
        }

        // AudioMixer �׷� ����
        if (audioSource != null && audioMixer != null)
        {
            var sfxGroup = audioMixer.FindMatchingGroups("SFX")[0];
            audioSource.outputAudioMixerGroup = sfxGroup;
        }
    }

    // ���� �� ȿ���� ���
    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    // �ǰ� �� ȿ���� ���
    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    // ���� �� ȿ���� ���
    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    // ������ �� ȿ���� ���
    public void PlayRollSound()
    {
        if (rollSound != null)
        {
            audioSource.PlayOneShot(rollSound);
        }
    }
}
