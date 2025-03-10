using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    float playerPosX;
    float playerPosY;
    GameObject player;

    public bool isDead = false;
    public bool nowHit = false;
    public bool nowAttack = false;

    //������ ���̴��� Ȯ��
    bool visible = true;
    //������ ������ �ʴ� �ð�
    float invisibleDuration = 1.0f;

    // �����հ� �߻� ��ġ
    public GameObject eBallPrefab;
    public GameObject windmillPrefab;
    public GameObject windmillSpawnPoint;
    public GameObject warningPrefab;
    //windmill �ӵ�
    public float windmillSpeed = 7.0f;

    //�������� ����� ��ġ ����
    public float pointX;
    public float pointY;

    //��� ��Ŀ�� ������ ����
    private GameObject warningMarker;

    public PlayerController playerController;

    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-5.7f, 4.2f),
        new Vector2(-2.8f, 5.9f),
        new Vector2(11.1f, 7.5f),
        new Vector2(14.4f, 4.2f)
    };

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
        if (isDead) return;

        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

        //�÷��̾ �������� ���ʿ� ������ ����, �����ʿ� ������ �������� �ٶ�
        if(visible)
        {
            if (playerPosX < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (playerPosX > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (isDead) break;
            yield return new WaitForSeconds(3.0f); // 3�ʸ��� ����

            if (player == null) continue;

            nowAttack = true;
            BossHide();
            yield return new WaitForSeconds(2.0f); // 1�ʸ��� ����

            nowAttack = true;
            BossAtk2();
            yield return new WaitForSeconds(0.5f); // 1�ʸ��� ����

            nowAttack = true;
            BossAtk3();
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
            GameObject director4 = GameObject.Find("GameDirector");
            director4.GetComponent<GameDirector>().DecreaseBoss4Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director4 = GameObject.Find("GameDirector");
            director4.GetComponent<GameDirector>().DecreaseBoss4Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //ȭ���� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.1f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }

    //���� ����
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
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
        this.animator.SetTrigger("HideTrigger");
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
        Debug.Log("Selected spawn index: " + randomIndex + ", Position: " + spawnPositions[randomIndex]);
        transform.position = spawnPositions[randomIndex];

        this.animator.SetTrigger("AppearTrigger");
        visible = true;
        GetComponent<SpriteRenderer>().enabled = true; //���� ���̱�
        GetComponent<Collider2D>().enabled = true; //�浹 Ȱ��ȭ
        rigid2D.isKinematic = false; //���� ȿ�� Ȱ��ȭ

        nowAttack = false;
    }


    //���� ����2
    public void BossAtk2()
    {
        pointX = playerPosX;
        pointY = playerPosY;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(EBallWithWarning(1.0f)); //1�� ���� ��� �� EBball ����
    }
    //�������� ���� �Լ�
    //�������� ����
    void EBall() //EBball 
    {
        if (eBallPrefab != null)
        {
            //EBball ����
            GameObject eBall = Instantiate(eBallPrefab);

            eBall.transform.position = new Vector3(pointX, pointY, 0);
            nowAttack = false;
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
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(playerPosX, playerPosY, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1�� �Ŀ� ��� ��Ŀ ����
    }

    //���� ����3
    public void BossAtk3()
    {
        pointX = playerPosX;
        pointY = playerPosY;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(WindmillWithDelay(0.5f));
    }
    //���� ��� windmill �߻�
    IEnumerator WindmillWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Windmill();
    }
    void Windmill() //windmill
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
            nowAttack = false;
        }
        else
        {
            Debug.LogError("windmill prefab or spawn point is not set");
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
