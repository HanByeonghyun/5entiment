using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectTile2 : MonoBehaviour
{
    //�÷��̾� ��ǥ
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
    private int initialPlayerHp = 60; // �ʱ� ü�� ��

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

    // �÷��̾ ������ ��ġ�� �̵���Ű�� ü���� �ʱ�ȭ�ϴ� �Լ�
    public void ResetPlayer()
    {
        SetRandomPosition(); // ������ ��ġ�� �̵�
        playerHp = initialPlayerHp; // ü���� �ʱⰪ���� ����
    }

    // �÷��̾ ������ ��ġ�� �̵���Ű�� �Լ�
    public void SetRandomPosition()
    {
        int randomIndex = Random.Range(0, spawnPositions.Length);  // �迭���� ������ �ε��� ����
        Vector2 randomPosition = spawnPositions[randomIndex];      // ������ ��ǥ ����
        transform.position = randomPosition;                       // �÷��̾� ��ġ ����
    }

    // �ٰŸ� ������ ������ ��
    public void TakeDamage()
    {   
        playerHp -= 10; //�÷��̾� ü�� ����
        if (bossAgent != null)
        {   // ���� ���� ���� + ���� ���ݽ� �߰� ����
            bossAgent.AddReward(10.0f + (5.0f * bossAgent.consecutiveHits));
            bossAgent.IncreaseConsecutiveHits();  // ���� ���� Ƚ�� ����
        }
        // �÷��̾� ����� ū ������ ��
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            bossAgent.SetReward(20.0f);         //ū ����
            bossAgent.EndEpisodeWithRewards();  //���Ǽҵ� ����
        }
    }
}