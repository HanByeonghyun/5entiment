using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenWebPage : MonoBehaviour
{
    // �̵��� URL �ּ�
    public string url = "http://localhost:3000/5entiment/signup"; // ȸ������ ����Ʈ URL
    public Button signUpButton; // ȸ������ ��ư ����

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnSignUpButtonClick()
    {
        Application.OpenURL(url); // URL�� �̵�
        DeselectButton(); // ��ư ���� ����
    }
    private void DeselectButton()
    {
        // ���� ���õ� ��ü�� null�� �����Ͽ� ������ ����
        EventSystem.current.SetSelectedGameObject(null);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
