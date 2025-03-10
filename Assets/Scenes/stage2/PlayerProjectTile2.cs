using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectTile2 : MonoBehaviour
{
    //플레이어 좌표
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-0.65f, 5.74f),
        new Vector2(-1.56f, 4.55f),
        new Vector2(-0.61f, 3.36f),
        new Vector2(-0.49f, 2.15f),
        new Vector2(-1.28f, 0.94f),
        new Vector2(-0.48f, -0.59f)
    };
    public int playerHp = 60;
    private int initialPlayerHp = 60; // 초기 체력 값

    private BossAgent2_1 bossAgent;

    // Start is called before the first frame update
    void Start()
    {
        bossAgent = FindFirstObjectByType<BossAgent2_1>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 플레이어를 무작위 위치로 이동시키고 체력을 초기화하는 함수
    public void ResetPlayer()
    {
        SetRandomPosition(); // 무작위 위치로 이동
        playerHp = initialPlayerHp; // 체력을 초기값으로 설정
    }

    // 플레이어를 무작위 위치로 이동시키는 함수
    public void SetRandomPosition()
    {
        int randomIndex = Random.Range(0, spawnPositions.Length);  // 배열에서 무작위 인덱스 선택
        Vector2 randomPosition = spawnPositions[randomIndex];      // 무작위 좌표 선택
        transform.position = randomPosition;                       // 플레이어 위치 설정
    }

    // 근거리 공격을 맞췄을 때
    public void TakeDamage()
    {   
        playerHp -= 10; //플레이어 체력 감소
        if (bossAgent != null)
        {   // 공격 성공 보상 + 연속 공격시 추가 보상
            bossAgent.AddReward(10.0f + (5.0f * bossAgent.consecutiveHits));
            bossAgent.IncreaseConsecutiveHits();  // 연속 공격 횟수 증가
        }
        // 플레이어 사망시 큰 보상을 줌
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            bossAgent.SetReward(20.0f);         //큰 보상
            bossAgent.EndEpisodeWithRewards();  //에피소드 종료
        }
    }
}