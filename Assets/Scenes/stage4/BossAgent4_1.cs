using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections;

//보스 학습용 코드
public class BossAgent4_1 : Agent
{
    Animator animator;
    Rigidbody2D rigid2D;

    public GameObject player;           // 플레이어

    private Vector3 bossStartPosition;   // 보스 시작 위치

    public bool visible = true;          // 보스 보이는지
    float invisibleDuration = 1.0f;      // 보스 안보이는 시간

    public bool canHide = true;
    public float canHideCooldown = 3.0f;

    public GameObject warningPrefab;

    public GameObject windmillPrefab;       // 윈드밀 프리팹
    public float windmillSpeed = 5.0f;      // 윈드밀 속도
    public Transform windmillSpawnPoint;    // 윈드밀 생성 위치

    public GameObject eBallPrefab;       // 에너지볼 프리팹

    //에너지볼 생기는 위치 변수
    public float pointX;
    public float pointY;

    public bool isDead = false;

    public int bossHp = 40;
    private int initialBossHp;          // 초기 보스 체력

    public int consecutiveHits = 0;

    public Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-5.7f, 4.2f),
        new Vector2(-2.8f, 5.9f),
        new Vector2(11.1f, 7.5f),
        new Vector2(14.4f, 4.2f)
    };


    //초기 설정
    public override void Initialize()
    {
        player = GameObject.Find("player");
        initialBossHp = bossHp;
        canHide = true;
    }

    //에피소드 시작시 초기화
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
        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
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

    //에이전트가 환경에서 수집할 관찰 데이터를 정의
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(bossHp / (float)initialBossHp);
    }

    public void IncreaseConsecutiveHits()
    {
        consecutiveHits++;  // 연속 히트 수 증가
    }
    public void ResetConsecutiveHits()
    {
        consecutiveHits = 0;  // 연속 히트 수 초기화
    }

    //에이전트가 행동을 취할 때 호출
    public override void OnActionReceived(ActionBuffers actions)
    {
        float hideAction = actions.ContinuousActions[0];

        // 순간이동 행동
        if (hideAction > 0.5f && canHide)
        {
            //nowAttack = true;
            canHide = false;
            BossHide(); // Hide 메서드를 호출하여 순간이동을 실행
        }
    }

    //목표물과 닿았을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            AddReward(-3.0f);
        }
    }

    //보스 죽음
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
        //BossDefeated(); // BossDefeated 메서드를 호출하여 보스의 죽음을 처리
        //DeathTrigger 발생 후 2.5초 뒤 보스 사라짐
        StartCoroutine(BossDeathWithDelay(2.0f));
    }
    //보스가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //보스 사라짐
    public void BossHide()
    {
        //this.animator.SetTrigger("HideTrigger");
        StartCoroutine(GoUnderground());
    }
    IEnumerator GoUnderground()
    {
        yield return new WaitForSeconds(0.3f); //애니메이션 시간동안 지연

        visible = false;
        GetComponent<SpriteRenderer>().enabled = false; //보스 숨기기
        GetComponent<Collider2D>().enabled = false; //충돌 비활성화
        rigid2D.isKinematic = true; //물리 효과 비활성화

        yield return new WaitForSeconds(invisibleDuration); //사라진 시간

        // 무작위 위치 선택
        int randomIndex = Random.Range(0, spawnPositions.Length);
        transform.position = spawnPositions[randomIndex];

        BossAppear(); //보스 나옴
    }

    //보스 나타남
    public void BossAppear()
    {
        int randomIndex = Random.Range(0, spawnPositions.Length);
        //Debug.Log("Selected spawn index: " + randomIndex + ", Position: " + spawnPositions[randomIndex]);
        transform.position = spawnPositions[randomIndex];

        //this.animator.SetTrigger("AppearTrigger");
        visible = true;
        GetComponent<SpriteRenderer>().enabled = true; //보스 보이기
        GetComponent<Collider2D>().enabled = true; //충돌 활성화
        rigid2D.isKinematic = false; //물리 효과 활성화

        //nowAttack = false;

        pointX = player.transform.position.x;
        pointY = player.transform.position.y;
        //nowAttack = true;
        BossAtk1();
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    IEnumerator HideCooldown()
    {
        yield return new WaitForSeconds(canHideCooldown);
        canHide = true; //3초후 공격 가능
    }


    //보스 공격
    public void BossAtk1()
    {
        //canAttack1 = false; //공격 직후 공격할 수 없도록 설정
        //StartCoroutine(AttackCooldown1()); //쿨다운 코루틴 시작
        StartCoroutine(windMillWithDelay(0.2f));
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    //IEnumerator AttackCooldown1()
    //{
    //    yield return new WaitForSeconds(attackCooldown1);
    //    //canAttack1 = true; //3초후 공격 가능
    //}
    //공격 모션 후 바람 발사
    IEnumerator windMillWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        windMill();
    }
    void windMill() //바람 
    {
        if (windmillPrefab != null && windmillSpawnPoint != null)
        {

            Vector3 windmillPosition = windmillSpawnPoint.transform.position - new Vector3(0, 0.5f, 0);
            //windmill 생성
            GameObject windmill = Instantiate(windmillPrefab, windmillPosition, windmillSpawnPoint.transform.rotation);

            // 플레이어의 위치로 발사할 방향을 계산
            Vector3 playerPosition = new Vector3(pointX, pointY, 0);
            Vector3 directionToPlayer = (playerPosition - windmillPosition).normalized; // 플레이어 방향으로 단위 벡터 계산

            // windmill에 물리적 힘을 추가해 플레이어 방향으로 발사
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

    //보스 공격2
    public void BossAtk2()
    {
        //this.animator.SetTrigger("Atk2Trigger");
        //canAttack2 = false; //공격 직후 공격할 수 없도록 설정
        //StartCoroutine(AttackCooldown2()); //쿨다운 코루틴 시작
        StartCoroutine(EBallWithWarning(1.0f)); //1초 동안 경고 후 EBball 생성
    }
    //3초동안 공격을 할 수 없게 하는 코루틴
    //IEnumerator AttackCooldown2()
    //{
    //    yield return new WaitForSeconds(attackCooldown2);
    //    //canAttack2 = true; //3초후 공격 가능
    //}
    //에너지볼 관련 함수
    //에너지볼 생성
    void EBall() //EBball 
    {
        if (eBallPrefab != null)
        {
            //EBball 생성
            GameObject eBall = Instantiate(eBallPrefab);

            eBall.transform.position = new Vector3(pointX, pointY, 0);
            //nowAttack = false;
            StartCoroutine(HideCooldown()); //쿨다운 코루틴 시작
        }
        else
        {
            Debug.LogError("eBall prefab is not set");
        }
    }
    IEnumerator EBallWithWarning(float warningDuration)
    {
        ShowWarning(); // 경고 신호 함수
        yield return new WaitForSeconds(warningDuration); // 예고 시간동안 대기

        EBall(); // 실제 EBball 함수 호출
    }

    void ShowWarning()
    {
        // 예를 들어 경고 마커를 생성하거나 해당 위치의 색을 변화시키는 등의 로직 구현
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(pointX, pointY, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1초 후에 경고 마커 제거
    }

    public void EndEpisodeWithRewards()
    {
        // 보스 체력이 50% 이상일 경우 추가 보상
        if (bossHp / (float)initialBossHp >= 0.5f)
        {
            AddReward(10f);  // 추가 보상
        }
        // 보스 체력이 적을수록 패널티가 커지도록 설정
        AddReward((1 - (bossHp / (float)initialBossHp)) * (-5f));

        bossStartPosition = transform.position;
        // 에피소드 종료
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 비워두거나 기본값을 설정해 경고 메시지 방지
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f; // 순간이동을 하지 않음
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
