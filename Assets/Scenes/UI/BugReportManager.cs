using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BugReportManager : MonoBehaviour
{
    [Header("Bug Report UI Panels")]
    public GameObject bugReportPanel;     // ���� ����Ʈ â
    public GameObject reportSendPanel;   // ���� �Ϸ� â

    [Header("Bug Report Components")]
    public InputField bugReportInputField;  // ����Ʈ �Է� �ʵ�
    public Button sendButton;               // ������ ��ư
    public Button cancelButton;             // ��� ��ư
    public Button confirmButton;            // Ȯ�� ��ư

    void Start()
    {
        // �ʱ�ȭ: ��� �г� ��Ȱ��ȭ
        if (bugReportPanel != null) bugReportPanel.SetActive(false);
        if (reportSendPanel != null) reportSendPanel.SetActive(false);

        // ��ư �̺�Ʈ ����
        if (sendButton != null) sendButton.onClick.AddListener(SendBugReport);
        if (cancelButton != null) cancelButton.onClick.AddListener(CloseBugReportPanel);
        if (confirmButton != null) confirmButton.onClick.AddListener(CloseReportSendPanel);
    }

    // ���� ���� â ����
    public void OpenBugReportPanel()
    {
        if (bugReportPanel != null)
        {
            bugReportPanel.SetActive(true);
        }
    }

    // ���� ����Ʈ ������
    private void SendBugReport()
    {
        if (bugReportPanel != null) bugReportPanel.SetActive(false); // ����Ʈ â �ݱ�
        if (reportSendPanel != null) reportSendPanel.SetActive(true); // ���� �Ϸ� â ����

        string reportContent = bugReportInputField.text;

        if (!string.IsNullOrEmpty(reportContent))
        {
            // URL ���� ������ ���� ������
            Debug.Log("���� ����Ʈ ����: " + reportContent);
        }
        else
        {
            Debug.LogWarning("���� ����Ʈ ������ ����ֽ��ϴ�!");
        }

        // �Է� �ʵ� �ʱ�ȭ
        bugReportInputField.text = "";
    }
    public void CloseBugReportPanel()
    {
        if (bugReportPanel != null)
        {
            bugReportPanel.SetActive(false);
        }
        // �ʿ� �� ���� �޴� ���� ���� �߰�
    }
    // ���� �Ϸ� â �ݱ� �� ���� �޴��� ����
    private void CloseReportSendPanel()
    {
        if (reportSendPanel != null)
        {
            reportSendPanel.SetActive(false);
        }

        Debug.Log("���� �޴��� ����");
        // �ʿ� �� �߰� ���� (��: ���� �޴� ���� ����)
    }
}