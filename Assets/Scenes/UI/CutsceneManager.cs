using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneStep
    {
        public Sprite backgroundImage;       // ��� �̹���
        public string[] dialogues;           // ���� �ؽ�Ʈ
        public AudioClip audioClip;          // ����
    }


    [Header("Cutscene Settings")]
    public List<CutsceneStep> cutsceneSteps; // �ƾ� �ܰ� ����Ʈ
    public GameObject cutsceneCanvas;       // �ƾ� UI ĵ����
    public Image backgroundImage;           // ��� �̹���
    public TMP_Text cutsceneText;           // �ؽ�Ʈ ������Ʈ
    public TMP_Text skipHint;               // ��ŵ ��Ʈ
    public GameObject subtitleBackground;   // �ڸ� ��� �г�
    public GameObject gameplayElements;     // �ƾ� �� ��Ȱ��ȭ�� �����÷��� ���
    public CanvasGroup blackBackground;     // BlackBackground CanvasGroup

    private CanvasGroup imageCanvasGroup;    // �̹��� ���̵带 ���� CanvasGroup
    private int currentIndex = 0;           // ���� �ƾ� �ܰ� �ε���
    private int currentDialogueIndex = 0;  // ���� �ؽ�Ʈ �ε���
    private bool isCutsceneActive = false; // �ƾ� Ȱ��ȭ ����
    private bool isTransitioning = false; // ���� ��ȯ ������ ����
    public AudioSource audioSource;        // ���� ����� AudioSource
    public AudioMixer audioMixer;            // AudioMixer ����
    public void Start()
    {
        // AudioSource ������Ʈ �߰�
        audioSource = gameObject.AddComponent<AudioSource>();

        // CanvasGroup �ʱ�ȭ
        imageCanvasGroup = backgroundImage.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
        {
            imageCanvasGroup = backgroundImage.gameObject.AddComponent<CanvasGroup>();
        }
        imageCanvasGroup.alpha = 0f; // �ʱⰪ�� �����ϰ� ����

        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(false); // �⺻������ ��Ȱ��ȭ
        }
        // AudioSource ���� ���� �� CutsceneCanvas�� �߰�
        if (audioSource == null)
        {
            audioSource = cutsceneCanvas.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = cutsceneCanvas.AddComponent<AudioSource>();
            }
        }

        // Output ������ AudioSource �ʱ�ȭ �ķ� ����
        Invoke("ForceSetOutputToBGM", 0.1f);

        StartCutscene();
    }
    public void ForceSetOutputToBGM()
    {
        if (audioSource != null && audioMixer != null)
        {
            var bgmGroup = audioMixer.FindMatchingGroups("BGM");
            if (bgmGroup.Length > 0)
            {
                audioSource.outputAudioMixerGroup = bgmGroup[0];
                Debug.Log("AudioSource Output�� BGM���� �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("BGM �׷��� AudioMixer���� ã�� �� �����ϴ�!");
            }
        }
    }
    public void Update()
    {
        if (!isCutsceneActive) return;

        // Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceCutscene(); // ���� �ܰ�� ����
        }
    }

    public void StartCutscene()
    {
        if (cutsceneCanvas == null)
        {
            Debug.LogError("CutsceneCanvas�� �������� �ʾҽ��ϴ�!");
            return;
        }

        cutsceneCanvas.SetActive(true); // ������ Ȱ��ȭ
        gameplayElements.SetActive(false);
        isCutsceneActive = true;

        currentIndex = 0;
        currentDialogueIndex = 0;

        UpdateCutsceneStep();
    }

    void UpdateCutsceneStep()
    {
        if (currentIndex >= cutsceneSteps.Count) // �ƾ��� �������� Ȯ��
        {
            EndCutscene();
            return;
        }
        CutsceneStep currentStep = cutsceneSteps[currentIndex];

        // ��� �̹��� ������Ʈ
        if (backgroundImage.sprite != currentStep.backgroundImage)
        {
            StartCoroutine(UpdateImageWithFade(currentStep.backgroundImage));
        }


        // �ؽ�Ʈ ������Ʈ
        if (cutsceneText != null && currentStep.dialogues != null && currentDialogueIndex < currentStep.dialogues.Length)
        {
            cutsceneText.text = currentStep.dialogues[currentDialogueIndex];
        }

        // ����� ������Ʈ
        if (currentStep.audioClip != null)
        {
            PlayBgm(currentStep.audioClip);
        }

    }
    public void PlayBgm(AudioClip clip)
    {
        if (clip == null)
        {
            audioSource.Stop();
            return;
        }

        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    void AdvanceCutscene()
    {
        if (isTransitioning) return; // �̹� ��ȯ ���̶�� ����

        if (currentIndex < cutsceneSteps.Count)
        {
            if (currentDialogueIndex < cutsceneSteps[currentIndex].dialogues.Length - 1)
            {
                currentDialogueIndex++;
            }
            else
            {
                // �ؽ�Ʈ�� ��� ��µǾ��ٸ� ���� �ƾ� �ܰ��
                currentDialogueIndex = 0;
                currentIndex++;
            }

            UpdateCutsceneStep();
        }
    }

    IEnumerator UpdateImageWithFade(Sprite newImage)
    {
        isTransitioning = true;

        // ���� ��� ���̵��� (��� ����)
        if (blackBackground != null)
        {
            while (blackBackground.alpha < 1f)
            {
                blackBackground.alpha += Time.deltaTime / 1f; // 1�� ���� ���̵���
                yield return null;
            }
        }

        // �̹��� ��ü �� ���̵�ƿ�
        if (imageCanvasGroup != null)
        {
            while (imageCanvasGroup.alpha > 0f)
            {
                imageCanvasGroup.alpha -= Time.deltaTime / 1f;
                yield return null;
            }

            // �̹��� ������Ʈ
            backgroundImage.sprite = newImage;

            // �̹��� ���̵���
            while (imageCanvasGroup.alpha < 1f)
            {
                imageCanvasGroup.alpha += Time.deltaTime / 1f;
                yield return null;
            }
        }
        // ���� ��� ���̵�ƿ� (��� ����)
        if (blackBackground != null)
        {
            while (blackBackground.alpha > 0f)
            {
                blackBackground.alpha -= Time.deltaTime / 1f; // 1�� ���� ���̵�ƿ�
                yield return null;
            }
        }

        isTransitioning = false;
    }
    public void EndCutscene()
    {
        isCutsceneActive = false;
        cutsceneCanvas.SetActive(false); // CutsceneCanvas ��Ȱ��ȭ
        skipHint.gameObject.SetActive(false); // ��ŵ ��Ʈ ��Ȱ��ȭ
        gameplayElements.SetActive(true); // �����÷��� ��� Ȱ��ȭ
        audioSource.Stop();

        currentIndex = 0;
        currentDialogueIndex = 0;

        // �ڸ� ��� ��Ȱ��ȭ
        if (subtitleBackground != null)
        {
            subtitleBackground.SetActive(false);
        }
    }

}
