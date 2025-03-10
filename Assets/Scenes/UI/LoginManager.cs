using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField idInputField;           // ���̵� �Է� �ʵ�
    public InputField passwordInputField;     // ��й�ȣ �Է� �ʵ�
    public GameObject successPanel;           // �α��� ���� �г�
    public GameObject failurePanel;           // �α��� ���� �г�
    public string mainMenuSceneName = "Main Menu"; // ���� �޴� �� �̸�

    // ���� ȸ������ (�׽�Ʈ��)
    private string correctId = "testUser";     // ���� ��Ȳ������ ������ �����ͺ��̽��� ����Ͽ� Ȯ��
    private string correctPassword = "1234";

    private string correctId2 = "gksqudgus";     // ���� ��Ȳ������ ������ �����ͺ��̽��� ����Ͽ� Ȯ��
    private string correctPassword2 = "gksqudgus";

    // �α��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnLoginButtonClick()
    {
        string enteredId = idInputField.text;
        string enteredPassword = passwordInputField.text;

        if ((enteredId == correctId && enteredPassword == correctPassword) || (enteredId == correctId2 && enteredPassword == correctPassword2))
        {
            // �α��� ����
            ShowSuccessPanel();
        }
        else
        {
            // �α��� ����
            ShowFailurePanel();
        }
    }

    // �α��� ���� �г� ǥ��
    private void ShowSuccessPanel()
    {
        successPanel.SetActive(true);  // �α��� ���� �г� Ȱ��ȭ
    }

    // �α��� ���� �г� ǥ��
    private void ShowFailurePanel()
    {
        failurePanel.SetActive(true);  // �α��� ���� �г� Ȱ��ȭ
    }

    // ���� �гο��� Ȯ�� ��ư Ŭ�� �� ȣ��
    public void OnSuccessConfirmButtonClick()
    {
        successPanel.SetActive(false);          // ���� �г� ��Ȱ��ȭ
        SceneManager.LoadScene(mainMenuSceneName); // ���� �޴� ������ �̵�
    }

    // ���� �гο��� Ȯ�� ��ư Ŭ�� �� ȣ��
    public void OnFailureConfirmButtonClick()
    {
        failurePanel.SetActive(false);          // ���� �г� ��Ȱ��ȭ
        // �Է� �ʵ� �ʱ�ȭ
        idInputField.text = "";
        passwordInputField.text = "";
    }
}
