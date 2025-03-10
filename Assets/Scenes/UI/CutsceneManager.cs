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
        public Sprite backgroundImage;       // 배경 이미지
        public string[] dialogues;           // 여러 텍스트
        public AudioClip audioClip;          // 사운드
    }


    [Header("Cutscene Settings")]
    public List<CutsceneStep> cutsceneSteps; // 컷씬 단계 리스트
    public GameObject cutsceneCanvas;       // 컷씬 UI 캔버스
    public Image backgroundImage;           // 배경 이미지
    public TMP_Text cutsceneText;           // 텍스트 컴포넌트
    public TMP_Text skipHint;               // 스킵 힌트
    public GameObject subtitleBackground;   // 자막 배경 패널
    public GameObject gameplayElements;     // 컷씬 중 비활성화할 게임플레이 요소
    public CanvasGroup blackBackground;     // BlackBackground CanvasGroup

    private CanvasGroup imageCanvasGroup;    // 이미지 페이드를 위한 CanvasGroup
    private int currentIndex = 0;           // 현재 컷씬 단계 인덱스
    private int currentDialogueIndex = 0;  // 현재 텍스트 인덱스
    private bool isCutsceneActive = false; // 컷씬 활성화 여부
    private bool isTransitioning = false; // 현재 전환 중인지 여부
    public AudioSource audioSource;        // 사운드 재생용 AudioSource
    public AudioMixer audioMixer;            // AudioMixer 연결
    public void Start()
    {
        // AudioSource 컴포넌트 추가
        audioSource = gameObject.AddComponent<AudioSource>();

        // CanvasGroup 초기화
        imageCanvasGroup = backgroundImage.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
        {
            imageCanvasGroup = backgroundImage.gameObject.AddComponent<CanvasGroup>();
        }
        imageCanvasGroup.alpha = 0f; // 초기값을 투명하게 설정

        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(false); // 기본적으로 비활성화
        }
        // AudioSource 동적 생성 및 CutsceneCanvas에 추가
        if (audioSource == null)
        {
            audioSource = cutsceneCanvas.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = cutsceneCanvas.AddComponent<AudioSource>();
            }
        }

        // Output 설정을 AudioSource 초기화 후로 강제
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
                Debug.Log("AudioSource Output이 BGM으로 설정되었습니다.");
            }
            else
            {
                Debug.LogWarning("BGM 그룹을 AudioMixer에서 찾을 수 없습니다!");
            }
        }
    }
    public void Update()
    {
        if (!isCutsceneActive) return;

        // 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceCutscene(); // 다음 단계로 진행
        }
    }

    public void StartCutscene()
    {
        if (cutsceneCanvas == null)
        {
            Debug.LogError("CutsceneCanvas가 설정되지 않았습니다!");
            return;
        }

        cutsceneCanvas.SetActive(true); // 강제로 활성화
        gameplayElements.SetActive(false);
        isCutsceneActive = true;

        currentIndex = 0;
        currentDialogueIndex = 0;

        UpdateCutsceneStep();
    }

    void UpdateCutsceneStep()
    {
        if (currentIndex >= cutsceneSteps.Count) // 컷씬이 끝났는지 확인
        {
            EndCutscene();
            return;
        }
        CutsceneStep currentStep = cutsceneSteps[currentIndex];

        // 배경 이미지 업데이트
        if (backgroundImage.sprite != currentStep.backgroundImage)
        {
            StartCoroutine(UpdateImageWithFade(currentStep.backgroundImage));
        }


        // 텍스트 업데이트
        if (cutsceneText != null && currentStep.dialogues != null && currentDialogueIndex < currentStep.dialogues.Length)
        {
            cutsceneText.text = currentStep.dialogues[currentDialogueIndex];
        }

        // 배경음 업데이트
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
        if (isTransitioning) return; // 이미 전환 중이라면 무시

        if (currentIndex < cutsceneSteps.Count)
        {
            if (currentDialogueIndex < cutsceneSteps[currentIndex].dialogues.Length - 1)
            {
                currentDialogueIndex++;
            }
            else
            {
                // 텍스트가 모두 출력되었다면 다음 컷씬 단계로
                currentDialogueIndex = 0;
                currentIndex++;
            }

            UpdateCutsceneStep();
        }
    }

    IEnumerator UpdateImageWithFade(Sprite newImage)
    {
        isTransitioning = true;

        // 검은 배경 페이드인 (배경 덮음)
        if (blackBackground != null)
        {
            while (blackBackground.alpha < 1f)
            {
                blackBackground.alpha += Time.deltaTime / 1f; // 1초 동안 페이드인
                yield return null;
            }
        }

        // 이미지 교체 및 페이드아웃
        if (imageCanvasGroup != null)
        {
            while (imageCanvasGroup.alpha > 0f)
            {
                imageCanvasGroup.alpha -= Time.deltaTime / 1f;
                yield return null;
            }

            // 이미지 업데이트
            backgroundImage.sprite = newImage;

            // 이미지 페이드인
            while (imageCanvasGroup.alpha < 1f)
            {
                imageCanvasGroup.alpha += Time.deltaTime / 1f;
                yield return null;
            }
        }
        // 검은 배경 페이드아웃 (배경 제거)
        if (blackBackground != null)
        {
            while (blackBackground.alpha > 0f)
            {
                blackBackground.alpha -= Time.deltaTime / 1f; // 1초 동안 페이드아웃
                yield return null;
            }
        }

        isTransitioning = false;
    }
    public void EndCutscene()
    {
        isCutsceneActive = false;
        cutsceneCanvas.SetActive(false); // CutsceneCanvas 비활성화
        skipHint.gameObject.SetActive(false); // 스킵 힌트 비활성화
        gameplayElements.SetActive(true); // 게임플레이 요소 활성화
        audioSource.Stop();

        currentIndex = 0;
        currentDialogueIndex = 0;

        // 자막 배경 비활성화
        if (subtitleBackground != null)
        {
            subtitleBackground.SetActive(false);
        }
    }

}
