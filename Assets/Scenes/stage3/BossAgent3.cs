using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using System.Collections;

public class BossAgent3 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    // ������ X�� �̵� ����
    public float leftMax = -8.0f; // X�� �ּҰ�
    public float rightMax = 18.0f;  // X�� �ִ밪

    public GameObject player;           // �÷��̾�

    public float speed = 7.0f;      // ���� �ӵ�

    //������ ���� �÷���
    bool isDead = false;
    bool isAttacking = false;

    public float bossCurrentPositionX;

    public GameObject atkPrefab;       // ���� ������
    public Transform atkSpawnPoint;    // ���� ���� ��ġ
    public bool canAttack1 = true;       // ���� ���� ����
    public float attackCooldown1 = 2.0f;   // ���� ��Ÿ��

    public GameObject swamp;

    public GameObject swampPrefab;       // ������ ������
    public GameObject warningPrefab;
    public float pointX;                 // ������ ���� ��ġ
    public float pointY;

    public bool canAttack2 = false;       // ���� ���� ����
    public bool nowAttack = false;       // ���� ���� ����
    public float attackCooldown2 = 3.0f;   // ���� ��Ÿ��

    public int playerHp = 90;
    public int bossHp = 100;
    private int initialPlayerHp;        // �ʱ� �÷��̾� ü��
    private int initialBossHp;          // �ʱ� ���� ü��

    public float distanceToPlayer;

    //�ʱ� ����
    public override void Initialize()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;
    }

    public void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    //���Ǽҵ� ���۽� �ʱ�ȭ
    public override void OnEpisodeBegin()
    {
        playerHp = initialPlayerHp;
        bossHp = initialBossHp;
    }

    public void Update()
    {
        if (!isAttacking) { return; }

        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //������Ʈ�� ȯ�濡�� ������ ���� �����͸� ����
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(playerHp / (float)initialPlayerHp);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(bossHp / (float)initialBossHp);
    }

    //������Ʈ�� �ൿ�� ���� �� ȣ��
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isDead || isAttacking) return;
        float moveX = actions.ContinuousActions[0];
        if (moveX != 0)
        {
            bossCurrentPositionX += moveX * speed * Time.deltaTime;
            bossCurrentPositionX = Mathf.Clamp(bossCurrentPositionX, leftMax, rightMax);
            transform.position = new Vector3(bossCurrentPositionX, transform.position.y, 0);
        }

        float attackAction = actions.ContinuousActions[1];
        if (attackAction > 0.5f && canAttack1)
        {
            // ���� ������ �÷��̾��� �Ÿ��� ���
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= 2.8f && canAttack1)
            {
                canAttack1 = false;
                BossAtk1();
               
            }
            else if (distanceToPlayer > 2.8f && canAttack2)
            {
                canAttack2 = false;
                BossAtk2();
            }

        }
    }

    //��ǥ���� ����� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(SlowTime(speed));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.1f);
        }
    }
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f);
        speed = originalSpeed;
    }

    //���� ����
    public void BossAtk1()
    {
        isAttacking = true;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(AttackCooldown1()); //��ٿ� �ڷ�ƾ ����
        StartCoroutine(AttackWithDelay1(0.2f));
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator AttackCooldown1()
    {
        yield return new WaitForSeconds(attackCooldown1);
        canAttack1 = true; //2���� ���� ����
        isAttacking = false;
    }
    //���� ��� �� ���� ������ ����
    IEnumerator AttackWithDelay1(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
    }
    void Attack() //����
    {
        // ������ ���⿡ ���� x�� ������ ����
        float xOffset = transform.localScale.x < 0 ? 0.5f : -0.5f; // ������ �������� �ٶ� ���� ����������, ������ �ٶ� ���� �������� ������ ����
        float yOffset = -0.2f; // y�� �������� �׻� ����

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
    public void BossAtk2()
    {
        isAttacking = true;
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(SwampWithWarning(1.0f)); //1�� ���� ��� �� swamp ����
    }
    //�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator AttackCooldown2()
    {
        yield return new WaitForSeconds(attackCooldown2);
        canAttack2 = true; //���� ���� ����
        isAttacking = false;
    }

    //������ ���� �Լ�
    //������ ����
    void Swamp() //swamp 
    {
        if (swampPrefab != null)
        {
            GameObject newSwamp = Instantiate(swampPrefab); // �� ������ ����
            newSwamp.transform.position = new Vector3(pointX, (-2.0f), 0);
            canAttack2 = false;
            StartCoroutine(AttackCooldown2()); //��ٿ� �ڷ�ƾ ����
        }
        else
        {
            Debug.LogError("swamp prefab is not set");
        }
    }
    IEnumerator SwampWithWarning(float warningDuration)
    {
        pointX = player.transform.position.x;
        pointY = player.transform.position.y;
        ShowWarning(); // ��� ��ȣ �Լ�
        yield return new WaitForSeconds(warningDuration); // ���� �ð����� ���

        Swamp(); // ���� Swamp �Լ� ȣ��
    }

    void ShowWarning()
    {
        // ���� ��� ��� ��Ŀ�� �����ϰų� �ش� ��ġ�� ���� ��ȭ��Ű�� ���� ���� ����
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(pointX, -2, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1�� �Ŀ� ��� ��Ŀ ����
    }

    //���� ����
    public void BossDeath()
    {
        if (!isDead)
        {
            isDead = true;
            // ������ �ٸ� ������ ������� �ʵ��� ���� �ʱ�ȭ
            canAttack1 = false;
            canAttack2 = false;
            nowAttack = false;

            this.animator.SetTrigger("DeathTrigger");
            //DeathTrigger �߻� �� 2.5�� �� ���� �����
            StartCoroutine(BossDeathWithDelay(2.5f));
        }
    }
    //������ �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public void EndEpisodeWithRewards()
    {
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // ����ΰų� �⺻���� ������ ��� �޽��� ����
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // �������� 0���� ����
        continuousActions[1] = 0f; // ������ ���� ����
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
