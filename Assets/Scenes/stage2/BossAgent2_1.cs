using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

public class BossAgent2_1 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    public GameObject player;           // 플레이어
    public GameObject arrowPrefab;      // 화살 프리팹
    public float arrowSpeed = 7.0f;    // 화살 속도
    public Transform arrowSpawnPoint;   // 화살 생성 위치

    public float speed = 6.0f;      // 보스 속도
    private Vector3 bossStartPosition;   // 보스 시작 위치
    public float leftMax = -12.0f;
    public float rightMax = 12.0f;

    //보스의 상태 플래그
    bool isDead = false;
    bool isAttacking = false;

    public float bossCurrentPositionX;
    public float playerCurrentPositionY;
    public GameObject atkPrefab;       // 공격 프리팹
    public Transform atkSpawnPoint;    // 공격 생성 위치
    public bool canAttack1 = true;       // 공격 가능 여부
    public float attackCooldown1 = 3.0f;   // 공격 쿨타임

    public float fireBallSpeed = 6.0f;
    public GameObject fireBallPrefab;       // 공격 프리팹
    public Transform fireBallSpawnPoint;    // 공격 생성 위치
    public bool canAttack2 = true;       // 공격 가능 여부
    public float attackCooldown2 = 3.0f;   // 공격 쿨타임

    public int consecutiveHits = 0;  // 연속 히트 수를 추적하는 변수

    public int playerHp = 70;
    private int initialPlayerHp;        // 초기 플레이어 체력
    public int bossHp = 90;
    private int initialBossHp;

    private int stepCounter = 0;             // 에피소드 내 스텝 카운트
    private const int maxStepsPerEpisode = 5000;  //5000; // 최대 스텝 수

    private float episodeTimer = 0f;         // 에피소드 타이머
    private const float maxEpisodeTime = 30.0f; // 최대 에피소드 시간 (30초)

    private Coroutine arrowCoroutine; // 화살 코루틴을 추적하기 위한 변수

    private PlayerProjectTile2[] playerProjectTiles; // PlayerProjectTile 스크립트 참조

    private PlayerProjectTile2 playerHealth;

    public float distanceToPlayer;
    public float distanceTime = 3.0f;
    public float nowTime;

    //초기 설정
    public override void Initialize()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("player");
        initialPlayerHp = playerHp;
        initialBossHp = bossHp;

        // 씬의 모든 PlayerProjectTile 컴포넌트를 가져와 배열에 저장
        playerProjectTiles = Object.FindObjectsByType<PlayerProjectTile2>(FindObjectsSortMode.None);

        playerHealth = player.GetComponent<PlayerProjectTile2>();
    }

    //에피소드 시작시 초기화
    public override void OnEpisodeBegin()
    {
        // 기존에 실행 중인 코루틴이 있으면 중지
        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }

        bossStartPosition = transform.position;
        transform.position = bossStartPosition;
        bossCurrentPositionX = transform.position.x;

        //각 플레이어의 위치를 무작위로 설정
        foreach (var playerProjectTile in playerProjectTiles)
        {
            playerProjectTile.ResetPlayer();
        }

        ResetConsecutiveHits();

        playerCurrentPositionY = player.transform.position.y;

        playerHp = initialPlayerHp;
        bossHp = initialBossHp;

        stepCounter = 0;            // 스텝 초기화
        episodeTimer = 0f;          // 타이머 초기화
        consecutiveHits = 0;

        arrowCoroutine = StartCoroutine(Arrow());
    }

    public void Update()
    {
        if (!isAttacking) { return; }

        nowTime = Time.time + distanceTime;
        // 플레이어와 보스 사이의 총 거리
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (Time.time >= nowTime) // 3초마다
        {
            if (distanceToPlayer < 2.0f) //거리가 2보다 가까울 때
            {
                AddReward(5.0f); //보상5점
            }
            else if (distanceToPlayer < 4.0f) //거리가 4보다 가까울 때
            {
                AddReward(1.0f); //보상1점
            }
            else //거리가 4보다 멀 때
            {
                AddReward(-3.0f); //패널티 -3점
            }
        }

        // 스텝 수와 타이머 증가
        stepCounter++;
        episodeTimer += Time.deltaTime;

        // 최대 스텝 또는 최대 시간에 도달하면 에피소드 종료
        if (stepCounter >= maxStepsPerEpisode || episodeTimer >= maxEpisodeTime)
        {
            EndEpisodeWithRewards();
        }

        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
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
    }

    //에이전트가 환경에서 수집할 관찰 데이터를 정의
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(playerHp / (float)initialPlayerHp);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(bossHp / (float)initialBossHp);
    }

    //에이전트가 행동을 취할 때 호출
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
            // 현재 보스와 플레이어의 거리를 계산
            if (distanceToPlayer <= 3.5f && canAttack1)
            {
                canAttack1 = false;
                BossAtk1();
            }
            else if (distanceToPlayer > 3.5f && canAttack2)
            {
                canAttack2 = false;
                BossAtk2();
            }

        }
    }

    public void IncreaseConsecutiveHits()
    {
        consecutiveHits++;  // 연속 히트 수 증가
    }
    public void ResetConsecutiveHits()
    {
        consecutiveHits = 0;  // 연속 히트 수 초기화
    }

    private IEnumerator Arrow()
    {
        while (true)
        {
            //화살 위치 조정
            Vector3 arrowPosition = arrowSpawnPoint.position;
            //조정한 위치에 화살 생성
            GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowSpawnPoint.rotation);

            //플레이어가 보는 방향
            float arrowDirection = player.transform.localScale.x;
            arrow.transform.localScale = new Vector3(arrowDirection, 1, 1);

            //화살에 물리적 힘을 추가해 발사
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = new Vector2(arrowDirection * arrowSpeed, 0);
            }

            yield return new WaitForSeconds(3.0f);
        }
    }


    //보스 공격
    public void BossAtk1()
    {   //근거리 공격을 했을 때 거리가 가까우면 보상
        if (distanceToPlayer <= 3.5f) 
        {
            AddReward(5.0f);
        }
        isAttacking = true;
        this.animator.SetTrigger("Atk1Trigger");
        StartCoroutine(AttackCooldown1()); //쿨다운 코루틴 시작
        StartCoroutine(AttackWithDelay1(0.2f));
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown1()
    {
        yield return new WaitForSeconds(attackCooldown1);
        canAttack1 = true; //3초후 공격 가능
        isAttacking = false;
    }
    //공격 모션 후 공격 프리팹 생성
    IEnumerator AttackWithDelay1(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
    }

    void Attack() //공격
    {
        // 보스의 방향에 따라 x축 오프셋 조정
        float xOffset = transform.localScale.x < 0 ? 1.83f : -1.83f; // 보스가 오른쪽을 바라볼 때는 오른쪽으로, 왼쪽을 바라볼 때는 왼쪽으로 오프셋 적용
        float yOffset = -0.8f; // y축 오프셋은 항상 일정

        // 보스의 위치에 오프셋 적용
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);

        //공격 생성
        GameObject atk1 = Instantiate(atkPrefab, spawnPosition, atkSpawnPoint.rotation);

        //플레이어가 보는 방향
        float atkDirection = transform.localScale.x;
        atk1.transform.localScale = new Vector3(atkDirection, 1, 1);

        //공격 생성
        Rigidbody2D atkRb1 = atk1.GetComponent<Rigidbody2D>();

        atkRb1.velocity = new Vector2(0, 0);
    }

    //보스 공격
    public void BossAtk2()
    {   //원거리 공격을 했을 때 거리가 멀면 보상
        if (distanceToPlayer > 3.5f)
        {
            AddReward(5.0f);
        }
        isAttacking = true;
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(AttackCooldown2()); //쿨다운 코루틴 시작
        StartCoroutine(AttackWithDelay2(0.2f));
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown2()
    {
        yield return new WaitForSeconds(attackCooldown2);
        canAttack2 = true; //3초후 공격 가능
        isAttacking = false;
    }
    //공격 모션 후 fireball 발사
    IEnumerator AttackWithDelay2(float delay)
    {
        yield return new WaitForSeconds(delay);
        FireBall();
    }

    void FireBall() //공격
    {
        // spawn point보다 살짝 아래에서 발사되도록 위치 조정
        Vector3 spawnPosition = fireBallSpawnPoint.position + new Vector3(0, -1.5f, 0);

        // 수정된 위치에서 화염구 생성
        GameObject fireBall1 = Instantiate(fireBallPrefab, spawnPosition, fireBallSpawnPoint.rotation);

        //플레이어가 보는 방향
        float fireBallDirection = transform.localScale.x;
        fireBall1.transform.localScale = new Vector3(fireBallDirection * (-1), 1, 1);

        //바람에 물리적 힘을 추가해 발사
        Rigidbody2D fireBallRb1 = fireBall1.GetComponent<Rigidbody2D>();

        Rigidbody2D fireBallRb = fireBall1.GetComponent<Rigidbody2D>();
        if (fireBallRb != null)
        {
            fireBallRb.velocity = new Vector2(fireBallDirection * fireBallSpeed * (-1), 0);
        }
    }

    //보스 죽음
    public void BossDeath()
    {
        if (!isDead)
        {
            isDead = true;
            // 보스의 다른 동작이 실행되지 않도록 변수 초기화
            canAttack1 = false;
            canAttack2 = false;

            this.animator.SetTrigger("DeathTrigger");
            //DeathTrigger 발생 후 1.5초 뒤 보스 사라짐
            StartCoroutine(BossDeathWithDelay(2.5f));
        }
    }
    //보스가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    public void EndEpisodeWithRewards()
    {

        bossStartPosition = transform.position;

        StopCoroutine("Arrow"); // 이전 화살 코루틴 중지
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 비워두거나 기본값을 설정해 경고 메시지 방지
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // 움직임을 0으로 설정
        continuousActions[1] = 0f; // 공격을 하지 않음
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
