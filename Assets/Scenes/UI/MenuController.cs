using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject NewGameConfirmationPanel; // 새 게임 시작 확인 창
    public GameObject SettingsPanel;            // 설정 창 패널
    public GameObject QuitConfirmationPanel;    // 게임 종료 확인 창 패널

    public GameObject BugReportConfirmationPanel; // 버그 제보 확인 창
    public GameObject LogoutConfirmationPanel;   // 로그아웃 확인 창

    public void Start()
    {
        NewGameConfirmationPanel.SetActive(false); // 새 게임 시작 확인 창 비활성화
        SettingsPanel.SetActive(false);
        QuitConfirmationPanel.SetActive(false);
        BugReportConfirmationPanel.SetActive(false);
        LogoutConfirmationPanel.SetActive(false);

    }
    public void OpenNewGameConfirmation()
    {
        NewGameConfirmationPanel.SetActive(true);
    }

    // 새 게임 시작 확인 창 닫기
    public void CloseNewGameConfirmation()
    {
        NewGameConfirmationPanel.SetActive(false);
    }

    // 새 게임 시작
    public void StartNewGame()
    {
        Debug.Log("새 게임을 시작합니다.");
        // 여기에 새 게임을 초기화하거나 게임을 재시작하는 로직 추가
        SceneManager.LoadScene("Tutorial0");
    }

    // SettingsPanel 열기
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }

    // QuitConfirmationPanel 열기
    public void OpenQuitConfirmation()
    {
        QuitConfirmationPanel.SetActive(true);
    }

    // QuitConfirmationPanel 닫기
    public void CloseQuitConfirmation()
    {
        QuitConfirmationPanel.SetActive(false);
    }
    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }
    // --- 버그 제보 기능 (임시) ---
    public void OpenBugReportConfirmation()
    {
        BugReportConfirmationPanel.SetActive(true);
    }

    public void CloseBugReportConfirmation()
    {
        BugReportConfirmationPanel.SetActive(false);
    }

    // --- 로그아웃 기능 ---
    public void OpenLogoutConfirmation()
    {
        LogoutConfirmationPanel.SetActive(true);
    }

    public void CloseLogoutConfirmation()
    {
        LogoutConfirmationPanel.SetActive(false);
    }

    public void Logout()
    {
        Debug.Log("로그아웃 완료. 로그인 화면으로 이동합니다.");
        PlayerPrefs.DeleteKey("UserToken"); // 세션 초기화
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoginScene");
    }
    public void DeleteAccount()
    {
        Debug.Log("회원탈퇴 실행. URL 열기 및 게임 종료.");
        //Application.OpenURL("https://yourwebsite.com/delete-account"); // 웹사이트 열기
        Application.Quit(); // 게임 종료
    }
}