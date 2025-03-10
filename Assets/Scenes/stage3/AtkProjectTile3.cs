using UnityEngine;

//�ּ��� ����3�� �н��� �� �߰��ؼ� ����� �ڵ�
public class AtkProjectTile3 : MonoBehaviour
{
    //private BossAgent3 bossAgent;   // ���� ������Ʈ�� �����Ͽ� ������ �� �� �ֵ��� ����
    private float lifetime = 2.0f; // �ٶ��� ���� �ʾ��� �� �г�Ƽ�� �� �ð� (2�� ��)

    public float damageInterval = 3.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    private void Start()
    {
        // BossAgent�� ã�ų� �����մϴ�. �� �ڵ�� ���� �ִ� BossAgent ������Ʈ�� �����մϴ�.
        //bossAgent = FindFirstObjectByType<BossAgent3>();

        // lifetime �Ŀ� ȭ���� ���� ���� ������ �����ϰ� ������ �ִ� �޼��� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);

        //nextDamgeTime = Time.time + damageInterval; //�ʱ� ���� ������ �ð� ����
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            // PlayerHealth ������Ʈ�� �����ͼ� ü���� ���ҽ�Ŵ
            //PlayerProjectTile3 playerHealth = other.GetComponent<PlayerProjectTile3>();

            //Debug.Log("player Hit!!");
            //playerHealth.TakeDamage();  // �÷��̾�� 10�� �������� ��
            //fire�� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����

            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            // Atk ������Ʈ Ÿ�� �� ����
            Destroy(this.gameObject);

            // ���� ����
            //if (bossAgent != null)
            //{
            //    bossAgent.AddReward(15.0f + (5.0f * bossAgent.consecutiveHits));  // �⺻ ���� + ���� ��Ʈ ����
            //    bossAgent.IncreaseConsecutiveHits();  // ���� ��Ʈ �� ����
            //}
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
