using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

public class BossAgent2 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    public GameObject player;           // 플레이어

    public float speed = 6.0f;      // 보스 속도
    private Vector3 bossStartPosition;   // 보스 시작 위치
    public float leftMax = -12.0f;
    public float rightMax = 12.0f;

    //보스의 상태 플래그
    bool isDead = false;
    bool isAttacking = false;

    public float bossCurrentPositionX;
    public GameObject atkPrefab;       // 공격 프리팹
    public Transform atkSpawnPoint;    // 공격 생성 위치
    public bool canAttack1 = true;       // 공격 가능 여부
    public float attackCooldown1 = 3.0f;   // 공격 쿨타임

    public float fireBallSpeed = 10.0f;
    public GameObject fireBallPrefab;       // 공격 프리팹
    public Transform fireBallSpawnPoint;    // 공격 생성 위치
    public bool canAttack2 = true;       // 공격 가능 여부
    public float attackCooldown2 = 3.0f;   // 공격 쿨타임

    public int playerHp = 70;
    private int initialPlayerHp;        // 초기 플레이어 체력
    public int bossHp = 90;
    private int initialBossHp;

    public float distanceToPlayer;


    //초기 설정
    public override void Initialize()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        //bossStartPosition = transform.position;
        player = GameObject.FindWithTag("player");
        initialPlayerHp = playerHp;
        initialBossHp = bossHp;
    }

    //에피소드 시작시 초기화
    public override void OnEpisodeBegin()
    {
        bossStartPosition = transform.position;
        transform.position = bossStartPosition;
        bossCurrentPositionX = transform.position.x;

        playerHp = initialPlayerHp;
        bossHp = initialBossHp;
    }

    public void Update()
    {
        if (!isAttacking) { return; }

        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
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
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
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

    //목표물과 닿았을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss22Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss22Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss22Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss22Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss22Hp(0.1f);
        }
    }
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(1.0f);
        speed = originalSpeed;
    }

    //보스 공격
    public void BossAtk1()
    {
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
    {
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
        Vector3 spawnPosition = fireBallSpawnPoint.position + new Vector3(0, -1.0f, 0);

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
