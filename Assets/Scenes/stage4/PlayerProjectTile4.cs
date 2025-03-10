using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectTile4 : MonoBehaviour
{
    //플레이어 좌표
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(0.1f, 5.74f),
        new Vector2(3.0f, 4.55f),
        new Vector2(6.0f, 3.36f),
    };
    public int playerHp = 90;
    private int initialPlayerHp = 90; // 초기 체력 값

    private BossAgent4 bossAgent;

    // Start is called before the first frame update
    void Start()
    {
        bossAgent = FindFirstObjectByType<BossAgent4>();
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

    public void TakeDamage()
    {
        playerHp -= 10;
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            // 플레이어 사망 처리 로직 추가 가능
            bossAgent.SetReward(20.0f);
            bossAgent.EndEpisodeWithRewards();
        }
    }
}