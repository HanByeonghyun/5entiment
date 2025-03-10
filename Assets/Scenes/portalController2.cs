using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class portalController2 : MonoBehaviour
{
    private Renderer portalRenderer; // 포탈의 렌더러 컴포넌트
    private bool playerInPortal = false; // 플레이어가 포탈에 있는지 여부
    private float timeInPortal = 0f; // 포탈에 머문 시간
    public float portalStayTime = 2f; // 포탈에 머물러야 하는 시간
    public string nextSceneName; // 이동할 씬 이름

    GameObject playerHP;

    public bool visible = false;

    public PlayerStateManager playerState;

    void Start()
    {
        // 포탈의 Renderer 컴포넌트를 가져와 비활성화
        portalRenderer = GetComponent<Renderer>();
        portalRenderer.enabled = false; // 처음에는 포탈이 보이지 않도록 설정
        this.playerHP = GameObject.Find("Hpbar");
        playerState = FindAnyObjectByType<PlayerStateManager>();
    }

    void Update()
    {
        //플레이어가 스테이지를 완료해 정령과 스킬을 얻었을 시
        if (playerState.canSkill1)
        {
            portalRenderer.enabled = true; // 포탈을 보이게 설정
            visible = true; //포탈 활성화 가능
        }

        // 플레이어가 포탈에 있을 경우 타이머 시작
        if (playerInPortal && visible)
        {
            timeInPortal += Time.deltaTime;

            // 2초 동안 포탈 위에 있을 경우 다음 씬으로 이동
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
