using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Dropdown graphicsDropdown;
    public Text graphicsQualityText; // ���� �׷��� ǰ���� ǥ���� �ؽ�Ʈ

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
        Debug.Log("�׷��� ǰ���� " + graphicsDropdown.options[qualityIndex].text + "�� �����Ǿ����ϴ�.");

        // ���� ǰ�� ������ UI �ؽ�Ʈ�� �ݿ�
        if (graphicsQualityText != null)
        {
            graphicsQualityText.text = "���� �׷��� ǰ��: " + graphicsDropdown.options[qualityIndex].text;
        }
    }

    private void Start()
    {
        // ������ ����� �׷��� ǰ�� ���� �ҷ�����
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
        // Dropdown�� �ʱⰪ�� ���� �׷��� ǰ���� ����
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        // �ʱ� �׷��� ǰ�� ǥ��
        if (graphicsQualityText != null)
        {
            graphicsQualityText.text = "���� �׷��� ǰ��: " + graphicsDropdown.options[graphicsDropdown.value].text;
        }
    }

}
