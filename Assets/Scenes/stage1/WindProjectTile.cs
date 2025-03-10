using UnityEngine;

public class WindProjectTile : MonoBehaviour
{
    private BossAgent1 bossAgent1;   // ���� ������Ʈ�� �����Ͽ� ������ �� �� �ֵ��� ����
    private float lifetime = 2.0f; // �ٶ��� ���� �ð�

    private void Start()
    {
        // BossAgent�� ã�ų� �����մϴ�. �� �ڵ�� ���� �ִ� BossAgent ������Ʈ�� �����մϴ�.
        bossAgent1 = FindFirstObjectByType<BossAgent1>();

        // lifetime �Ŀ� �ٶ� ����
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player") // �÷��̾�� ������ ������ ��
        {
            PlayerProjectTile playerHealth = other.GetComponent<PlayerProjectTile>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();  // �÷��̾�� 10�� �������� ��
            }
            // ���� ����
            if (bossAgent1 != null)
            {   // ���� + �������� ���� �ÿ� ������ �� ��
                bossAgent1.AddReward(15.0f + (5.0f * bossAgent1.consecutiveHits));
                bossAgent1.IncreaseConsecutiveHits();  // �������� ���� Ƚ�� ����
            }

            // Wind ������Ʈ Ÿ�� �� ����
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        // �ٶ��� �浹 ���� lifetime�� ����� ���
        // �ٶ� ����
        Destroy(this.gameObject);
    }
}
