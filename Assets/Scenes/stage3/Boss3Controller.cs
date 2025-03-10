using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    float playerPosX;
    float playerPosY;
    GameObject player;

    //���� ��Ÿ��
    float attackCooldown = 3f;

    //������ �����հ� �߻� ��ġ
    public GameObject swampPrefab;
    public GameObject warningPrefab;

    //������ ����� ��ġ ����
    public float pointX;
    public float pointY;

    //��� ��Ŀ�� ������ ����
    private GameObject warningMarker;

    public GameObject atkPrefab;       // ���� ������
    public Transform atkSpawnPoint;    // ���� ���� ��ġ

    // ���� �̵� ���� ����
    public float speed = 5.0f;
    private float moveDirection = 1.0f; // 1�� ������, -1�� ����

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    public PlayerController playerController;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("player");

        playerController = FindAnyObjectByType<PlayerController>();

        // �ֱ����� ������ ���� �ڷ�ƾ ����
        StartCoroutine(AutoAttack());
    }

    void Update()
    {
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

        if (nowHit) return;
        if (nowAttack) return;
        if (isDead) return;
        if (player == null) return;

        // �̵��ϴ� ���⿡ ���� �ٶ󺸴� ���� ����
        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // �������� �ٶ�
        }
        else if (moveDirection < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // ������ �ٶ�
        }

        // ������ �¿� �̵�
        transform.position += new Vector3(speed * moveDirection * Time.deltaTime, 0, 0);

        // ���� �������� ������ ����
        if (transform.position.x > 17.0f || transform.position.x < -9.0f) // x ��ǥ ���� ����
        {
            moveDirection *= -1.0f; // ���� ����
        }
    }

    IEnumerator AutoAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown); // 3�ʸ��� ����

            if (player == null) continue;

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // �÷��̾���� �Ÿ��� ���� �ٸ� ���� ���
            if (distanceToPlayer > 3.5f) // �� �Ÿ����� ����1
            {

                nowAttack = true;
                BossAtk3();
            }
            else if (distanceToPlayer <= 3.5f) // ����� �Ÿ����� ����2
            {
                nowAttack = true;
                BossAtk2();
            }
        }
    }

    //���� ���� ����
    //ȭ�� ������Ʈ(arrow)�� �浹�� HitTrigger �߻�
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (nowAttack) return;
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director3 = GameObject.Find("GameDirector");
            director3.GetComponent<GameDirector>().DecreaseBoss3Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director3 = GameObject.Find("GameDirector");
            director3.GetComponent<GameDirector>().DecreaseBoss3Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            StartCoroutine(SlowTime(speed));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.1f);
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
        //DeathTrigger �߻� �� 2.5�� �� ���� �����
        StartCoroutine(BossDeathWithDelay(2.5f));
    }
    //������ �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //���� ����2
    public void BossAtk2()
    {
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(AttackWithDelay(0.8f));
    }
    IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
        nowAttack = false;
    }
    void Attack() //����
    {
        // ������ ���⿡ ���� x�� ������ ����
        float xOffset = transform.localScale.x < 0 ? 1.83f : -1.83f; // ������ �������� �ٶ� ���� ����������, ������ �ٶ� ���� �������� ������ ����
        float yOffset = -0.8f; // y�� �������� �׻� ����

        // ������ ��ġ�� ������ ����
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);

        //���� ����
        GameObject atk1 = Instantiate(atkPrefab, spawnPosition, atkSpawnPoint.rotation);

        //�÷��̾ ���� ����
        float atkDirection = transform.localScale.x;
        atk1.transform.localScale = new Vector3(atkDirection, 1, 1);

        //���� ����
        Rigidbody2D atkRb1 = atk1.GetComponent<Rigidbody2D>();

        atkRb1.velocity = new Vector2(0, 0);
    }



    //���� ����3
    public void BossAtk3()
    {
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(SwampWithWarning(1.0f)); //1�� ���� ��� �� swamp ����
    }
    //������ ���� �Լ�
    //������ ����
    void Swamp() //swamp 
    {
        if (swampPrefab != null)
        {
            //swamp ����
            GameObject newSwamp = Instantiate(swampPrefab);
            newSwamp.transform.position = new Vector3(pointX, (-2.0f), 0);
            nowAttack = false;
        }
        else
        {
            Debug.LogError("swamp prefab is not set");
        }
    }
    IEnumerator SwampWithWarning(float warningDuration)
    {
        pointX = playerPosX;
        pointY = playerPosY;
        ShowWarning(); // ��� ��ȣ �Լ�
        yield return new WaitForSeconds(warningDuration); // ���� �ð����� ���

        Swamp(); // ���� Swamp �Լ� ȣ��
    }

    void ShowWarning()
    {
        // ���� ��� ��� ��Ŀ�� �����ϰų� �ش� ��ġ�� ���� ��ȭ��Ű�� ���� ���� ����
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(playerPosX, -2, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1�� �Ŀ� ��� ��Ŀ ����
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
