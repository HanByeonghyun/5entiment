using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Dropdown graphicsDropdown;
    public Text graphicsQualityText; // 현재 그래픽 품질을 표시할 텍스트

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
        Debug.Log("그래픽 품질이 " + graphicsDropdown.options[qualityIndex].text + "로 설정되었습니다.");

        // 현재 품질 수준을 UI 텍스트에 반영
        if (graphicsQualityText != null)
        {
            graphicsQualityText.text = "현재 그래픽 품질: " + graphicsDropdown.options[qualityIndex].text;
        }
    }

    private void Start()
    {
        // 이전에 저장된 그래픽 품질 설정 불러오기
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
        // Dropdown의 초기값을 현재 그래픽 품질로 설정
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        // 초기 그래픽 품질 표시
        if (graphicsQualityText != null)
        {
            graphicsQualityText.text = "현재 그래픽 품질: " + graphicsDropdown.options[graphicsDropdown.value].text;
        }
    }

}
