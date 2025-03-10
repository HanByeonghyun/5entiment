using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // 스킬 사용 횟수
    public int skill1Count = 0;
    public int skill2Count = 0;
    public int skill3Count = 0;
    public int rainCount = 0;

    // 스킬 얻었는지 여부
    public bool canSkill0 = false; 
    public bool canSkill1 = false; 
    public bool canSkill2 = false; 
    public bool canSkill3 = false;
    public bool canSkill4 = false;

    public GameObject player0Prefab;

    public GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("player");

        DontDestroyOnLoad(this.gameObject); // 씬 전환 시 유지
    }

    //스킬 쿨타임 시작(쿨타임 끝났을 시 스킬 사용 횟수 0으로 초기화)-------------------------------------------
    public IEnumerator Skill1Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime); //스킬 쿨타임
        skill1Count = 0; // 스킬 사용횟수 초기화
    }
    public IEnumerator Skill2Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        skill2Count = 0; // 스킬 사용횟수 초기화
    }
    public IEnumerator Skill3Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        skill3Count = 0; // 스킬 사용횟수 초기화
    }
    public IEnumerator RainCooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        rainCount = 0; // 스킬 사용횟수 초기화
    } // -----------------------------------------------------------------------------------------

    //스킬 사용시 호출되는 함수--------------------------------------------------------------
    public void Skill1CountUp()
    {
        skill1Count++; //스킬 사용 횟수 증가
        //스킬 사용 횟수 최댓값 도달
        if (skill1Count == 8)
        {
            //변신이 풀려 다시 기본 모드로 돌아감
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            //스킬 쿨타임 시작 함수 호출(스킬 쿨타임)
            StartCoroutine(Skill1Cooltime(10.0f));
        }
    }
    public void Skill2CountUp()
    {
        skill2Count++;
        if (skill2Count == 5)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(Skill2Cooltime(15.0f));
        }
    }
    public void Skill3CountUp()
    {
        skill3Count++;
        if (skill3Count == 8)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(Skill3Cooltime(10.0f));
        }
    }
    public void RainCountUp()
    {
        rainCount++;
        if (rainCount == 2)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(RainCooltime(20.0f));
        }
    }
    //------------------------------------------------------------------------------------------

    //플레이어 변신 후 변신한 프리팹으로 참조 초기화
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}