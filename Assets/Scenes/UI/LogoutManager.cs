using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject logoutPanel; // �α׾ƿ� Ȯ�� â

    public void OpenLogoutPanel()
    {
        // �α׾ƿ� Ȯ�� â ǥ��
        if (logoutPanel != null)
        {
            logoutPanel.SetActive(true);
        }
    }

    public void CloseLogoutPanel()
    {
        // �α׾ƿ� Ȯ�� â �ݱ�
        if (logoutPanel != null)
        {
            logoutPanel.SetActive(false);
        }
    }

    public void LogoutAndReturnToLogin()
    {
        //// �α׾ƿ� ó�� (��: ��ū ����)
        //PlayerPrefs.DeleteKey("UserToken"); // ��: ���� ��ū ����
        //PlayerPrefs.Save();

        // �α��� ������ �̵�
        SceneManager.LoadScene("Login");
    }
}
