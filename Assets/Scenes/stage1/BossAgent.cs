using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//학습 적용하는 코드
public class BossAgent : Agent
{
    Animator animator;

    public GameObject player;           // 플레이어
    //public GameObject arrowPrefab;      // 화살 프리팹
    //public float arrowSpeed = 10.0f;    // 화살 속도
    //public Transform arrowSpawnPoint;   // 화살 생성 위치

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
    public bool nowAttack = false;
    public bool nowHit = false;

    //public int consecutiveHits = 0;  // 연속 히트 수를 추적하는 변수

    public int playerHp = 80;           // 현재 플레이어 체력
    public int bossHp = 50;             // 현재 보스 체력
    private int initialPlayerHp;        // 초기 플레이어 체력
    private int initialBossHp;          // 초기 보스 체력


    //초기 설정
    public override void Initialize()
    {
        bossStartPosition = transform.position;
        player = GameObject.FindWithTag("player");
        initialBossHp = bossHp;
        initialPlayerHp = playerHp;
    }

    //에피소드 시작시 초기화
    public override void OnEpisodeBegin()
    {
        this.animator = GetComponent<Animator>();

        transform.position = bossStartPosition;
        bossCurrentPositionY = transform.position.y;

        //플레이어와 보스의 체력 초기화
        playerHp = initialPlayerHp;
        bossHp = initialBossHp;
    }

    public void Update()
    {
        if (isDead) return;
        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
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
    }

    //목표물(플레이어의 공격)과 닿았을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            this.animator.SetTrigger("HitTrigger");
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            this.animator.SetTrigger("HitTrigger");
            //강화공격3에 맞았을 때 속도 감소
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss11Hp(0.1f);
        }
    }
    //속도 감소 함수
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f);
        speed = originalSpeed;
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

    //보스 죽음
    public void BossDeath()
    {
        if (!isDead)
        {
            isDead = true;
            // 보스의 다른 동작이 실행되지 않도록 변수 초기화
            canAttack = false;

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

    //에피소드 종료
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
