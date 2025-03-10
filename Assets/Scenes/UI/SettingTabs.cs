using UnityEngine;

public class SettingsTabs : MonoBehaviour
{
    public GameObject Graphicpanel;
    public GameObject Soundpanel;
    public GameObject Controlpanel;
    public GameObject SettingsPanel; // 설정 패널 전체를 닫기 위한 참조

    private void Start()
    {
        ShowGraphicsSettings();
    }

    public void ShowGraphicsSettings()
    {
        Graphicpanel.SetActive(true);
        Soundpanel.SetActive(false);
        Controlpanel.SetActive(false);
    }

    public void ShowSoundSettings()
    {
        Graphicpanel.SetActive(false);
        Soundpanel.SetActive(true);
        Controlpanel.SetActive(false);
    }

    public void ShowControlsSettings()
    {
        Graphicpanel.SetActive(false);
        Soundpanel.SetActive(false);
        Controlpanel.SetActive(true);
    }
    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
    }
}
