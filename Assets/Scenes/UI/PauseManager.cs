using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;            // �Ͻ����� �г�
    public GameObject settingsPanel;         // ���� �г�
    public GameObject quitConfirmPanel;     // ���� Ȯ�� �г�
    public Button continueButton;            // ����ϱ� ��ư
    public Button settingsButton;            // ���� ��ư
    public Button quitButton;                // �����ϱ� ��ư

    public bool isPaused = false;
    private KeyCode pauseKey;

    private static bool isLoadingScene = false; // �� �ε� ������ Ȯ��

    public void Start()
    {
        // ��ư �̺�Ʈ ����
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        // �г� �ʱ� ���� ����
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        LoadKeyBindings();

        SettingControl.OnKeyBindingsUpdated += LoadKeyBindings;

        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ ���
    }

    public void OnDestroy()
    {
        SettingControl.OnKeyBindingsUpdated -= LoadKeyBindings;

        SceneManager.sceneLoaded -= OnSceneLoaded; // �� �ε� �̺�Ʈ ����
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (settingsPanel.activeSelf)
            {
                // ���� �г��� Ȱ��ȭ�� ���¿��� ESC Ŭ�� �� ���� �г��� �ݰ� �Ͻ����� �гη� ���ư��� �Ѵ�.
                CloseSettings();
            }
            else 
            {
                TogglePause();
            }
        }
    }

    private void LoadKeyBindings()
    {
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("�Ͻ�����", KeyCode.Escape.ToString()));
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused); // Canvas Ȱ��ȭ/��Ȱ��ȭ
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            // �Ͻ����� ���¿��� ���� �� ����
            SaveCurrentScene();
        }
    }
    // ���� �� �̸� ����
    private void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();
        Debug.Log("���� �� ����: " + currentScene);
    }
    // 1. ����ϱ� ��ư ���
    public void ContinueGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }


    // 2. ���� �г� ����
    public void OpenSettings()
    {
        // ���� �г��� ��Ȱ��ȭ�Ǿ� �ִٸ� Ȱ��ȭ ��Ű��
        if (settingsPanel != null)
        {
            pausePanel.SetActive(false);         // �Ͻ����� �г��� �����.
            settingsPanel.SetActive(true);       // ���� �г��� �����ش�.
        }
        else
        {
            if (settingsPanel == null)
            {
                Debug.LogError("SettingsPanel is not assigned!");

            }
            else
            {
                Debug.LogError("SettingsPanel is called!");
                settingsPanel.SetActive(false);     // ���� �г��� �����.
                pausePanel.SetActive(true);         // �Ͻ����� �г��� �ٽ� �����ش�.
            }
        }
    }

    // ���� �г� �ݱ�
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // 3. �����ϱ� ��ư ���
    public void QuitGame()
    {
        quitConfirmPanel.SetActive(true);
    }
    // ���� �޴��� �̵�
    public void GoToMainMenu()
    {
        if (isLoadingScene)
        {
            Debug.LogWarning("�� �ε� ���Դϴ�. ���� �޴� �̵��� �ߴ��մϴ�.");
            return;
        }

        isLoadingScene = true;

        Debug.Log("���� �޴��� �̵� ����");
        Time.timeScale = 1; // �ð� �簳
        
        SaveCurrentScene(); // ���� �޴��� �̵� ���� ���� �� ����
        SceneManager.LoadScene("Main menu");
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main menu")
        {
            Debug.Log("���� �޴� �ε� �Ϸ�");
            isLoadingScene = false; // �ε� �Ϸ�
        }
    }
    // ���� ����
    public void ExitGame()
    {
        Application.Quit(); // ���� ����
        Debug.Log("������ ����Ǿ����ϴ�.");
    }
    //���� Ȯ��â �ݱ�
    public void CloseQuitPanel()
    {
        quitConfirmPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
