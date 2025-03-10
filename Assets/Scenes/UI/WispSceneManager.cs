using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WispSceneManager : MonoBehaviour
{
    public CanvasGroup WispImage; // ��Ÿ�� �̹����� CanvasGroup
    public TextMeshProUGUI subtitleText; // �ڸ� �ؽ�Ʈ
    public GameObject subtitleBackground; // �ڸ� ��� �г�
    public float fadeDuration = 1f; // ���̵� ��/�ƿ� ���� �ð�
    public string[] subtitles; // �ڸ� �ؽ�Ʈ �迭
    public float[] subtitleDisplayTimes; // �� �ڸ� ǥ�� �ð�

    public void StartCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    private IEnumerator CutsceneSequence()
    {
        // ���̵� ��
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // �ڸ� ǥ��
        for (int i = 0; i < subtitles.Length; i++)
        {
            subtitleText.text = subtitles[i]; // �ڸ� �ؽ�Ʈ ����
            subtitleBackground.SetActive(true); // ��� Ȱ��ȭ
            subtitleText.gameObject.SetActive(true); // �ڸ� Ȱ��ȭ
            yield return new WaitForSeconds(subtitleDisplayTimes[i]); // �ڸ� ǥ�� �ð� ���
            subtitleBackground.SetActive(false); // ��� ��Ȱ��ȭ
            subtitleText.gameObject.SetActive(false); // �ڸ� ��Ȱ��ȭ
        }

        // ���̵� �ƿ�
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
