using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial0Manager : MonoBehaviour
{

    public GameObject vase; // 용기_0
    public GameObject AttackText; // Attack 텍스트
    public GameObject Attack;     // Attack 이미지
    public GameObject SummonText;
    public SpriteRenderer portalSpriteRenderer; // 포탈의 SpriteRenderer
    public GameObject portalText; // 포탈 텍스트
    public WispSceneManager wispSceneManager; // WispSceneManager 컴포넌트 참조

    private bool isActivated = false; // 공격 UI 활성화 여부
    private bool isPortalTextActivated = false; // 포탈 텍스트 활성화 여부

    public void Start()
    {
        // 초기화: 텍스트와 이미지를 비활성화
        AttackText.SetActive(false);
        SummonText.SetActive(false);
        Attack.SetActive(false);
        portalText.SetActive(false);

        // 포탈의 SpriteRenderer가 존재할 경우 초기 비활성화 상태 확인
        if (portalSpriteRenderer != null)
        {
            portalSpriteRenderer.enabled = false; // 포탈 초기 비활성화
        }
    }
    public void Update()
    {
        if (vase == null && !isActivated)
        {
            ActivateAttackUI(); // UI 활성화 메서드 호출
            isActivated = true; // 한 번만 실행되도록 설정

            // WispSceneManager의 StartCutscene 호출
            if (wispSceneManager != null)
            {
                wispSceneManager.StartCutscene(); // 컷씬 시작
            }
        }
        if (portalSpriteRenderer != null && portalSpriteRenderer.enabled && !isPortalTextActivated)
        {
            ActivatePortalText(); // 포탈 텍스트 활성화
            isPortalTextActivated = true; // 한 번만 실행되도록 설정
        }
    }
    // UI 활성화 메서드
    private void ActivateAttackUI()
    {
        AttackText.SetActive(true); // "공격 : ctrl" 텍스트 활성화
        SummonText.SetActive(true); // 추가 설명 텍스트 활성화
        Attack.SetActive(true); // "CTRL" 키 이미지 활성화
    }
    // 포탈 텍스트 활성화 메서드
    private void ActivatePortalText()
    {
        portalText.SetActive(true); // "포탈 관련 텍스트" 활성화
    }
}
