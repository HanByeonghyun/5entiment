using UnityEngine;

public class ArrowProjectTile : MonoBehaviour
{
    private BossAgent4_1 bossAgent4;   // ���� ������Ʈ�� �����Ͽ� ������ �� �� �ֵ��� ����
    private float lifetime = 2.0f; // ȭ���� ���� �ʾ��� �� ������ �� �ð� (2�� ��)

    //public GameObject boss4;

    private void Start()
    {
        // BossAgent4�� �����մϴ�.
        bossAgent4 = FindFirstObjectByType<BossAgent4_1>();

        // lifetime �Ŀ� ȭ���� ���� ���� ������ �����ϰ� ������ �ִ� �޼��� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    //ȭ���� �������� �¾��� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        //ȭ���� ����1���� �¾��� ��
        if (other.gameObject.tag == "Boss1")
        {
            //ȭ�� ����
            Destroy(this.gameObject);
        }
        //ȭ���� ����2���� �¾��� ��
        if (other.gameObject.tag == "Boss2")
        {
            //ȭ�� ����
            Destroy(this.gameObject);
        }
        //ȭ���� ����3���� �¾��� ��
        if (other.gameObject.tag == "Boss3")
        {
            //ȭ�� ����
            Destroy(this.gameObject);
        }
        //ȭ���� ����4���� �¾��� ��
        if (other.gameObject.tag == "Boss4")
        {
            //ȭ�� ����
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        //ȭ���� ���� �ʰ� ���� �ð��� ������ ��(����4���� ����)
        if (bossAgent4 != null)
        {   // ȭ���� ������ �� ���� + �������� ������ �� �߰� ����
            bossAgent4.SetReward(10.0f + (bossAgent4.consecutiveHits * 5.0f)); 
            //�������� ���� Ƚ�� ����
            bossAgent4.GetComponent<BossAgent4_1>().IncreaseConsecutiveHits();
            Debug.Log("Boss moving");
        }
        // ȭ�� ����
        Destroy(this.gameObject);
    }
}
