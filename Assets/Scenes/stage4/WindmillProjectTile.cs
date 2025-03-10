using UnityEngine;

public class WindmillProjectTile : MonoBehaviour
{
    private float lifetime = 2.0f; // ������� ���� �ʾ��� �� ���ӽð�

    private void Start()
    {
        // lifetime �Ŀ� ����� �����ϴ� �Լ� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            // Wind ������Ʈ Ÿ�� �� ����
            Destroy(this.gameObject);
        }
    }

    private void GiveRewardIfMissed()
    {
        // ����� ����
        Destroy(this.gameObject);
    }
}
