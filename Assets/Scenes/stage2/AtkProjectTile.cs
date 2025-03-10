using UnityEngine;

public class AtkProjectTile : MonoBehaviour
{
    private BossAgent2_1 bossAgent;   // ���� ������Ʈ�� �����Ͽ� ������ �� �� �ֵ��� ����
    private float lifetime = 2.0f; // �ٶ��� ���� �ʾ��� �� �г�Ƽ�� �� �ð� (2�� ��)

    public float damageInterval = 3.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    private void Start()
    {
        // BossAgent�� ã�ų� �����մϴ�. �� �ڵ�� ���� �ִ� BossAgent ������Ʈ�� �����մϴ�.
        bossAgent = FindFirstObjectByType<BossAgent2_1>();

        // lifetime �Ŀ� ������ ���� ���� ������ �����ϰ� �����ϴ� �Լ�
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // �ٰŸ� ������ ������ ��
    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            // PlayerHealth ������Ʈ�� �����ͼ� ü���� ���ҽ�Ŵ
            PlayerProjectTile2 playerHealth = other.GetComponent<PlayerProjectTile2>();
            if (playerHealth != null)
            {
                Debug.Log("player Hit!!");
                playerHealth.TakeDamage();  // �÷��̾�� 10�� �������� ��
                // �ٰŸ� ���� �� ����
                Destroy(this.gameObject);
            }
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        // ������ �浹 ���� lifetime�� ����� ��� ���� ����
        if (bossAgent != null)
        {
            bossAgent.ResetConsecutiveHits();  // ���� ��Ʈ �� �ʱ�ȭ
        }

        // ���� ����
        Destroy(this.gameObject);
    }
}
