using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsceneManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager; // CutsceneManager 스크립트 참조

    public void Start()
    {
        // Null 확인
        if (cutsceneManager != null)
        {
            Debug.Log("CutsceneManager가 제대로 연결되었습니다.");
            cutsceneManager.StartCutscene();
        }
        else
        {
            Debug.LogError("CutsceneManager가 연결되지 않았습니다!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
