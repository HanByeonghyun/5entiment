using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;            // 일시정지 패널
    public GameObject settingsPanel;         // 설정 패널
    public GameObject quitConfirmPanel;     // 종료 확인 패널
    public Button continueButton;            // 계속하기 버튼
    public Button settingsButton;            // 설정 버튼
    public Button quitButton;                // 종료하기 버튼

    public bool isPaused = false;
    private KeyCode pauseKey;

    private static bool isLoadingScene = false; // 씬 로딩 중인지 확인

    public void Start()
    {
        // 버튼 이벤트 연결
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        // 패널 초기 상태 설정
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        LoadKeyBindings();

        SettingControl.OnKeyBindingsUpdated += LoadKeyBindings;

        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
    }

    public void OnDestroy()
    {
        SettingControl.OnKeyBindingsUpdated -= LoadKeyBindings;

        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 해제
    }

    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (settingsPanel.activeSelf)
            {
                // 설정 패널이 활성화된 상태에서 ESC 클릭 시 설정 패널을 닫고 일시정지 패널로 돌아가게 한다.
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
        pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("일시정지", KeyCode.Escape.ToString()));
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused); // Canvas 활성화/비활성화
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            // 일시정지 상태에서 현재 씬 저장
            SaveCurrentScene();
        }
    }
    // 현재 씬 이름 저장
    private void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();
        Debug.Log("현재 씬 저장: " + currentScene);
    }
    // 1. 계속하기 버튼 기능
    public void ContinueGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }


    // 2. 설정 패널 열기
    public void OpenSettings()
    {
        // 설정 패널이 비활성화되어 있다면 활성화 시키고
        if (settingsPanel != null)
        {
            pausePanel.SetActive(false);         // 일시정지 패널을 숨긴다.
            settingsPanel.SetActive(true);       // 설정 패널을 보여준다.
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
                settingsPanel.SetActive(false);     // 설정 패널을 숨긴다.
                pausePanel.SetActive(true);         // 일시정지 패널을 다시 보여준다.
            }
        }
    }

    // 설정 패널 닫기
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // 3. 종료하기 버튼 기능
    public void QuitGame()
    {
        quitConfirmPanel.SetActive(true);
    }
    // 메인 메뉴로 이동
    public void GoToMainMenu()
    {
        if (isLoadingScene)
        {
            Debug.LogWarning("씬 로딩 중입니다. 메인 메뉴 이동을 중단합니다.");
            return;
        }

        isLoadingScene = true;

        Debug.Log("메인 메뉴로 이동 시작");
        Time.timeScale = 1; // 시간 재개
        
        SaveCurrentScene(); // 메인 메뉴로 이동 전에 현재 씬 저장
        SceneManager.LoadScene("Main menu");
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main menu")
        {
            Debug.Log("메인 메뉴 로드 완료");
            isLoadingScene = false; // 로딩 완료
        }
    }
    // 게임 종료
    public void ExitGame()
    {
        Application.Quit(); // 게임 종료
        Debug.Log("게임이 종료되었습니다.");
    }
    //종료 확인창 닫기
    public void CloseQuitPanel()
    {
        quitConfirmPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
