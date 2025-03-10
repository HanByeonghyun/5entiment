using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//�н� ��Ű�� �ڵ�
public class BossAgent1 : Agent
{
    Animator animator;

    public GameObject player;           // �÷��̾�
    public GameObject arrowPrefab;      // ȭ�� ������
    public float arrowSpeed = 10.0f;    // ȭ�� �ӵ�
    public Transform arrowSpawnPoint;   // ȭ�� ���� ��ġ

    public float speed = 4.0f;      // ���� �ӵ�
    private Vector3 bossStartPosition;   // ���� ���� ��ġ
    public float downMax = -2.55f;
    public float upMax = 3.5f;

    public float bossCurrentPositionY;
    public float playerCurrentPositionY;
    public GameObject windPrefab;       // �ٶ� ������
    public float windSpeed = 5.0f;      // �ٶ� �ӵ�
    public Transform windSpawnPoint;    // �ٶ� ���� ��ġ
    public bool canAttack = true;       // ���� ���� ����
    public float attackCooldown = 3.0f;   // ���� ��Ÿ��

    public bool isDead = false;

    public int consecutiveHits = 0;  // ���� ��Ʈ ���� �����ϴ� ����

    public int playerHp = 80;
    public int bossHp = 40;
    private int initialPlayerHp;        // �ʱ� �÷��̾� ü��
    private int initialBossHp;          // �ʱ� ���� ü��

    private int stepCounter = 0;             // ���Ǽҵ� �� ���� ī��Ʈ
    private const int maxStepsPerEpisode = 5000;  //5000; // �ִ� ���� ��

    private float episodeTimer = 0f;         // ���Ǽҵ� Ÿ�̸�
    private const float maxEpisodeTime = 30f; // �ִ� ���Ǽҵ� �ð� (30��)

    private Coroutine arrowCoroutine; // ȭ�� �ڷ�ƾ�� �����ϱ� ���� ����

    private PlayerProjectTile[] playerProjectTiles; // PlayerProjectTile ��ũ��Ʈ ����

    private PlayerProjectTile playerHealth;

    public float distanceToPlayer;
    public float distanceTime = 3.0f;
    public float nowTime;



    //�ʱ� ����
    public override void Initialize()
    {
        bossStartPosition = transform.position;
        player = GameObject.FindWithTag("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;

        // ���� ��� PlayerProjectTile ������Ʈ�� ������ �迭�� ����
        playerProjectTiles = Object.FindObjectsByType<PlayerProjectTile>(FindObjectsSortMode.None);

        // �迭 �� �ϳ��� playerHealth�� ������ �� �ֽ��ϴ�. (��: ù ��° ���)
        if (playerProjectTiles.Length > 0)
        {
            playerHealth = playerProjectTiles[0]; // ù ��° PlayerProjectTile�� ����
        }

        playerHealth = player.GetComponent<PlayerProjectTile>();
    }

    //���Ǽҵ� ���۽� �ʱ�ȭ
    public override void OnEpisodeBegin()
    {
        this.animator = GetComponent<Animator>();

        // ������ ���� ���� �ڷ�ƾ�� ������ ����
        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }

        transform.position = bossStartPosition;
        bossCurrentPositionY = transform.position.y;

        //�� �÷��̾��� ��ġ�� �������� ����
        foreach (var playerProjectTile in playerProjectTiles)
        {
            playerProjectTile.ResetPlayer();
        }


        playerCurrentPositionY = player.transform.position.y;

        playerHp = initialPlayerHp;
        bossHp = initialBossHp;

        stepCounter = 0;            // ���� �ʱ�ȭ
        episodeTimer = 0f;          // Ÿ�̸� �ʱ�ȭ
        consecutiveHits = 0;

        arrowCoroutine = StartCoroutine(Arrow());
    }

