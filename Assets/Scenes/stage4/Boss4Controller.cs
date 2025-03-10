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

    //보스가 보이는지 확인
    bool visible = true;
    //보스가 보이지 않는 시간
    float invisibleDuration = 1.0f;

    // 프리팹과 발사 위치
    public GameObject eBallPrefab;
    public GameObject windmillPrefab;
    public GameObject windmillSpawnPoint;
    public GameObject warningPrefab;
    //windmill 속도
    public float windmillSpeed = 7.0f;

    //에너지볼 생기는 위치 변수
    public float pointX;
    public float pointY;

    //경고 마커를 저장할 변수
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

        // 주기적인 공격을 위한 코루틴 시작
        StartCoroutine(AutoAttack());
    }

    void Update()
    {
        if (isDead) return;

        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
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
            yield return new WaitForSeconds(3.0f); // 3초마다 공격

            if (player == null) continue;

            nowAttack = true;
            BossHide();
            yield return new WaitForSeconds(2.0f); // 1초마다 공격

            nowAttack = true;
            BossAtk2();
            yield return new WaitForSeconds(0.5f); // 1초마다 공격

            nowAttack = true;
            BossAtk3();
        }
    }

    //보스 공격 당함
    //화살 오브젝트(arrow)와 충돌시 HitTrigger 발생
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (nowAttack) return;
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director4 = GameObject.Find("GameDirector");
            director4.GetComponent<GameDirector>().DecreaseBoss4Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director4 = GameObject.Find("GameDirector");
            director4.GetComponent<GameDirector>().DecreaseBoss4Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss4Hp(0.1f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }

    //보스 죽음
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
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
        this.animator.SetTrigger("HideTrigger");
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
        Debug.Log("Selected spawn index: " + randomIndex + ", Position: " + spawnPositions[randomIndex]);
        transform.position = spawnPositions[randomIndex];

        this.animator.SetTrigger("AppearTrigger");
        visible = true;
        GetComponent<SpriteRenderer>().enabled = true; //보스 보이기
        GetComponent<Collider2D>().enabled = true; //충돌 활성화
        rigid2D.isKinematic = false; //물리 효과 활성화

        nowAttack = false;
    }


    //보스 공격2
    public void BossAtk2()
    {
        pointX = playerPosX;
        pointY = playerPosY;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(EBallWithWarning(1.0f)); //1초 동안 경고 후 EBball 생성
    }
    //에너지볼 관련 함수
    //에너지볼 생성
    void EBall() //EBball 
    {
        if (eBallPrefab != null)
        {
            //EBball 생성
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
        ShowWarning(); // 경고 신호 함수
        yield return new WaitForSeconds(warningDuration); // 예고 시간동안 대기

        EBall(); // 실제 EBball 함수 호출
    }

    void ShowWarning()
    {
        // 예를 들어 경고 마커를 생성하거나 해당 위치의 색을 변화시키는 등의 로직 구현
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(playerPosX, playerPosY, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1초 후에 경고 마커 제거
    }

    //보스 공격3
    public void BossAtk3()
    {
        pointX = playerPosX;
        pointY = playerPosY;
        this.animator.SetTrigger("Atk2Trigger");
        StartCoroutine(WindmillWithDelay(0.5f));
    }
    //공격 모션 windmill 발사
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
