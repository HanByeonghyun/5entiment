using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//�н� �����ϴ� �ڵ�
public class BossAgent : Agent
{
    Animator animator;

    public GameObject player;           // �÷��̾�
    //public GameObject arrowPrefab;      // ȭ�� ������
    //public float arrowSpeed = 10.0f;    // ȭ�� �ӵ�
    //public Transform arrowSpawnPoint;   // ȭ�� ���� ��ġ

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
    public bool nowAttack = false;
    public bool nowHit = false;

    //public int consecutiveHits = 0;  // ���� ��Ʈ ���� �����ϴ� ����

    public int playerHp = 80;           // ���� �÷��̾� ü��
    public int bossHp = 50;             // ���� ���� ü��
    private int initialPlayerHp;        // �ʱ� �÷��̾� ü��
    private int initialBossHp;          // �ʱ� ���� ü��


    //�ʱ� ����
    public override void Initialize()
    {
        bossStartPosition = transform.position;
        player = GameObject.FindWithTag("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;
    }

    //���Ǽҵ� ���۽� �ʱ�ȭ
    public override void OnEpisodeBegin()
    {
        this.animator = GetComponent<Animator>();

        transform.position = bossStartPosition;
        bossCurrentPositionY = transform.position.y;

        //�÷��̾�� ������ ü�� �ʱ�ȭ
        playerHp = initialPlayerHp;
        bossHp = initialBossHp;
    }

    public void Update()
    {
        if (isDead) return;
        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
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
    }

    //��ǥ��(�÷��̾��� ����)�� ����� ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            this.animator.SetTrigger("HitTrigger");
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            this.animator.SetTrigger("HitTrigger");
            //��ȭ����3�� �¾��� �� �ӵ� ����
            StartCoroutine(SlowTime(speed));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.1f);
        }
    }
    //�ӵ� ���� �Լ�
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f);
        speed = originalSpeed;
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

    //���� ����
    public void BossDeath()
    {
        if (!isDead)
        {
            isDead = true;
            // ������ �ٸ� ������ ������� �ʵ��� ���� �ʱ�ȭ
            canAttack = false;

            this.animator.SetTrigger("DeathTrigger");
            //DeathTrigger �߻� �� 1.5�� �� ���� �����
            StartCoroutine(BossDeathWithDelay(2.5f));
        }
    }
    //������ �״� �ִϸ��̼� �߻� �� 1.5�� �� ������� �����̸� �ֱ� ����
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    //���Ǽҵ� ����
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
