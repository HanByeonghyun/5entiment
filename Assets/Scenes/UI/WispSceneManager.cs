using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WispSceneManager : MonoBehaviour
{
    public CanvasGroup WispImage; // 나타날 이미지의 CanvasGroup
    public TextMeshProUGUI subtitleText; // 자막 텍스트
    public GameObject subtitleBackground; // 자막 배경 패널
    public float fadeDuration = 1f; // 페이드 인/아웃 지속 시간
    public string[] subtitles; // 자막 텍스트 배열
    public float[] subtitleDisplayTimes; // 각 자막 표시 시간

    public void StartCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    private IEnumerator CutsceneSequence()
    {
        // 페이드 인
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // 자막 표시
        for (int i = 0; i < subtitles.Length; i++)
        {
            subtitleText.text = subtitles[i]; // 자막 텍스트 변경
            subtitleBackground.SetActive(true); // 배경 활성화
            subtitleText.gameObject.SetActive(true); // 자막 활성화
            yield return new WaitForSeconds(subtitleDisplayTimes[i]); // 자막 표시 시간 대기
            subtitleBackground.SetActive(false); // 배경 비활성화
            subtitleText.gameObject.SetActive(false); // 자막 비활성화
        }

        // 페이드 아웃
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            WispImage.alpha = alpha;
            yield return null;
        }

        WispImage.alpha = endAlpha;
    }
}
