using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject NewGameConfirmationPanel; // �� ���� ���� Ȯ�� â
    public GameObject SettingsPanel;            // ���� â �г�
    public GameObject QuitConfirmationPanel;    // ���� ���� Ȯ�� â �г�

    public GameObject BugReportConfirmationPanel; // ���� ���� Ȯ�� â
    public GameObject LogoutConfirmationPanel;   // �α׾ƿ� Ȯ�� â

    public void Start()
    {
        NewGameConfirmationPanel.SetActive(false); // �� ���� ���� Ȯ�� â ��Ȱ��ȭ
        SettingsPanel.SetActive(false);
        QuitConfirmationPanel.SetActive(false);
        BugReportConfirmationPanel.SetActive(false);
        LogoutConfirmationPanel.SetActive(false);

    }
    public void OpenNewGameConfirmation()
    {
        NewGameConfirmationPanel.SetActive(true);
    }

    // �� ���� ���� Ȯ�� â �ݱ�
    public void CloseNewGameConfirmation()
    {
        NewGameConfirmationPanel.SetActive(false);
    }

    // �� ���� ����
    public void StartNewGame()
    {
        Debug.Log("�� ������ �����մϴ�.");
        // ���⿡ �� ������ �ʱ�ȭ�ϰų� ������ ������ϴ� ���� �߰�
        SceneManager.LoadScene("Tutorial0");
    }

    // SettingsPanel ����
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }

    // QuitConfirmationPanel ����
    public void OpenQuitConfirmation()
    {
        QuitConfirmationPanel.SetActive(true);
    }

    // QuitConfirmationPanel �ݱ�
    public void CloseQuitConfirmation()
    {
        QuitConfirmationPanel.SetActive(false);
    }
    // ���� ����
    public void QuitGame()
    {
        Application.Quit();
    }
    // --- ���� ���� ��� (�ӽ�) ---
    public void OpenBugReportConfirmation()
    {
        BugReportConfirmationPanel.SetActive(true);
    }

    public void CloseBugReportConfirmation()
    {
        BugReportConfirmationPanel.SetActive(false);
    }

    // --- �α׾ƿ� ��� ---
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
        Debug.Log("�α׾ƿ� �Ϸ�. �α��� ȭ������ �̵��մϴ�.");
        PlayerPrefs.DeleteKey("UserToken"); // ���� �ʱ�ȭ
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoginScene");
    }
    public void DeleteAccount()
    {
        Debug.Log("ȸ��Ż�� ����. URL ���� �� ���� ����.");
        //Application.OpenURL("https://yourwebsite.com/delete-account"); // ������Ʈ ����
        Application.Quit(); // ���� ����
    }
}