    public void Update()
    {
        if (isDead) return;

        nowTime = Time.time + distanceTime;
        // �÷��̾�� ������ y�� �Ÿ�
        distanceToPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);
        if (Time.time >= nowTime) // 3�ʸ���
        {
            if (distanceToPlayer < 2.0f) //�Ÿ��� 2���� ����� ��
            {
                AddReward(5.0f); //����5��
            }
            else if (distanceToPlayer < 4.0f) //�Ÿ��� 4���� ����� ��
            {
                AddReward(1.0f); //����1��
            }
            else //�Ÿ��� 4���� �� ��
            {
                AddReward(-3.0f); //�г�Ƽ -3��
            }
        }

        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (player.transform.position.x < transform.position.x)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //������Ʈ�� ȯ�濡�� ������ ���� �����͸� ����
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(playerHp / (float)initialPlayerHp);   //�÷��̾� ü�� ����
        sensor.AddObservation(transform.position);                  //���� ��ġ
        sensor.AddObservation(player.transform.position);           //�÷��̾� ��ġ
        sensor.AddObservation(bossHp / (float)initialBossHp);       //������ ü�� ����
    }

    //������Ʈ�� �ൿ�� ���� �� ȣ��
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isDead) { return; }
        //������ ���� ������ ù ��° �ൿ(���� ������)
        float moveY = actions.ContinuousActions[0];
        //������ ���� ������
        bossCurrentPositionY += moveY * speed * Time.deltaTime;
        bossCurrentPositionY = Mathf.Clamp(bossCurrentPositionY, downMax, upMax);
        transform.position = new Vector3(transform.position.x, bossCurrentPositionY, 0);

        //������ ���� ������ �� ��° �ൿ(����)
        float attackAction = actions.ContinuousActions[1];
        if (attackAction > 0.5f && canAttack)
        {
            BossAtk();
        }

        // ���� ���� Ÿ�̸� ����
        stepCounter++;
        episodeTimer += Time.deltaTime;

        // �ִ� ���� �Ǵ� �ִ� �ð��� �����ϸ� ���Ǽҵ� ����
        if (stepCounter >= maxStepsPerEpisode || episodeTimer >= maxEpisodeTime)
        {
            EndEpisodeWithRewards();
        }
    }

    //��ǥ���� ����� ��
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "arrow")
    //    {
    //        this.animator.SetTrigger("HitTrigger");
    //    }
    //}

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
        while(true)
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
    public void BossAtk()
    {
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
    }
    void Wind() //�ٶ� 
    {
        //�ٶ� ����
        GameObject wind1 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);
        GameObject wind2 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);

        //�÷��̾ ���� ����
        float windDirection = transform.localScale.x;
        wind1.transform.localScale = new Vector3(windDirection, 1, 1);
        wind2.transform.localScale = new Vector3(windDirection * (-1), 1, 1);

        //�ٶ��� ������ ���� �߰��� �߻�
        Rigidbody2D windRb1 = wind1.GetComponent<Rigidbody2D>();
        Rigidbody2D windRb2 = wind2.GetComponent<Rigidbody2D>();

        windRb1.velocity = new Vector2(windDirection * windSpeed, 0);
        windRb2.velocity = new Vector2(windDirection * windSpeed * (-1), 0);
    }

    public void EndEpisodeWithRewards()
    {
        // �÷��̾� ü���� 50% ������ ��� �߰� ����
        if (playerHealth.playerHp / (float)initialPlayerHp <= 0.5f)
        {
            AddReward(10f);  // �߰� ����
        }
        // �÷��̾� ü���� �������� �г�Ƽ�� Ŀ������ ����
        AddReward(-(playerHealth.playerHp / (float)initialPlayerHp) * 5f);

        StopCoroutine("Arrow"); // ȭ�� ����
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // ����ΰų� �⺻���� ������ ��� �޽��� ����
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // �������� 0���� ����
        continuousActions[1] = 0f; // ������ ���� ����
    }
}
