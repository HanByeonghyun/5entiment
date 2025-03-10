using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private float lifetime = 1.0f; // ȭ���� ���� �ʾ��� �� ���� �ð�(1��)

    void Start()
    {
        // lifetime �Ŀ� ȭ���� ���� ���� ������ �����ϰ� �����ϴ� �Լ�
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
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
            // ȭ�� ����
            Destroy(this.gameObject);
        }
        //ȭ���� ���Ϳ��� �¾��� ��
        if (other.gameObject.tag == "monster")
        {
            // ȭ�� ����
            Destroy(this.gameObject);
        }
        //ȭ���� ���� �¾��� ��
        if (other.gameObject.tag == "Ground2")
        {
            Destroy(this.gameObject);
        }
    }
    private void GiveRewardIfMissed()
    {
        // ȭ�� ����
        Destroy(this.gameObject);
    }
}
