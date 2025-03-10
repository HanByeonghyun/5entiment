using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    //�÷��̾� ȿ����
    private PlayerSoundManager soundManager;

    //�̵� �� ����
    float jumpForce = 500.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 3.0f;

    //������ ���� ���ο� ��Ÿ��
    bool canRoll = true;
    float rollCooldown = 1f;

    //�����Ҷ� ������ ����Ʈ ������
    public GameObject change1Prefab;
    public GameObject change2Prefab;
    public GameObject change3Prefab;
    public GameObject change4Prefab;

    //ȭ�� �����հ� �߻� ��ġ, �ӵ�
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10.0f;
    //��ų�� ��ȭ�� ȭ�� �����յ�� �� ��ų�� ��Ÿ��
    public GameObject RainPrefab;
    public GameObject skill1Prefab;
    public float skill1Speed = 10.0f;
    public GameObject skill2Prefab;
    public float skill2Speed = 10.0f;
    public GameObject skill3Prefab;
    public float skill3Speed = 10.0f;

    //�÷��̾��� ���� �÷���(���, ������, �´��� ������ ����)
    bool isDead = false;
    bool nowAttack = false;
    bool nowHit = false;

    //�÷��̾��� �⺻����, ��ų ��� ���� ����
    bool canAttack = true;
    bool canAttack2 = true;
    //���ݺ� ��Ÿ��
    float attackCooldown = 0.7f;
    float skill1Cooldown = 0f;
    float skill2Cooldown = 1.0f;
    float skill3Cooldown = 0.7f;
    // �÷��̾ ���ƿ� ��ġ
    public Vector3 respawnPosition1 = new Vector3(-8.6f, 0.3f, 0);  //stage2_3
    public Vector3 respawnPosition2 = new Vector3(4.5f, 3.55f, 0);  //stage4_Boss
    public Vector3 respawnPosition3 = new Vector3(-8.1f, 12.5f, 0); //stage3_3
    public Vector3 respawnPosition4 = new Vector3(-7.6f, 6.6f, 0);  //stage4_2
    public Vector3 respawnPosition5 = new Vector3(38.0f, 1.5f, 0);  //stage4_3

    //����, 2������ ���� ����
    private bool isGrounded = false;
    private bool canDoubleJump = false;

    // �÷��� Effector2D ����(stage1_Boss�ʿ��� ����ϴ� ���� ����)
    private Collider2D playerCollider;
    private bool canDown = false;

    //������ ������
    public GameObject player0Prefab;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject player3Prefab;
    public GameObject player4Prefab;

    // Ű ����
    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;
    private KeyCode moveDownKey;
    private KeyCode jumpKey;
    private KeyCode attackKey;
    private KeyCode rollKey;

    //�Ͻ������� �� ������ ��ũ��Ʈ ��ü
    public PauseManager pauseManager;

    // ���� �������� ������ ��ũ��Ʈ ��ü�� ������Ʈ
    public GameObject playerStateManager;
    public PlayerStateManager playerState;

    //�÷��̾��� ü�°� ��ų Ƚ�� ������Ʈ
    GameObject playerHP;
    GameObject playerMP;

    //������ �׾��� �� ���� ȹ�� ���� ����
    public bool visible = false; 

    //�÷��̾ ���� ��ġ
    private Vector3 deathPosition; 

    void Start()
    {
        
        Application.targetFrameRate = 60; //���� ������ �ӵ�
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        this.playerHP = GameObject.Find("Hpbar");
        this.playerMP = GameObject.Find("Mpbar");

        this.playerStateManager = GameObject.Find("PlayerStateManager");
        playerState = FindAnyObjectByType<PlayerStateManager>();

        // ���� ���������� �Ѿ�� �÷��̾ �����ǰ� �ϴ� �Լ�
        DontDestroyOnLoad(this.gameObject);

        // Hpbar, Mpbar ������Ʈ�� ã�Ƽ� �����ǵ��� ����
        GameObject playerUI = GameObject.FindWithTag("playerUI");
        if (playerUI != null)
        {
            DontDestroyOnLoad(playerUI); // Hpbar, Mpbar�� �� ��ȯ �� ����
        }

        //�Ͻ����� UI�� ���� �� �α� ����
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
            if (pauseManager != null )
            {
                Debug.Log("PauseManager�� �����ϴ�");
            }
        }
        LoadKeyBindings();

        SettingControl.OnKeyBindingsUpdated += LoadKeyBindings;

        // �� ���� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;

        //�÷��̾� ȿ���� ���� ��ũ��Ʈ
        soundManager = GetComponent<PlayerSoundManager>();
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SettingControl.OnKeyBindingsUpdated -= LoadKeyBindings;
    }

    //���� ��ȯ�� ��(���������� �Ѿ ��)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // MainMenu������ �Ѿ �� �÷��̾� ������Ʈ�� UI ����
        if (scene.name == "Main menu")
        {
            Destroy(this.gameObject); // �÷��̾� ������Ʈ ����
            GameObject playerUI = GameObject.Find("playerUI");
            if (playerUI != null)
            {
                Destroy(playerUI); // �÷��̾� UI ����
            }
            return;
        }

        //MianMenu�� �ƴ� �ٸ� ������ �Ѿ ��
        // appearPrefab�� ��ġ�� �÷��̾� ����
        GameObject appearPrefab = GameObject.Find("appearPrefab");
        if (appearPrefab != null)
        {
            transform.position = appearPrefab.transform.position;
        }
        else
        {
            Debug.LogWarning("appearPrefab�� ã�� �� �����ϴ�.");
        }
    }

    //�׻� ȣ��ǰ� �ִ� �Լ�
    void Update()
    {

        //�Ͻ�����--------------------------------------------------------
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
            if (pauseManager != null)
            {
                Debug.Log("PauseManager�� �����ϴ�");
            }
        }
        //ESC���� �Ͻ����� ���� �� ������ �Ұ� 
        if (pauseManager != null && pauseManager.isPaused) return;
        // -----------------------------------------------------------------

        
        // �÷��̾ �װų� �������̰ų� �´����� �� �����Ӱ� ������ ����
        if (isDead) return;
        if (nowAttack) return;
        if (nowHit) return;

        // �⺻ ����� �� ��ų ��� ���ΰ� true�� �� QWERŰ ������ �ٸ� �÷��̾� ���������� ����
        if (transform.name == "player0Prefab(Clone)" || transform.name == "player0Prefab")
        {
            if (Input.GetKeyDown(KeyCode.Q) && playerState.skill1Count == 0 && playerState.canSkill1)
            {
                // ���� �Լ��� ������ �����հ� �� �� ��Ÿ�� ����Ʈ�� �־ ȣ��
                TransformToPrefab(player1Prefab, change1Prefab);
            }
            if (Input.GetKeyDown(KeyCode.W) && playerState.skill2Count == 0 && playerState.canSkill2)
            {
                TransformToPrefab(player2Prefab, change2Prefab);
            }
            if (Input.GetKeyDown(KeyCode.E) && playerState.skill3Count == 0 && playerState.canSkill3)
            {
                TransformToPrefab(player3Prefab, change3Prefab);
            }
            if (Input.GetKeyDown(KeyCode.R) && playerState.rainCount == 0 && playerState.canSkill4)
            {
                TransformToPrefab(player4Prefab, change4Prefab);
            }
        }

        // ���� ����
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(jumpKey))
        {
            if (canDown)
            { 
                soundManager.PlayJumpSound();
                StartCoroutine(EnableTriggerTemporarily());
               
            }
        }
        else if (Input.GetKeyDown(jumpKey)) //�⺻ ����
        {
            if (isGrounded) //�ٴڿ� �پ��ִٸ�
            {
                soundManager.PlayJumpSound();
                Jump1(); //�⺻ ����
                canDoubleJump = true; // ù ���� �� 2�� ���� ����
            }
            else if (canDoubleJump)
            {
                Jump2(); //2�� ����
                canDoubleJump = false; // 2�� ���� �� �� �̻� ���� �Ұ�
            }
        }

        //�¿� �̵� + ������------------------------------------------------------------------------
        int key = 0;
        if (Input.GetKey(moveRightKey))
        {
            key = 1; //�÷��̾� ����
            this.animator.SetBool("RunBool", true); //�ٴ� �ִϸ��̼� Ȱ��ȭ
            if (Input.GetKeyDown(rollKey) && canRoll) //������
            {
                soundManager.PlayRollSound();
                StartCoroutine(RollCooldown()); // �� ��Ÿ�� �ڷ�ƾ ȣ��
                this.animator.SetTrigger("RollTrigger"); //������ �ִϸ��̼��� ����ϴ� Ʈ���� ȣ��
                //������ ���� �ӵ� ����
                maxWalkSpeed = 7.0f;
            }
        }
        else if (Input.GetKey(moveLeftKey))
        {
            key = -1;
            this.animator.SetBool("RunBool", true);
            if (Input.GetKeyDown(rollKey) && canRoll) //������
            {
                soundManager.PlayRollSound();
                StartCoroutine(RollCooldown()); // �� ��ٿ� �ڷ�ƾ ȣ��
                this.animator.SetTrigger("RollTrigger");
                maxWalkSpeed = 7.0f;
            }
        }
        else
        {
            key = 0;
            this.animator.SetBool("RunBool", false);
        }
        //�÷��̾��� �ӵ�
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        //���ǵ� ����
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        else
        {
            maxWalkSpeed = 3.0f;
        }

        //�����̴� ���⿡ ���� �����Ѵ�.
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        //--------------------------------------------------------------------------------------



        // �⺻ ���� + ��ȭ ---------------------------------------------------------------
        if (Input.GetKeyDown(attackKey) && canAttack && playerState.canSkill0)
        {
            // player4Prefab�� �� ȭ���� ��ġ�� ��¦ �Ʒ��� ����
            float arrowOffsetY = transform.name == "player4Prefab(Clone)" ? -0.2f : 0f;

            GameObject director = GameObject.Find("GameDirector");
            
            //��ȭ�� �⺻ ����
            if (transform.name == "player1Prefab(Clone)" && playerState.skill1Count < 8 && playerState.canSkill1) //��ų1
            {
                //��ȭ�� �⺻ ���� ���� ��� Ƚ�� ����
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.125f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //���� ���� ������ �� ������ ����
                StartCoroutine(Skill1Cooldown()); //��ٿ� �ڷ�ƾ ����
                StartCoroutine(Skill1WithDelay(0.18f, arrowOffsetY));
            }
            else if (transform.name == "player2Prefab(Clone)" && playerState.skill2Count < 5 && playerState.canSkill2) //��ų2
            {
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.2f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //���� ���� ������ �� ������ ����
                StartCoroutine(Skill2Cooldown()); //��ٿ� �ڷ�ƾ ����
                StartCoroutine(Skill2WithDelay(0.18f, arrowOffsetY));
            }
            else if (transform.name == "player3Prefab(Clone)" && playerState.skill3Count < 8 && playerState.canSkill3) //��ų3
            {
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.125f);
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //���� ���� ������ �� ������ ����
                StartCoroutine(Skill3Cooldown()); //��ٿ� �ڷ�ƾ ����
                StartCoroutine(Skill3WithDelay(0.18f, arrowOffsetY));
            }
            else //�⺻ ����
            {
                nowAttack = true;
                this.animator.SetTrigger("AtkTrigger");
                canAttack = false; //���� ���� ������ �� ������ ����
                StartCoroutine(AttackCooldown()); //��ٿ� �ڷ�ƾ ����
                StartCoroutine(AttackWithDelay(0.18f, arrowOffsetY));
            }
            //���� ȿ����
            soundManager.PlayAttackSound();
        } 
        //-------------------------------------------------------------------------------------------
        // ��ų4 ---------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftShift) && transform.name == "player4Prefab(Clone)" && canAttack2 && playerState.canSkill4)
        {
            if (playerState.rainCount < 2)
            {
                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().DecreasePlayerMp(0.5f);
                canAttack2 = false;
                nowAttack = true;
                this.animator.SetTrigger("Atk2Trigger");
                canAttack = false; //���� ���� ������ �� ������ ����
                StartCoroutine(SpawnRainPrefabs());
                soundManager.PlayAttackSound();
            }
        }//-----------------------------------------------------------------------------------

        //�÷��̾ ȭ�� ������ ������ ó������
        if (transform.position.y < -10)
        {
            //���� �������� �� ��������
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        // ���� ȹ�� ���� ����-------------------------------------------------------------
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        if (boss1.Length == 0 && boss2.Length == 0 && boss3.Length == 0 && boss4.Length == 0)
        {
            visible = true;
        }
        else
        {
            visible = false;
        }
        //---------------------------------------------------------------------------------
    }

    private void LoadKeyBindings()
    {
        moveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("���� �̵�", KeyCode.LeftArrow.ToString()));
        moveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("���� �̵�", KeyCode.RightArrow.ToString()));
        moveDownKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("�Ʒ�", KeyCode.DownArrow.ToString()));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("����", KeyCode.LeftAlt.ToString()));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("����", KeyCode.LeftControl.ToString()));
        rollKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("������", KeyCode.Z.ToString()));
    }

    // ������ ��Ÿ�� ���� �Լ�
    IEnumerator RollCooldown()
    {
        canRoll = false; // ������ ��� �Ұ�
        yield return new WaitForSeconds(rollCooldown); // ������ ��Ÿ�� �ð� ���� ���
        canRoll = true; // ������ �ٽ� ��� ����
    }

    // ��ų 1 ---------------------------------------------------------------------------
    //���� ��� �� skill1 �߻�(�Ķ����(���� ��� ���� �� ������, ȭ�� �߻� ����)
    IEnumerator Skill1WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        // ��� �ð� �� ���� �߻�
        Skill1(offsetY);
        nowAttack = false;
    }
    //1�ʵ��� skill1�� �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator Skill1Cooldown()
    {
        yield return new WaitForSeconds(skill1Cooldown);
        canAttack = true; //1���� ���� ����
    }
    //�÷��̾� skill1
    void Skill1(float offsetY)
    {
        if (skill1Prefab != null && arrowSpawnPoint != null)
        {
            //ȭ�� ��ġ ����
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);
            //ȭ�� ����
            float arrowDirection = transform.localScale.x;

            // ȭ�� ���� �� 100�� �ݽð� �������� ȸ��
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z�� ���� 115�� ȸ��

            // �÷��̾ ������ ���� ������ �ݴ� �������� ȸ�� (Z�� ���� 115�� ȸ��)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // ������ ��ġ�� ȸ������ ȭ�� ����
            GameObject arrow = Instantiate(skill1Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //ȭ�쿡 ������ ���� �߰��� �߻�
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill1 prefab or spawn point is not set");
        }
        //��ų ��� Ƚ�� ����(=��ų ��� ���� Ƚ�� ����)
        playerStateManager.GetComponent<PlayerStateManager>().Skill1CountUp();
    }
    // --------------------------------------------------------------------------------------

    // ��ų 2 ---------------------------------------------------------------------------
    //���� ��� �� skill2 �߻�
    IEnumerator Skill2WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Skill2(offsetY);
        nowAttack = false;
    }
    //1�ʵ��� skill2�� �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator Skill2Cooldown()
    {
        yield return new WaitForSeconds(skill2Cooldown);
        canAttack = true; //1���� ���� ����
    }
    //�÷��̾� skill2
    void Skill2(float offsetY)
    {
        if (skill2Prefab != null && arrowSpawnPoint != null)
        {
            //ȭ�� ��ġ ����
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);

            float arrowDirection = transform.localScale.x;

            // ȭ�� ���� �� 100�� �ݽð� �������� ȸ��
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z�� ���� 115�� ȸ��

            // �÷��̾ ������ ���� ������ �ݴ� �������� ȸ�� (Z�� ���� 230�� ȸ��)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // ������ ��ġ�� ȸ������ ȭ�� ����
            GameObject arrow = Instantiate(skill2Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //ȭ�쿡 ������ ���� �߰��� �߻�
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill2 prefab or spawn point is not set");
        }
        playerStateManager.GetComponent<PlayerStateManager>().Skill2CountUp();
    }
    // --------------------------------------------------------------------------------------

    // ��ų 3 ---------------------------------------------------------------------------
    //���� ��� �� skill3 �߻�
    IEnumerator Skill3WithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Skill3(offsetY);
        nowAttack = false;
    }
    //1�ʵ��� skill3�� �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator Skill3Cooldown()
    {
        yield return new WaitForSeconds(skill3Cooldown);
        canAttack = true; //1���� ���� ����
    }
    //�÷��̾� skill3
    void Skill3(float offsetY)
    {
        if (skill3Prefab != null && arrowSpawnPoint != null)
        {
            //ȭ�� ��ġ ����
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);

            float arrowDirection = transform.localScale.x;

            // ȭ�� ���� �� 100�� �ݽð� �������� ȸ��
            Quaternion arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, 115); // Z�� ���� 115�� ȸ��

            // �÷��̾ ������ ���� ������ �ݴ� �������� ȸ�� (Z�� ���� 230�� ȸ��)
            if (arrowDirection < 0)
            {
                arrowRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, 0, -115);
            }

            // ������ ��ġ�� ȸ������ ȭ�� ����
            GameObject arrow = Instantiate(skill3Prefab, arrowPosition, arrowRotation);

            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //ȭ�쿡 ������ ���� �߰��� �߻�
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * skill1Speed, 0);
            }
        }
        else
        {
            Debug.LogError("skill3 prefab or spawn point is not set");
        }
        playerStateManager.GetComponent<PlayerStateManager>().Skill3CountUp();
    }
    // --------------------------------------------------------------------------------------

    // ��ų4(�÷��̾� �Ӹ� ������ �������� ȭ�� ���� �� �밢������ �߻�)------------------------------------
    // RainPrefab�� ������� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnRainPrefabs()
    {
        Vector3 playerPos = transform.position;
        float direction = transform.localScale.x; // �÷��̾ �������� ���� ������ 1, ������ ���� ������ -1

        yield return new WaitForSeconds(1.0f); // 1�� ���
        nowAttack = false;

        float offsetY = 5.0f; // �÷��̾� �Ӹ� �� ���� �Ÿ�
        float spawnInterval = 0.2f; // ���� ����
        float rowOffset = 0.5f; // �� ���� x ������
        int rainCount = 5; // ������ RainPrefab ��
        float fallAngleY = -0.90f; // �밢�� ���� ���� ���� (�� ������ �ϸ�, �� ũ�� ���ĸ�)

        for (int i = 0; i < rainCount; i++)
        {
            // �� ���� ȭ���� ������ ����
            for (int j = -1; j <= 1; j++)
            {
                // RainPrefab�� �÷��̾� �Ӹ� ���� ����
                Vector3 spawnPosition = playerPos + new Vector3(j * rowOffset, offsetY, 0);
                GameObject rain = Instantiate(RainPrefab, spawnPosition, Quaternion.identity);

                // �÷��̾ �ٶ󺸴� ���⿡ ���� �밢������ ���������� ����
                Rigidbody2D rainRb = rain.GetComponent<Rigidbody2D>();
                Vector2 fallDirection = new Vector2(direction, fallAngleY).normalized; // �밢�� ���� ����
                float rainSpeed = 15.0f; // RainPrefab�� �������� �ӵ�
                rainRb.velocity = fallDirection * rainSpeed;

                // �������� ���� ���� �� �̹��� �¿� ����
                if (direction > 0)
                {
                    rain.transform.localScale = new Vector3(-1, 1, 1); // �¿� ����
                    rain.transform.rotation = Quaternion.Euler(0, 0, 15); // ������ �밢�� ���� (��: -45��)
                }
                else
                {
                    rain.transform.localScale = new Vector3(1, 1, 1); // ���� ���� ����
                    rain.transform.rotation = Quaternion.Euler(0, 0, -15); // ���� �밢�� ���� (��: 45��)
                }
            }

            // ���� RainPrefab �������� ���
            yield return new WaitForSeconds(spawnInterval);
        }
        canAttack2 = true;
        playerStateManager.GetComponent<PlayerStateManager>().RainCountUp();
    }
    // -------------------------------------------------------------------------------------------


    
    // �⺻ ���� ---------------------------------------------------------------------------------
    //���� ��� �� ȭ�� �߻�
    IEnumerator AttackWithDelay(float delay, float offsetY)
    {
        yield return new WaitForSeconds(delay);
        Attack(offsetY);
        nowAttack = false;
    }
    //1�ʵ��� ������ �� �� ���� �ϴ� �ڷ�ƾ
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; //1���� ���� ����
    }
    //�÷��̾� ����
    void Attack(float offsetY)
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            //ȭ�� ��ġ ����
            Vector3 arrowPosition = arrowSpawnPoint.position + new Vector3(0, offsetY, 0);
            //������ ��ġ�� ȭ�� ����
            GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowSpawnPoint.rotation);

            //�÷��̾ ���� ����
            float arrowDirection = transform.localScale.x;
            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //ȭ�쿡 ������ ���� �߰��� �߻�
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * arrowSpeed, 0);
            }
        }
        else
        {
            Debug.LogError("Arrow prefab or spawn point is not set");
        }
    }
    // --------------------------------------------------------------------------------------

    //�÷��̾� ���� ����---------------------------------------------------------------------
    public void PlayerHit()
    {
        nowHit = true;
        soundManager.PlayHitSound();
        this.animator.SetTrigger("HitTrigger");
        StartCoroutine(HitTime(0.5f));
    }
    //�´� ���� ������ ����
    IEnumerator HitTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }
    // ---------------------------------------------------------------------------------------

    //�÷��̾� �״� �ڵ� ---------------------------------------------------------------------
    //�÷��̾ �״� �ִϸ��̼� �߻� �� 2�� �� ������� �����̸� �ֱ� ����
    IEnumerator PlayerDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnAtDeathPosition(); // ���� ��ġ���� ��Ȱ
    }
    public void PlayerDeath()
    {
        if (!isDead)
        {
            isDead = true;  // �÷��̾ �׾����� ǥ��
            deathPosition = transform.position; // ���� ��ġ ����
            this.animator.SetTrigger("DeathTrigger");
            StartCoroutine(PlayerDeathWithDelay(2.0f));
        }
    }

    // �׾��� �ڸ����� ��Ȱ�ϴ� �޼���
    private void RespawnAtDeathPosition()
    {
        transform.position = deathPosition; // ����� ��ġ�� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
        //ü�� �ʱ�ȭ
        this.playerHP.GetComponent<Image>().fillAmount = 1.0f;
    }
    //----------------------------------------------------------------------------------------

    //����-------------------------------------------------------------------------------------
    void Jump1()
    {
        this.animator.SetTrigger("JumpTrigger");
        this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0); // ���� y �ӵ� ����
        this.rigid2D.AddForce(Vector2.up * this.jumpForce);
    }
    void Jump2()
    {
        this.animator.SetTrigger("Jump2Trigger");
        this.rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0); // ���� y �ӵ� ����
        this.rigid2D.AddForce(Vector2.up * this.jumpForce); 
    }
    //-----------------------------------------------------------------------------------------------

    // ��򰡿� ��ų� �������� ��-----------------------------------------------------------------------
    //�ٴ�, ���������� ��Ҵ��� Ȯ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //��������(�Ʒ��� �������� ����)������ ��
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            isGrounded = true;
            canDoubleJump = false; // �ٴڿ� ������ 2�� ���� �ʱ�ȭ
            canDown = true; //�������� ���� ����
        }
        else if (collision.gameObject.CompareTag("Ground")) //�������� �Ұ��� ��
        {
            isGrounded = true;
            canDoubleJump = false; // �ٴڿ� ������ 2�� ���� �ʱ�ȭ
            canDown = false;

        }
        else if (collision.gameObject.CompareTag("Cliff")) //stage4_Boss
        {
            Respawn2(); // �÷��̾ ������ ��ġ�� �̵���Ű�� ü���� �ް� �ϴ� �Լ� ȣ��
        }
        else if (collision.gameObject.CompareTag("FireCliff")) //stage2_3
        {
            Respawn1();
        } 
        else if (collision.gameObject.CompareTag("FireCliff2")) //staeg3_3
        {
            Respawn3();
        }
        else if (collision.gameObject.CompareTag("Fall")) //stage4_2
        {
            Respawn4();
        }
        else if (collision.gameObject.CompareTag("Fall2")) //stage4_3
        {
            Respawn5();
        }

        //���ɿ� ����� ��
        if (collision.gameObject.CompareTag("sentiment0"))
        {
            playerState.canSkill0 = true; //�⺻���� Ȱ��ȭ
            Destroy(collision.gameObject); // sentiment0 ����
        }
        //���� ȹ�� �����ϰ� ���ɿ� ����� ��
        if (collision.gameObject.CompareTag("sentiment1") && visible)
        {
            playerState.canSkill1 = true;   // ���ݰ�ȭ1 ����
            Destroy(collision.gameObject);  // sentiment1 ����
        }
        if (collision.gameObject.CompareTag("sentiment2") && visible)
        {
            playerState.canSkill2 = true;   // ���ݰ�ȭ2 ����
            Destroy(collision.gameObject);  // sentiment2 ����
        }
        if (collision.gameObject.CompareTag("sentiment3") && visible)
        {
            playerState.canSkill3 = true;   // ���ݰ�ȭ3 ����
            Destroy(collision.gameObject);  // sentiment3 ����
        }
        if (collision.gameObject.CompareTag("sentiment4") && visible)
        {
            playerState.canSkill4 = true;   // ��ų4 ����
            Destroy(collision.gameObject);  // sentiment4 ����
        }
    }
    //��򰡿��� �������� ��(���߿� ���� ��)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap") || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            canDoubleJump = true;
            canDown = false;
        }
    }
    // ---------------------------------------------------------------------------------------

    // �÷��̾��� isTrigger�� ��� Ȱ��ȭ�ϴ� �ڷ�ƾ(��ü�� ���� �ʾƼ� ���� ������ ������)------
    IEnumerator EnableTriggerTemporarily()
    {
        if (playerCollider != null)
        {
            this.animator.SetTrigger("DownTrigger");
            playerCollider.isTrigger = true;  // �÷��̾� �ݶ��̴� Ʈ����(��ü�� ���� �ʰ���) Ȱ��ȭ
            yield return new WaitForSeconds(0.3f);  // 0.2�� ���� Ȱ��ȭ ����
            playerCollider.isTrigger = false;  // Ʈ���� ��Ȱ��ȭ
        }
    }
    //-----------------------------------------------------------------------------------------

    //�÷��̾ Ư�� ��ǥ�� �����ϴ� �޼���-----------------------------------------------------
    void Respawn1() //stage2_3
    {
        transform.position = respawnPosition1; // ������ ��ġ�� �÷��̾� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            //ü�� ����
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }

    void Respawn2() //stage4_Boss
    {
        transform.position = respawnPosition2; // ������ ��ġ�� �÷��̾� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }
    
    void Respawn3() //stage3_3
    {
        transform.position = respawnPosition3; // ������ ��ġ�� �÷��̾� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
    }
    
    void Respawn4() //stage4_2
    {
        transform.position = respawnPosition4; // ������ ��ġ�� �÷��̾� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }
    
    void Respawn5() //stage4_3
    {
        transform.position = respawnPosition5; // ������ ��ġ�� �÷��̾� �̵�
        rigid2D.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDead = false;
        nowAttack = false;
        nowHit = false;
        GameObject director = GameObject.Find("GameDirector");
        if (director != null)
        {
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
        }
    }

    // ���� �ڵ�(������ ������, ������ �� ����Ʈ ������)------------------------------------------------
    public void TransformToPrefab(GameObject playerPrefab, GameObject changePrefab) //����1
    {
        // �������� �����Ǿ� �ִ��� Ȯ��
        if (playerPrefab != null)
        {
            //���� ���� Ƚ�� �ʱ�ȭ
            this.playerMP.GetComponent<Image>().fillAmount = 1.0f;
            if (playerPrefab != player0Prefab)
            {
                ShowChangePrefab(changePrefab);
            }
            // ���� ��ġ�� �÷��̾� ������ �������� �����ϰ� �Ӽ�(��ġ, ����) ����
            GameObject newPlayer = Instantiate(playerPrefab, transform.position, transform.rotation);
            newPlayer.transform.localScale = transform.localScale; // ���� ����

            // CameraController�� ���ο� �÷��̾� ������Ʈ�� ����
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.SetPlayer(newPlayer);
            }
            //������, ��ų ���� Ȯ�� ������Ʈ���� ������ �÷��̾� ������Ʈ ����
            Boss1Controller boss1Controller = FindAnyObjectByType<Boss1Controller>();
            if (boss1Controller != null)
            {
                boss1Controller.SetPlayer(newPlayer);
            }
            BossAgent bossAgent = FindAnyObjectByType<BossAgent>();
            if (bossAgent != null)
            {
                bossAgent.SetPlayer(newPlayer);
            }
            Boss2Controller boss2Controller = FindAnyObjectByType<Boss2Controller>();
            if (boss2Controller != null)
            {
                boss2Controller.SetPlayer(newPlayer);
            }
            BossAgent2 bossAgent2 = FindAnyObjectByType<BossAgent2>();
            if (bossAgent2 != null)
            {
                bossAgent2.SetPlayer(newPlayer);
            }
            Boss3Controller boss3Controller = FindAnyObjectByType<Boss3Controller>();
            if (boss3Controller != null)
            {
                boss3Controller.SetPlayer(newPlayer);
            }
            BossAgent3 bossAgent3 = FindAnyObjectByType<BossAgent3>();
            if (bossAgent3 != null)
            {
                bossAgent3.SetPlayer(newPlayer);
            }
            Boss4Controller boss4Controller = FindAnyObjectByType<Boss4Controller>();
            if (boss4Controller != null)
            {
                boss4Controller.SetPlayer(newPlayer);
            }
            BossAgent4 bossAgent4 = FindAnyObjectByType<BossAgent4>();
            if (bossAgent4 != null)
            {
                bossAgent4.SetPlayer(newPlayer);
            }
            PlayerStateManager playerStateManager = FindAnyObjectByType<PlayerStateManager>();
            if (playerStateManager != null)
            {
                playerStateManager.SetPlayer(newPlayer);
            }

            // ���� �÷��̾� ����
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Player prefab is not set.");
        }
    }

    //�÷��̾� ���Ž� ������ ������Ʈ�� ����
    public void ShowChangePrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
    //---------------------------------------------------------------------------------------
}