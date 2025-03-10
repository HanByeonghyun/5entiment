using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    //보스 움직임 최소, 최대 높이
    float downMax = -2.55f;
    float upMax = 3.5f;
    //보스의 x,y 좌표
    float currentPositionY;
    float currentPositionX;
    //플레이어 x,y 좌표
    float playerPosX;
    float playerPosY;
    //보스 위아래 이동 속도
    public float speed = 4.0f;
    float currentDirection = 1.0f; // 위아래 움직임 방향 (1은 위, -1은 아래)

    GameObject player;
    //보스의 공격 가능 여부
    bool canAttack = true;
    //공격 쿨타임
    float attackCooldown = 3f;

    //바람 프리팹과 발사 위치
    public GameObject windPrefab;
    public Transform windSpawnPoint;
    public float windSpeed = 7.0f;

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    public PlayerController playerController;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        currentPositionY = transform.position.y;

        player = GameObject.FindWithTag("player");

        playerController = GameObject.FindWithTag("player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isDead) return;
        if (nowHit) return;
        if (player == null) return;

        // 플레이어 위치 업데이트
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

        //플레이어가 보스보다 왼쪽에 있으면 왼쪽, 오른쪽에 있으면 오른쪽을 바라봄
        if (playerPosX < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerPosX > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 보스 자동 위아래 이동
        float newPositionY = transform.position.y + (speed * currentDirection * Time.deltaTime);
        newPositionY = Mathf.Clamp(newPositionY, downMax, upMax);

        // 방향 반전
        if (newPositionY <= downMax || newPositionY >= upMax)
        {
            currentDirection *= -1;
        }

        transform.position = new Vector3(transform.position.x, newPositionY, 0);

        // 플레이어의 y좌표가 보스의 y좌표 범위에 들어오면 공격
        if (Mathf.Abs(playerPosY - transform.position.y) < 1.0f && canAttack)
        {
            nowAttack = true;
            BossAtk();
        }
    }

    //보스 공격 당함
    //화살 오브젝트(arrow)와 충돌시 HitTrigger 발생
    void OnTriggerEnter2D(Collider2D other)
    {
        if (nowAttack) return;
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss1Hp(0.1f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f);
        speed = originalSpeed;
    }

    //보스 죽음
    public void BossDeath()
    {
        this.animator.SetTrigger("DeathTrigger");
        isDead = true;
        //DeathTrigger 발생 후 1.5초 뒤 보스 사라짐
        StartCoroutine(BossDeathWithDelay(1.5f));
    }
    //보스가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //보스 공격
    public void BossAtk()
    {
        this.animator.SetTrigger("AtkTrigger");
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
        nowAttack = false;
    }
    void Wind() //바람 발사 함수
    {
        if (windPrefab != null && windSpawnPoint != null)
        {
            //보스의 위치에 바람 생성
            GameObject wind1 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);
            GameObject wind2 = Instantiate(windPrefab, windSpawnPoint.position, windSpawnPoint.rotation);

            //보스가 보는 방향
            float windDirection = transform.localScale.x;
            wind1.transform.localScale = new Vector3(windDirection, 1, 1);
            wind2.transform.localScale = new Vector3(windDirection * (-1), 1, 1);

            //바람에 물리적 힘을 추가해 발사
            Rigidbody2D windRb1 = wind1.GetComponent<Rigidbody2D>();
            Rigidbody2D windRb2 = wind2.GetComponent<Rigidbody2D>();
            if (windRb1 != null && windRb2 != null)
            {
                windRb1.velocity = new Vector2(windDirection * windSpeed, 0);
                windRb2.velocity = new Vector2(windDirection * windSpeed * (-1), 0);
            }
        }
        else
        {
            Debug.LogError("wind prefab or spawn point is not set");
        }
    }

    //플레이어 변신 시 인식할 플레이어 오브젝트 변경
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
