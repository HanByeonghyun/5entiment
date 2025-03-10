using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectTile4 : MonoBehaviour
{
    //�÷��̾� ��ǥ
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(0.1f, 5.74f),
        new Vector2(3.0f, 4.55f),
        new Vector2(6.0f, 3.36f),
    };
    public int playerHp = 90;
    private int initialPlayerHp = 90; // �ʱ� ü�� ��

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

    public void TakeDamage()
    {
        playerHp -= 10;
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            // �÷��̾� ��� ó�� ���� �߰� ����
            bossAgent.SetReward(20.0f);
            bossAgent.EndEpisodeWithRewards();
        }
    }
}