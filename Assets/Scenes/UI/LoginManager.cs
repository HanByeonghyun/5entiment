using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField idInputField;           // 아이디 입력 필드
    public InputField passwordInputField;     // 비밀번호 입력 필드
    public GameObject successPanel;           // 로그인 성공 패널
    public GameObject failurePanel;           // 로그인 실패 패널
    public string mainMenuSceneName = "Main Menu"; // 메인 메뉴 씬 이름

    // 옳은 회원정보 (테스트용)
    private string correctId = "testUser";     // 실제 상황에서는 서버나 데이터베이스를 사용하여 확인
    private string correctPassword = "1234";

    private string correctId2 = "gksqudgus";     // 실제 상황에서는 서버나 데이터베이스를 사용하여 확인
    private string correctPassword2 = "gksqudgus";

    // 로그인 버튼 클릭 시 호출되는 함수
    public void OnLoginButtonClick()
    {
        string enteredId = idInputField.text;
        string enteredPassword = passwordInputField.text;

        if ((enteredId == correctId && enteredPassword == correctPassword) || (enteredId == correctId2 && enteredPassword == correctPassword2))
        {
            // 로그인 성공
            ShowSuccessPanel();
        }
        else
        {
            // 로그인 실패
            ShowFailurePanel();
        }
    }

    // 로그인 성공 패널 표시
    private void ShowSuccessPanel()
    {
        successPanel.SetActive(true);  // 로그인 성공 패널 활성화
    }

    // 로그인 실패 패널 표시
    private void ShowFailurePanel()
    {
        failurePanel.SetActive(true);  // 로그인 실패 패널 활성화
    }

    // 성공 패널에서 확인 버튼 클릭 시 호출
    public void OnSuccessConfirmButtonClick()
    {
        successPanel.SetActive(false);          // 성공 패널 비활성화
        SceneManager.LoadScene(mainMenuSceneName); // 메인 메뉴 씬으로 이동
    }

    // 실패 패널에서 확인 버튼 클릭 시 호출
    public void OnFailureConfirmButtonClick()
    {
        failurePanel.SetActive(false);          // 실패 패널 비활성화
        // 입력 필드 초기화
        idInputField.text = "";
        passwordInputField.text = "";
    }
}
