using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    //���� ������ �ּ�, �ִ� ����
    float downMax = -2.55f;
    float upMax = 3.5f;
    //������ x,y ��ǥ
    float currentPositionY;
    float currentPositionX;
    //�÷��̾� x,y ��ǥ
    float playerPosX;
    float playerPosY;
    //���� ���Ʒ� �̵� �ӵ�
    public float speed = 4.0f;
    float currentDirection = 1.0f; // ���Ʒ� ������ ���� (1�� ��, -1�� �Ʒ�)

    GameObject player;
    //������ ���� ���� ����
    bool canAttack = true;
    //���� ��Ÿ��
    float attackCooldown = 3f;

    //�ٶ� �����հ� �߻� ��ġ
    public GameObject windPrefab;
    public Transform windSpawnPoint;
    public float windSpeed = 7.0f;

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    public PlayerController playerController;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        currentPositionY = transform.position.y;

        player = GameObject.FindWithTag("player");

        playerController = GameObject.FindWithTag("player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isDead) return;
        if (nowHit) return;
        if (player == null) return;

        // �÷��̾� ��ġ ������Ʈ
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (playerPosX < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerPosX > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // ���� �ڵ� ���Ʒ� �̵�
        float newPositionY = transform.position.y + (speed * currentDirection * Time.deltaTime);
        newPositionY = Mathf.Clamp(newPositionY, downMax, upMax);

        // ���� ����
        if (newPositionY <= downMax || newPositionY >= upMax)
        {
            currentDirection *= -1;
        }

        transform.position = new Vector3(transform.position.x, newPositionY, 0);

        // �÷��̾��� y��ǥ�� ������ y��ǥ ������ ������ ����
        if (Mathf.Abs(playerPosY - transform.position.y) < 1.0f && canAttack)
        {
            nowAttack = true;
            BossAtk();
        }
    }

    //���� ���� ����
    //ȭ�� ������Ʈ(arrow)�� �浹�� HitTrigger �߻�
    void OnTriggerEnter2D(Collider2D other)
    {
        if (nowAttack) return;
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            StartCoroutine(SlowTime(speed));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.1f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f);
        speed = originalSpeed;
    }

    //���� ����
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
        //DeathTrigger �߻� �� 1.5�� �� ���� �����
        StartCoroutine(BossDeathWithDelay(1.5f));
    }
    //������ �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //���� ����
    public void BossAtk()
    {
        this.animator.SetTrigger("AtkTrigger");
        canAttack = false; //���� ���� ������ �� ������ ����
        StartCoroutine(AttackCooldown()); //��ٿ� �ڷ�ƾ ����
        StartCoroutine(WindWithDelay(0.2f));
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; //3���� ���� ����
    }
    //���� ��� �� �ٶ� �߻�
    IEnumerator WindWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Wind();
        nowAttack = false;
    }
    void Wind() //�ٶ� �߻� �Լ�
    {
        if (windPrefab != null && windSpawnPoint != null)
        {
            //������ ��ġ�� �ٶ� ����
            GameObject wind1 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);
            GameObject wind2 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);

            //������ ���� ����
            float windDirection = transform.localScale.x;
            wind1.transform.localScale = new Vector3(windDirection, 1, 1);
            wind2.transform.localScale = new Vector3(windDirection * (-1), 1, 1);

            //�ٶ��� ������ ���� �߰��� �߻�
            Rigidbody2D windRb1 = wind1.GetComponent<Rigidbody2D>();
            Rigidbody2D windRb2 = wind2.GetComponent<Rigidbody2D>();
            if (windRb1 != null && windRb2 != null)
            {
                windRb1.velocity = new Vector2(windDirection * windSpeed, 0);
                windRb2.velocity = new Vector2(windDirection * windSpeed * (-1), 0);
            }
        }
        else
        {
            Debug.LogError("wind prefab or spawn point is not set");
        }
    }

    //�÷��̾� ���� �� �ν��� �÷��̾� ������Ʈ ����
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
