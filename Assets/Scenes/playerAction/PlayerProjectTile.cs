using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �н� �� ����� �÷��̾� ��ũ��Ʈ
public class PlayerProjectTile : MonoBehaviour
{
    //���Ǽҵ尡 ������ ������ �÷��̾ �������� ������ ��ǥ
    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-0.65f, 5.74f),
        new Vector2(-1.56f, 4.55f),
        new Vector2(-0.61f, 3.36f),
        new Vector2(-0.49f, 2.15f),
        new Vector2(-1.28f, 0.94f),
        new Vector2(-0.48f, -0.59f)
    };
    public int playerHp = 80;         // �÷��̾� ���� ü��
    private int initialPlayerHp = 80; // �÷��̾� �ʱ� ü�� ��

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

    public void TakeDamage() // ����1�� �÷��̾�� ������ ������ ��
    {
        playerHp -= 10; //ü�� ����
        // ������ ���߸� ���� + �������� ����� �߰� ����
        bossAgent1.AddReward(10.0f + (bossAgent1.consecutiveHits * 5.0f));
        // �������� ���� Ƚ�� ����
        bossAgent1.IncreaseConsecutiveHits();

        // �÷��̾ ��� ���缭 �׿��� �� ū ����
        if (playerHp <= 0)
        {
            playerHp = 0;
            Debug.Log("Player is defeated!");
            bossAgent1.AddReward(20.0f);            //ū ����
            bossAgent1.EndEpisodeWithRewards();     //���Ǽҵ� ����
        }
    }
}