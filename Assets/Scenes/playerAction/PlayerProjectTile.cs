using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스 학습 때 사용한 플레이어 스크립트
public class PlayerProjectTile : MonoBehaviour
{
    //에피소드가 시작할 때마다 플레이어가 무작위로 생성될 좌표
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-0.65f, 5.74f),
        new Vector2(-1.56f, 4.55f),
        new Vector2(-0.61f, 3.36f),
        new Vector2(-0.49f, 2.15f),
        new Vector2(-1.28f, 0.94f),
        new Vector2(-0.48f, -0.59f)
    };
    public int playerHp = 80;         // 플레이어 현재 체력
    private int initialPlayerHp = 80; // 플레이어 초기 체력 값

    private BossAgent1 bossAgent1;

    // Start is called before the first frame update
    void Start()
    {
        bossAgent1 = FindFirstObjectByType<BossAgent1>();
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

    public void TakeDamage() // 보스1이 플레이어에게 공격을 맞췄을 때
    {
        playerHp -= 10; //체력 감소
        // 공격을 맞추면 보상 + 연속으로 맞출시 추가 보상
        bossAgent1.AddReward(10.0f + (bossAgent1.consecutiveHits * 5.0f));
        // 연속으로 맞춘 횟수 증가
        bossAgent1.IncreaseConsecutiveHits();

        // 플레이어를 계속 맞춰서 죽였을 시 큰 보상
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            bossAgent1.AddReward(20.0f);            //큰 보상
            bossAgent1.EndEpisodeWithRewards();     //에피소드 종료
        }
    }
}