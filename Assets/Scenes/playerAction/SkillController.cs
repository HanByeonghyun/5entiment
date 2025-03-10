using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private float lifetime = 1.0f; // ȭ���� ���� �ʾ��� �� ���ӽð�(1��)
    Animator animator;
    Rigidbody2D rigid2D;

    void Start()
    {
        // lifetime �Ŀ� ȭ���� ���� ���� ������ �����ϰ� �����ϴ� �Լ� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);
        this.animator = GetComponent<Animator>();
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //ȭ���� ����1���� �¾��� ��
        if (other.gameObject.tag == "Boss1")
        {
            //�´� ȿ��
            StartCoroutine(HitDelay());
        }

        //ȭ���� ����2���� �¾��� ��
        if (other.gameObject.tag == "Boss2")
        {
            //�´� ȿ��
            StartCoroutine(HitDelay());
        }

        //ȭ���� ����3���� �¾��� ��
        if (other.gameObject.tag == "Boss3")
        {
            //�´� ȿ��
            StartCoroutine(HitDelay());
        }

        //ȭ���� ����4���� �¾��� ��
        if (other.gameObject.tag == "Boss4")
        {
            //�´� ȿ��
            StartCoroutine(HitDelay());
        }
        //ȭ���� ���Ϳ��� �¾��� ��
        if (other.gameObject.tag == "monster")
        {
            //�´� ȿ��
            StartCoroutine(HitDelay());
        }
        if (other.gameObject.tag == "Ground2" || other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator HitDelay()
    {
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        this.animator.SetTrigger("HitTrigger");
        yield return new WaitForSeconds(0.5f);
        // ȭ�� ����
        Destroy(this.gameObject);
    }
    private void GiveRewardIfMissed()
    {
        // ȭ�� ����
        Destroy(this.gameObject);
    }
}
