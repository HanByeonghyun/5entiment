using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//학습 시키는 코드
public class BossAgent1 : Agent
{
    Animator animator;

    public GameObject player;           // 플레이어
    public GameObject arrowPrefab;      // 화살 프리팹
    public float arrowSpeed = 10.0f;    // 화살 속도
    public Transform arrowSpawnPoint;   // 화살 생성 위치

    public float speed = 4.0f;      // 보스 속도
    private Vector3 bossStartPosition;   // 보스 시작 위치
    public float downMax = -2.55f;
    public float upMax = 3.5f;

    public float bossCurrentPositionY;
    public float playerCurrentPositionY;
    public GameObject windPrefab;       // 바람 프리팹
    public float windSpeed = 5.0f;      // 바람 속도
    public Transform windSpawnPoint;    // 바람 생성 위치
    public bool canAttack = true;       // 공격 가능 여부
    public float attackCooldown = 3.0f;   // 공격 쿨타임

    public bool isDead = false;

    public int consecutiveHits = 0;  // 연속 히트 수를 추적하는 변수

    public int playerHp = 80;
    public int bossHp = 40;
    private int initialPlayerHp;        // 초기 플레이어 체력
    private int initialBossHp;          // 초기 보스 체력

    private int stepCounter = 0;             // 에피소드 내 스텝 카운트
    private const int maxStepsPerEpisode = 5000;  //5000; // 최대 스텝 수

    private float episodeTimer = 0f;         // 에피소드 타이머
    private const float maxEpisodeTime = 30f; // 최대 에피소드 시간 (30초)

    private Coroutine arrowCoroutine; // 화살 코루틴을 추적하기 위한 변수

    private PlayerProjectTile[] playerProjectTiles; // PlayerProjectTile 스크립트 참조

    private PlayerProjectTile playerHealth;

    public float distanceToPlayer;
    public float distanceTime = 3.0f;
    public float nowTime;



    //초기 설정
    public override void Initialize()
    {
        bossStartPosition = transform.position;
        player = GameObject.FindWithTag("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;

        // 씬의 모든 PlayerProjectTile 컴포넌트를 가져와 배열에 저장
        playerProjectTiles = Object.FindObjectsByType<PlayerProjectTile>(FindObjectsSortMode.None);

        // 배열 중 하나를 playerHealth로 참조할 수 있습니다. (예: 첫 번째 요소)
        if (playerProjectTiles.Length > 0)
        {
            playerHealth = playerProjectTiles[0]; // 첫 번째 PlayerProjectTile을 참조
        }

        playerHealth = player.GetComponent<PlayerProjectTile>();
    }

    //에피소드 시작시 초기화
    public override void OnEpisodeBegin()
    {
        this.animator = GetComponent<Animator>();

        // 기존에 실행 중인 코루틴이 있으면 중지
        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }

        transform.position = bossStartPosition;
        bossCurrentPositionY = transform.position.y;

        //각 플레이어의 위치를 무작위로 설정
        foreach (var playerProjectTile in playerProjectTiles)
        {
            playerProjectTile.ResetPlayer();
        }


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
        if (isDead) return;

        nowTime = Time.time + distanceTime;
        // 플레이어와 보스의 y축 거리
        distanceToPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);
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

        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
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

    //에이전트가 환경에서 수집할 관찰 데이터를 정의
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(playerHp / (float)initialPlayerHp);   //플레이어 체력 비율
        sensor.AddObservation(transform.position);                  //보스 위치
        sensor.AddObservation(player.transform.position);           //플레이어 위치
        sensor.AddObservation(bossHp / (float)initialBossHp);       //보스의 체력 비율
    }

    //에이전트가 행동을 취할 때 호출
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isDead) { return; }
        //보스의 결정 가능한 첫 번째 행동(상하 움직임)
        float moveY = actions.ContinuousActions[0];
        //결정에 따른 움직임
        bossCurrentPositionY += moveY * speed * Time.deltaTime;
        bossCurrentPositionY = Mathf.Clamp(bossCurrentPositionY, downMax, upMax);
        transform.position = new Vector3(transform.position.x, bossCurrentPositionY, 0);

        //보스의 결정 가능한 두 번째 행동(공격)
        float attackAction = actions.ContinuousActions[1];
        if (attackAction > 0.5f && canAttack)
        {
            BossAtk();
        }

        // 스텝 수와 타이머 증가
        stepCounter++;
        episodeTimer += Time.deltaTime;

        // 최대 스텝 또는 최대 시간에 도달하면 에피소드 종료
        if (stepCounter >= maxStepsPerEpisode || episodeTimer >= maxEpisodeTime)
        {
            EndEpisodeWithRewards();
        }
    }

    //목표물과 닿았을 때
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "arrow")
    //    {
    //        this.animator.SetTrigger("HitTrigger");
    //    }
    //}

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
        while(true)
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
    public void BossAtk()
    {
        canAttack = false; //공격 직후 공격할 수 없도록 설정
        StartCoroutine(AttackCooldown()); //쿨다운 코루틴 시작
        StartCoroutine(WindWithDelay(0.2f));
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; //3초후 공격 가능
    }
    //공격 모션 후 바람 발사
    IEnumerator WindWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Wind();
    }
    void Wind() //바람 
    {
        //바람 생성
        GameObject wind1 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);
        GameObject wind2 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);

        //플레이어가 보는 방향
        float windDirection = transform.localScale.x;
        wind1.transform.localScale = new Vector3(windDirection, 1, 1);
        wind2.transform.localScale = new Vector3(windDirection * (-1), 1, 1);

        //바람에 물리적 힘을 추가해 발사
        Rigidbody2D windRb1 = wind1.GetComponent<Rigidbody2D>();
        Rigidbody2D windRb2 = wind2.GetComponent<Rigidbody2D>();

        windRb1.velocity = new Vector2(windDirection * windSpeed, 0);
        windRb2.velocity = new Vector2(windDirection * windSpeed * (-1), 0);
    }

    public void EndEpisodeWithRewards()
    {
        // 플레이어 체력이 50% 이하일 경우 추가 보상
        if (playerHealth.playerHp / (float)initialPlayerHp <= 0.5f)
        {
            AddReward(10f);  // 추가 보상
        }
        // 플레이어 체력이 높을수록 패널티가 커지도록 설정
        AddReward(-(playerHealth.playerHp / (float)initialPlayerHp) * 5f);

        StopCoroutine("Arrow"); // 화살 중지
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 비워두거나 기본값을 설정해 경고 메시지 방지
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // 움직임을 0으로 설정
        continuousActions[1] = 0f; // 공격을 하지 않음
    }
}
