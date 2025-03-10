using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monsController : MonoBehaviour
{
    public float speed = 2f; // �̵� �ӵ�
    public float minX = -3f; // Y�� �ּҰ�
    public float maxX = 3f; // Y�� �ִ밪

    private bool movingRight = true; // ���� ��ȯ
    private Animator animator;
    private Rigidbody2D rb;

    public float damageInterval = 1.5f; //���� ��Ÿ�� ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    private Image monsHP; // ���� ü�¹� ����

    void Start()
    {
        this.animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        nextDamgeTime = Time.time + damageInterval; //�ʱ� ���� ������ �ð� ����

        // ���� ������ �� �ڽ� ĵ������ ü�¹� Image ������Ʈ ã��
        monsHP = transform.Find("Canvas/monsHP").GetComponent<Image>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        //�װų� �������̰ų� �´����� �� ������ ����
        if (isDead) return;
        if (nowAttack) return;
        if (nowHit) return;
        // ���� X ��ġ�� ������ ������ ����� �ʵ��� ��
        if (transform.position.x < minX)
        {
            movingRight = true; // ���������� ��ȯ
        }
        else if (transform.position.x > maxX)
        {
            movingRight = false; // �������� ��ȯ
        }

        // ���⿡ ���� �̵� �ӵ� �� �̵� �ִϸ��̼� ����
        float direction = movingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        animator.SetBool("WalkBool", true); // �̵� �ִϸ��̼� Ȱ��ȭ

        // ���⿡ ���� ������ ����
        transform.localScale = new Vector3(direction, 1, 1);
    }

    // ������Ʈ�� ����� ��
    void OnTriggerStay2D(Collider2D other)
    {
        //�÷��̾�� ��� ��Ÿ���� ������ ��
        if (other.gameObject.tag == "player" && Time.time >= nextDamgeTime)
        {
            nowAttack = true;
            rb.velocity = Vector2.zero; // ���� �� �̵� ����
            this.animator.SetTrigger("AtkTrigger");

            //GameDirector�� �÷��̾� ü�� ���� �Լ� ȣ��
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            nextDamgeTime = Time.time + damageInterval;
            StartCoroutine(AtkWithDelay(1.0f));
        }
    }
    IEnumerator AtkWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowAttack = false;
    }

    //���𰡿� ����� ��
    void OnTriggerEnter2D(Collider2D other)
    {
        //�÷��̾��� ���ݰ� ��ų�� �¾��� ��
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //���� ü�°��� �Լ� ȣ��
            DecreaseMonsHp(0.25f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(0.25f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(0.5f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(1.0f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //��ȭ����3�� ������ �������� �Լ� ȣ��
            StartCoroutine(SlowTime(speed));
            DecreaseMonsHp(0.5f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }

    //�ӵ� �������� �Լ�
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f); //2�ʵ� �ӵ� ����ȭ
        speed = originalSpeed;
    }

    //ü�°��� �Լ�
    public void DecreaseMonsHp(float damage)
    {
        if (monsHP != null)
        {
            monsHP.fillAmount -= damage;
            // ������ HP�� 0�� ���� ��
            if (monsHP.fillAmount <= 0)
            {
                MonsDeath();
            }
        }
    }

    //���� ��� �Լ�
    public void MonsDeath()
    {
        if (!isDead)
        {
            isDead = true;
            rb.velocity = Vector2.zero; // �̵��� ����
            GetComponent<Collider2D>().enabled = false; //�浹 ��Ȱ��ȭ
            rb.isKinematic = true; //���� ȿ�� ��Ȱ��ȭ
            this.animator.SetTrigger("DeathTrigger");
            StartCoroutine(MonsDeathWithDelay(1.5f));
        }
    }//���Ͱ� �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator MonsDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //������Ʈ ����
        Destroy(this.gameObject);
    }
}
