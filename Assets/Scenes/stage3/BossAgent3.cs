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

    // 보스의 X축 이동 범위
    public float leftMax = -8.0f; // X축 최소값
    public float rightMax = 18.0f;  // X축 최대값

    public GameObject player;           // 플레이어

    public float speed = 7.0f;      // 보스 속도

    //보스의 상태 플래그
    bool isDead = false;
    bool isAttacking = false;

    public float bossCurrentPositionX;

    public GameObject atkPrefab;       // 공격 프리팹
    public Transform atkSpawnPoint;    // 공격 생성 위치
    public bool canAttack1 = true;       // 공격 가능 여부
    public float attackCooldown1 = 2.0f;   // 공격 쿨타임

    public GameObject swamp;

    public GameObject swampPrefab;       // 웅덩이 프리팹
    public GameObject warningPrefab;
    public float pointX;                 // 웅덩이 생성 위치
    public float pointY;

    public bool canAttack2 = false;       // 공격 가능 여부
    public bool nowAttack = false;       // 현재 공격 상태
    public float attackCooldown2 = 3.0f;   // 공격 쿨타임

    public int playerHp = 90;
    public int bossHp = 100;
    private int initialPlayerHp;        // 초기 플레이어 체력
    private int initialBossHp;          // 초기 보스 체력

    public float distanceToPlayer;

    //초기 설정
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

    //에피소드 시작시 초기화
    public override void OnEpisodeBegin()
    {
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

    //목표물과 닿았을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss33Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
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

    //보스 공격
    public void BossAtk1()
    {
        isAttacking = true;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(AttackCooldown1()); //쿨다운 코루틴 시작
        StartCoroutine(AttackWithDelay1(0.2f));
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown1()
    {
        yield return new WaitForSeconds(attackCooldown1);
        canAttack1 = true; //2초후 공격 가능
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
        float xOffset = transform.localScale.x < 0 ? 0.5f : -0.5f; // 보스가 오른쪽을 바라볼 때는 오른쪽으로, 왼쪽을 바라볼 때는 왼쪽으로 오프셋 적용
        float yOffset = -0.2f; // y축 오프셋은 항상 일정

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

    //보스 공격3
    public void BossAtk2()
    {
        isAttacking = true;
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(SwampWithWarning(1.0f)); //1초 동안 경고 후 swamp 생성
    }
    //초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator AttackCooldown2()
    {
        yield return new WaitForSeconds(attackCooldown2);
        canAttack2 = true; //초후 공격 가능
        isAttacking = false;
    }

    //웅덩이 관련 함수
    //웅덩이 생성
    void Swamp() //swamp 
    {
        if (swampPrefab != null)
        {
            GameObject newSwamp = Instantiate(swampPrefab); // 새 웅덩이 생성
            newSwamp.transform.position = new Vector3(pointX, (-2.0f), 0);
            canAttack2 = false;
            StartCoroutine(AttackCooldown2()); //쿨다운 코루틴 시작
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
        ShowWarning(); // 경고 신호 함수
        yield return new WaitForSeconds(warningDuration); // 예고 시간동안 대기

        Swamp(); // 실제 Swamp 함수 호출
    }

    void ShowWarning()
    {
        // 예를 들어 경고 마커를 생성하거나 해당 위치의 색을 변화시키는 등의 로직 구현
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(pointX, -2, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1초 후에 경고 마커 제거
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
            nowAttack = false;

            this.animator.SetTrigger("DeathTrigger");
            //DeathTrigger 발생 후 2.5초 뒤 보스 사라짐
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
