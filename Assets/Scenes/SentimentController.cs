using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 
public class SentimentController : MonoBehaviour
{
    private Renderer portalRenderer; // 정령의 렌더러 컴포넌트
    private Collider2D portalCollider;
    public bool visible = false;     // 정령이 보이는지 여부

    void Start()
    {
        // 포탈의 Renderer, Collider2D 컴포넌트를 가져와 비활성화
        portalRenderer = GetComponent<Renderer>();
        portalCollider = GetComponent<Collider2D>();

        portalRenderer.enabled = false; // 처음에는 정령이 보이지 않도록 설정
        if (portalCollider != null)
        {
            portalCollider.enabled = false; // 처음에는 Collider(부딪히게 하는 효과)도 비활성화
        }
    }

    void Update()
    {
        //몬스터나 보스가 모두 사라졌는지 확인
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        //몬스터와 보스가 없을 시
        if (monsters.Length == 0 && boss1.Length == 0 && boss2.Length == 0
            && boss3.Length == 0 && boss4.Length == 0)
        {
            portalRenderer.enabled = true; // 정령을 보이게 설정
            visible = true;
            if (portalCollider != null)
            {
                portalCollider.enabled = true; // Collider 활성화
            }
        } else
        {
            visible = false;
            if (portalCollider != null)
            {
                portalCollider.enabled = false; // Collider 비활성화
            }
        }
    }
}
