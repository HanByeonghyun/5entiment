using UnityEngine;

public class AtkController : MonoBehaviour
{
    private float lifetime = 0.5f; // �ٶ��� ���� �ʾ��� �� ���ӽð�(0.5��)

    public float damageInterval = 3.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    private void Start()
    {
        // lifetime �Ŀ� ������ ���� �ʾ� �����ϴ� �Լ�
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            GameObject director = GameObject.Find("GameDirector");
            //�÷��̾� ü�� ����
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            // Wind ������Ʈ Ÿ�� �� ����
            Destroy(this.gameObject);

            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
