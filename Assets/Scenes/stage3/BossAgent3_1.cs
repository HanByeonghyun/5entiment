using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using System.Collections;

public class BossAgent3_1 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    // ������ X�� �̵� ����
    public float leftMax = -8.0f; // X�� �ּҰ�
    public float rightMax = 18.0f;  // X�� �ִ밪

    public GameObject player;           // �÷��̾�
    public GameObject arrowPrefab;      // ȭ�� ������
    public float arrowSpeed = 10.0f;    // ȭ�� �ӵ�
    public Transform arrowSpawnPoint;   // ȭ�� ���� ��ġs

    public float moveSpeed = 7.0f;      // ���� �ӵ�
    private Vector3 bossStartPosition;   // ���� ���� ��ġ

    //������ ���� �÷���
    bool isDead = false;
    bool isAttacking = false;

    public float bossCurrentPositionX;
    public float playerCurrentPositionY;
    public float playerCurrentPositionX;

    public GameObject atkPrefab;       // ���� ������
    public Transform atkSpawnPoint;    // ���� ���� ��ġ
    public bool canAttack1 = true;       // ���� ���� ����
    public float attackCooldown1 = 3.0f;   // ���� ��Ÿ��

    public GameObject swamp;

    public GameObject swampPrefab;       // ������ ������
    public GameObject warningPrefab;
    public float pointX;                 // ������ ���� ��ġ
    public float pointY;

    public bool canAttack2 = false;       // ���� ���� ����
    public bool nowAttack = false;       // ���� ���� ����
    public float attackCooldown2 = 3.0f;   // ���� ��Ÿ��

    public int consecutiveHits = 0;  // ���� ��Ʈ ���� �����ϴ� ����

    public int playerHp = 90;
    public int bossHp = 100;
    private int initialPlayerHp;        // �ʱ� �÷��̾� ü��
    private int initialBossHp;          // �ʱ� ���� ü��

    private int stepCounter = 0;             // ���Ǽҵ� �� ���� ī��Ʈ
    private const int maxStepsPerEpisode = 5000;  //5000; // �ִ� ���� ��

    private float episodeTimer = 0f;         // ���Ǽҵ� Ÿ�̸�
    private const float maxEpisodeTime = 30f; // �ִ� ���Ǽҵ� �ð� (30��)

    private Coroutine arrowCoroutine; // ȭ�� �ڷ�ƾ�� �����ϱ� ���� ����

    private PlayerProjectTile3[] playerProjectTiles; // PlayerProjectTile ��ũ��Ʈ ����

    public float distanceToPlayer;

    //�ʱ� ����
    public override void Initialize()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        //bossStartPosition = transform.position;
        player = GameObject.Find("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;

        // ���� ��� PlayerProjectTile ������Ʈ�� ������ �迭�� ����
        playerProjectTiles = Object.FindObjectsByType<PlayerProjectTile3>(FindObjectsSortMode.None);
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
        // ������ ���� ���� �ڷ�ƾ�� ������ ����
        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }

        //bossSpawnPositions �迭���� ������ ��ġ ����
        //int randomIndex = Random.Range(0, bossSpawnPositions.Length);
        //Vector3 randomPosition = new Vector3(bossSpawnPositions[randomIndex].x, bossSpawnPositions[randomIndex].y, 0); // Z ��ǥ�� �ʿ信 ���� ����
        //transform.position = randomPosition; // ������ ��ġ�� �������� ���õ� ��ġ�� ����
        //bossCurrentPositionX = transform.position.x;

        bossStartPosition = transform.position;
        transform.position = bossStartPosition;
        bossCurrentPositionX = transform.position.x;

        playerCurrentPositionX = player.transform.position.x;
        playerCurrentPositionY = player.transform.position.y;

        //�� �÷��̾��� ��ġ�� �������� ����
        foreach (var playerProjectTile3 in playerProjectTiles)
        {
            playerProjectTile3.ResetPlayer();
        }

        ResetConsecutiveHits();

        playerHp = initialPlayerHp;
        bossHp = initialBossHp;

        stepCounter = 0;            // ���� �ʱ�ȭ
        episodeTimer = 0f;          // Ÿ�̸� �ʱ�ȭ
        consecutiveHits = 0;

        arrowCoroutine = StartCoroutine(Arrow());
    }

    public void Update()
    {
        if (!isAttacking) { return; }

        playerCurrentPositionX = player.transform.position.x;
        playerCurrentPositionY = player.transform.position.y;


        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (player.transform.position.x < transform.position.x)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        episodeTimer += Time.deltaTime;

        // �ִ� ���� �Ǵ� �ִ� �ð��� �����ϸ� ���Ǽҵ� ����
        if (stepCounter >= maxStepsPerEpisode || episodeTimer >= maxEpisodeTime)
        {
            EndEpisodeWithRewards();
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
            //this.animator.SetBool("RunBool", true);
            bossCurrentPositionX += moveX * moveSpeed * Time.deltaTime;
            bossCurrentPositionX = Mathf.Clamp(bossCurrentPositionX, leftMax, rightMax);
            transform.position = new Vector3(bossCurrentPositionX, transform.position.y, 0);
        }
        else
        {
            //this.animator.SetBool("RunBool", false);
        }

        float attackAction = actions.ContinuousActions[1];
        if (attackAction > 0.5f && canAttack1)
        {
            // ���� ������ �÷��̾��� �Ÿ��� ���
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= 2.8f && canAttack1)
            {
                BossAtk1();
            }
            else if (distanceToPlayer > 2.8f && canAttack2)
            {
                BossAtk2();
            }

        }

        // ���� ���� Ÿ�̸� ����
        stepCounter++;
    }

    //��ǥ���� ����� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            //this.animator.SetTrigger("HitTrigger");
        }
    }

    public void IncreaseConsecutiveHits()
    {
        consecutiveHits++;  // ���� ��Ʈ �� ����
    }
    public void ResetConsecutiveHits()
    {
        consecutiveHits = 0;  // ���� ��Ʈ �� �ʱ�ȭ
    }

    private IEnumerator Arrow()
    {
        while (true)
        {
            //ȭ�� ��ġ ����
            Vector3 arrowPosition = arrowSpawnPoint.position;
            //������ ��ġ�� ȭ�� ����
            GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowSpawnPoint.rotation);

            //�÷��̾ ���� ����
            float arrowDirection = player.transform.localScale.x;
            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //ȭ�쿡 ������ ���� �߰��� �߻�
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * arrowSpeed, 0);
            }

            yield return new WaitForSeconds(3.0f);
        }
    }

    //���� ����
    public void BossAtk1()
    {
        isAttacking = true;
        canAttack1 = false; //���� ���� ������ �� ������ ����
        //this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(AttackCooldown1()); //��ٿ� �ڷ�ƾ ����
        StartCoroutine(AttackWithDelay1(0.2f));
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator AttackCooldown1()
    {
        yield return new WaitForSeconds(attackCooldown1);
        canAttack1 = true; //3���� ���� ����
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
        //this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(SwampWithWarning(1.0f)); //1�� ���� ��� �� swamp ����
        canAttack2 = false;
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
        pointX = playerCurrentPositionX;
        pointY = playerCurrentPositionY;
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
        bossStartPosition = transform.position;
        StopCoroutine("Arrow"); // ���� ȭ�� �ڷ�ƾ ����
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
