using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject logoutPanel; // 로그아웃 확인 창

    public void OpenLogoutPanel()
    {
        // 로그아웃 확인 창 표시
        if (logoutPanel != null)
        {
            logoutPanel.SetActive(true);
        }
    }

    public void CloseLogoutPanel()
    {
        // 로그아웃 확인 창 닫기
        if (logoutPanel != null)
        {
            logoutPanel.SetActive(false);
        }
    }

    public void LogoutAndReturnToLogin()
    {
        //// 로그아웃 처리 (예: 토큰 삭제)
        //PlayerPrefs.DeleteKey("UserToken"); // 예: 세션 토큰 삭제
        //PlayerPrefs.Save();

        // 로그인 씬으로 이동
        SceneManager.LoadScene("Login");
    }
}
