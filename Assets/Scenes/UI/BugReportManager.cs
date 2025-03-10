using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BugReportManager : MonoBehaviour
{
    [Header("Bug Report UI Panels")]
    public GameObject bugReportPanel;     // 버그 리포트 창
    public GameObject reportSendPanel;   // 전송 완료 창

    [Header("Bug Report Components")]
    public InputField bugReportInputField;  // 리포트 입력 필드
    public Button sendButton;               // 보내기 버튼
    public Button cancelButton;             // 취소 버튼
    public Button confirmButton;            // 확인 버튼

    void Start()
    {
        // 초기화: 모든 패널 비활성화
        if (bugReportPanel != null) bugReportPanel.SetActive(false);
        if (reportSendPanel != null) reportSendPanel.SetActive(false);

        // 버튼 이벤트 연결
        if (sendButton != null) sendButton.onClick.AddListener(SendBugReport);
        if (cancelButton != null) cancelButton.onClick.AddListener(CloseBugReportPanel);
        if (confirmButton != null) confirmButton.onClick.AddListener(CloseReportSendPanel);
    }

    // 버그 제보 창 열기
    public void OpenBugReportPanel()
    {
        if (bugReportPanel != null)
        {
            bugReportPanel.SetActive(true);
        }
    }

    // 버그 리포트 보내기
    private void SendBugReport()
    {
        if (bugReportPanel != null) bugReportPanel.SetActive(false); // 리포트 창 닫기
        if (reportSendPanel != null) reportSendPanel.SetActive(true); // 전송 완료 창 열기

        string reportContent = bugReportInputField.text;

        if (!string.IsNullOrEmpty(reportContent))
        {
            // URL 전송 로직은 현재 생략됨
            Debug.Log("버그 리포트 내용: " + reportContent);
        }
        else
        {
            Debug.LogWarning("버그 리포트 내용이 비어있습니다!");
        }

        // 입력 필드 초기화
        bugReportInputField.text = "";
    }
    public void CloseBugReportPanel()
    {
        if (bugReportPanel != null)
        {
            bugReportPanel.SetActive(false);
        }
        // 필요 시 메인 메뉴 복귀 로직 추가
    }
    // 전송 완료 창 닫기 및 메인 메뉴로 복귀
    private void CloseReportSendPanel()
    {
        if (reportSendPanel != null)
        {
            reportSendPanel.SetActive(false);
        }

        Debug.Log("메인 메뉴로 복귀");
        // 필요 시 추가 로직 (예: 메인 메뉴 상태 복원)
    }
}