using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsceneManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager; // CutsceneManager ��ũ��Ʈ ����

    public void Start()
    {
        // Null Ȯ��
        if (cutsceneManager != null)
        {
            Debug.Log("CutsceneManager�� ����� ����Ǿ����ϴ�.");
            cutsceneManager.StartCutscene();
        }
        else
        {
            Debug.LogError("CutsceneManager�� ������� �ʾҽ��ϴ�!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
