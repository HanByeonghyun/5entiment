using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenWebPage : MonoBehaviour
{
    // 이동할 URL 주소
    public string url = "http://localhost:3000/5entiment/signup"; // 회원가입 사이트 URL
    public Button signUpButton; // 회원가입 버튼 참조

    // 버튼 클릭 시 호출되는 함수
    public void OnSignUpButtonClick()
    {
        Application.OpenURL(url); // URL로 이동
        DeselectButton(); // 버튼 선택 해제
    }
    private void DeselectButton()
    {
        // 현재 선택된 객체를 null로 설정하여 선택을 해제
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
