using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//���� �н��� �ڵ�
public class BossAgent4_1 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    public GameObject player;           // �÷��̾�

    private Vector3 bossStartPosition;   // ���� ���� ��ġ

    public bool visible = true;          // ���� ���̴���
    float invisibleDuration = 1.0f;      // ���� �Ⱥ��̴� �ð�

    public bool canHide = true;
    public float canHideCooldown = 3.0f;

    public GameObject warningPrefab;

    public GameObject windmillPrefab;       // ����� ������
    public float windmillSpeed = 5.0f;      // ����� �ӵ�
    public Transform windmillSpawnPoint;    // ����� ���� ��ġ

    public GameObject eBallPrefab;       // �������� ������

    //�������� ����� ��ġ ����
    public float pointX;
    public float pointY;

    public bool isDead = false;

    public int bossHp = 40;
    private int initialBossHp;          // �ʱ� ���� ü��

    public int consecutiveHits = 0;

    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-5.7f, 4.2f),
        new Vector2(-2.8f, 5.9f),
        new Vector2(11.1f, 7.5f),
        new Vector2(14.4f, 4.2f)
    };


    //�ʱ� ����
    public override void Initialize()
    {
        player = GameObject.Find("player");
        initialBossHp = bossHp;
        canHide = true;
    }

    //���Ǽҵ� ���۽� �ʱ�ȭ
    public override void OnEpisodeBegin()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        consecutiveHits = 0;

        bossHp = initialBossHp;
        canHide = true;
    }

    public void Update()
    {
        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (visible)
        {
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    //������Ʈ�� ȯ�濡�� ������ ���� �����͸� ����
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(bossHp / (float)initialBossHp);
    }

    public void IncreaseConsecutiveHits()
    {
        consecutiveHits++;  // ���� ��Ʈ �� ����
    }
    public void ResetConsecutiveHits()
    {
        consecutiveHits = 0;  // ���� ��Ʈ �� �ʱ�ȭ
    }

    //������Ʈ�� �ൿ�� ���� �� ȣ��
    public override void OnActionReceived(ActionBuffers actions)
    {
        float hideAction = actions.ContinuousActions[0];

        // �����̵� �ൿ
        if (hideAction > 0.5f && canHide)
        {
            //nowAttack = true;
            canHide = false;
            BossHide(); // Hide �޼��带 ȣ���Ͽ� �����̵��� ����
        }
    }

    //��ǥ���� ����� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            AddReward(-3.0f);
        }
    }

    //���� ����
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
        //BossDefeated(); // BossDefeated �޼��带 ȣ���Ͽ� ������ ������ ó��
        //DeathTrigger �߻� �� 2.5�� �� ���� �����
        StartCoroutine(BossDeathWithDelay(2.0f));
    }
    //������ �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //���� �����
    public void BossHide()
    {
        //this.animator.SetTrigger("HideTrigger");
        StartCoroutine(GoUnderground());
    }
    IEnumerator GoUnderground()
    {
        yield return new WaitForSeconds(0.3f); //�ִϸ��̼� �ð����� ����

        visible = false;
        GetComponent<SpriteRenderer>().enabled = false; //���� �����
        GetComponent<Collider2D>().enabled = false; //�浹 ��Ȱ��ȭ
        rigid2D.isKinematic = true; //���� ȿ�� ��Ȱ��ȭ

        yield return new WaitForSeconds(invisibleDuration); //����� �ð�

        // ������ ��ġ ����
        int randomIndex = Random.Range(0, spawnPositions.Length);
        transform.position = spawnPositions[randomIndex];

        BossAppear(); //���� ����
    }

    //���� ��Ÿ��
    public void BossAppear()
    {
        int randomIndex = Random.Range(0, spawnPositions.Length);
        //Debug.Log("Selected spawn index: " + randomIndex + ", Position: " + spawnPositions[randomIndex]);
        transform.position = spawnPositions[randomIndex];

        //this.animator.SetTrigger("AppearTrigger");
        visible = true;
        GetComponent<SpriteRenderer>().enabled = true; //���� ���̱�
        GetComponent<Collider2D>().enabled = true; //�浹 Ȱ��ȭ
        rigid2D.isKinematic = false; //���� ȿ�� Ȱ��ȭ

        //nowAttack = false;

        pointX = player.transform.position.x;
        pointY = player.transform.position.y;
        //nowAttack = true;
        BossAtk1();
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator HideCooldown()
    {
        yield return new WaitForSeconds(canHideCooldown);
        canHide = true; //3���� ���� ����
    }


    //���� ����
    public void BossAtk1()
    {
        //canAttack1 = false; //���� ���� ������ �� ������ ����
        //StartCoroutine(AttackCooldown1()); //��ٿ� �ڷ�ƾ ����
        StartCoroutine(windMillWithDelay(0.2f));
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    //IEnumerator AttackCooldown1()
    //{
    //    yield return new WaitForSeconds(attackCooldown1);
    //    //canAttack1 = true; //3���� ���� ����
    //}
    //���� ��� �� �ٶ� �߻�
    IEnumerator windMillWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        windMill();
    }
    void windMill() //�ٶ� 
    {
        if (windmillPrefab != null && windmillSpawnPoint != null)
        {

            Vector3 windmillPosition = windmillSpawnPoint.transform.position - new Vector3(0, 0.5f, 0);
            //windmill ����
            GameObject windmill = Instantiate(windmillPrefab, windmillPosition, windmillSpawnPoint.transform.rotation);

            // �÷��̾��� ��ġ�� �߻��� ������ ���
            Vector3 playerPosition = new Vector3(pointX, pointY, 0);
            Vector3 directionToPlayer = (playerPosition - windmillPosition).normalized; // �÷��̾� �������� ���� ���� ���

            // windmill�� ������ ���� �߰��� �÷��̾� �������� �߻�
            Rigidbody2D windmillRb = windmill.GetComponent<Rigidbody2D>();
            if (windmillRb != null)
            {
                windmillRb.velocity = directionToPlayer * windmillSpeed;
            }
            //nowAttack = false;

            pointX = player.transform.position.x;
            pointY = player.transform.position.y;
            //nowAttack = true;
            BossAtk2();

        }
        else
        {
            Debug.LogError("windmill prefab or spawn point is not set");
        }
    }

    //���� ����2
    public void BossAtk2()
    {
        //this.animator.SetTrigger("Atk2Trigger");
        //canAttack2 = false; //���� ���� ������ �� ������ ����
        //StartCoroutine(AttackCooldown2()); //��ٿ� �ڷ�ƾ ����
        StartCoroutine(EBallWithWarning(1.0f)); //1�� ���� ��� �� EBball ����
    }
    //3�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    //IEnumerator AttackCooldown2()
    //{
    //    yield return new WaitForSeconds(attackCooldown2);
    //    //canAttack2 = true; //3���� ���� ����
    //}
    //�������� ���� �Լ�
    //�������� ����
    void EBall() //EBball 
    {
        if (eBallPrefab != null)
        {
            //EBball ����
            GameObject eBall = Instantiate(eBallPrefab);

            eBall.transform.position = new Vector3(pointX, pointY, 0);
            //nowAttack = false;
            StartCoroutine(HideCooldown()); //��ٿ� �ڷ�ƾ ����
        }
        else
        {
            Debug.LogError("eBall prefab is not set");
        }
    }
    IEnumerator EBallWithWarning(float warningDuration)
    {
        ShowWarning(); // ��� ��ȣ �Լ�
        yield return new WaitForSeconds(warningDuration); // ���� �ð����� ���

        EBall(); // ���� EBball �Լ� ȣ��
    }

    void ShowWarning()
    {
        // ���� ��� ��� ��Ŀ�� �����ϰų� �ش� ��ġ�� ���� ��ȭ��Ű�� ���� ���� ����
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(pointX, pointY, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1�� �Ŀ� ��� ��Ŀ ����
    }

    public void EndEpisodeWithRewards()
    {
        // ���� ü���� 50% �̻��� ��� �߰� ����
        if (bossHp / (float)initialBossHp >= 0.5f)
        {
            AddReward(10f);  // �߰� ����
        }
        // ���� ü���� �������� �г�Ƽ�� Ŀ������ ����
        AddReward((1 - (bossHp / (float)initialBossHp)) * (-5f));

        bossStartPosition = transform.position;
        // ���Ǽҵ� ����
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // ����ΰų� �⺻���� ������ ��� �޽��� ����
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // �����̵��� ���� ����
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
