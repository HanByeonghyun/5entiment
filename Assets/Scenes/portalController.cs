using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class portalController : MonoBehaviour
{
    private Renderer portalRenderer; // 포탈의 렌더러 컴포넌트
    private bool playerInPortal = false; // 플레이어가 포탈에 있는지 여부
    private float timeInPortal = 0f; // 포탈에 머문 시간
    public float portalStayTime = 2f; // 포탈에 머물러야 하는 시간
    public string nextSceneName; // 이동할 씬 이름

    GameObject playerHP;

    public bool visible = false; //포탈 활성화 여부

    void Start()
    {
        // 포탈의 Renderer 컴포넌트를 가져와 비활성화
        portalRenderer = GetComponent<Renderer>();
        portalRenderer.enabled = false; // 처음에는 포탈이 보이지 않도록 설정
        this.playerHP = GameObject.Find("Hpbar");
    }

    void Update()
    {
        
        GameObject[] sentiment0 = GameObject.FindGameObjectsWithTag("sentiment0");
        GameObject[] sentiment1 = GameObject.FindGameObjectsWithTag("sentiment1");
        GameObject[] sentiment2 = GameObject.FindGameObjectsWithTag("sentiment2");
        GameObject[] sentiment3 = GameObject.FindGameObjectsWithTag("sentiment3");
        GameObject[] sentiment4 = GameObject.FindGameObjectsWithTag("sentiment4");
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        //몬스터나 보스가 모두 사라졌는지 확인
        // 몬스터가 모두 사라진 경우 포탈을 보이게 함
        if (sentiment0.Length == 0 && sentiment1.Length == 0 && sentiment2.Length == 0 && sentiment3.Length == 0 
            && sentiment4.Length == 0 && monsters.Length == 0 && boss1.Length == 0 && boss2.Length == 0
            && boss3.Length == 0 && boss4.Length == 0)
        {
            portalRenderer.enabled = true; // 포탈을 보이게 설정
            visible = true; //포탈 활성화 여부
        }

        // 플레이어가 포탈에 있고 포탈이 활성화 됐을 경우 타이머 시작
        if (playerInPortal && visible)
        {
            timeInPortal += Time.deltaTime;

            // 3초 동안 포탈 위에 있을 경우 다음 씬으로 이동
            if (timeInPortal >= portalStayTime)
            {
                LoadNextScene();
            }
        }
        else
        {
            timeInPortal = 0f; // 포탈을 떠나면 타이머 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 포탈에 플레이어가 들어오면 타이머 시작
        if (other.CompareTag("player"))
        {
            Debug.Log("Player entered the portal."); // 디버그 로그 추가
            playerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 포탈에서 플레이어가 나가면 타이머 초기화
        if (other.CompareTag("player"))
        {
            Debug.Log("Player exited the portal."); // 디버그 로그 추가
            playerInPortal = false;
            timeInPortal = 0f;
        }
    }

    //다음 스테이지로 넘어가는 함수
    private void LoadNextScene()
    {
        // 지정된 이름의 씬으로 이동
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            //플레이어 체력 초기화
            this.playerHP.GetComponent<Image>().fillAmount = 1.0f;
            Debug.Log("Loading next scene: " + nextSceneName); // 디버그 로그 추가
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("다음 씬 이름이 설정되지 않았습니다.");
        }
    }
}
