using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    float playerPosX;
    float playerPosY;
    GameObject player;
     
    //공격 쿨타임
    float attackCooldown = 2f;

    //fireBall 프리팹과 발사 위치, 속도
    public GameObject fireBallPrefab;
    public Transform fireBallSpawnPoint;
    public float fireBallSpeed = 10.0f;

    public GameObject atkPrefab;       // 공격 프리팹
    public Transform atkSpawnPoint;    // 공격 생성 위치

    // 보스 이동 관련 변수
    public float speed = 5.0f;
    private float moveDirection = 1.0f; // 1은 오른쪽, -1은 왼쪽

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    public PlayerController playerController;

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
        if (nowHit) return;
        if (nowAttack) return;
        if (isDead) return;
        if (player == null) return;

        // 이동하는 방향에 따라 바라보는 방향 설정
        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 오른쪽을 바라봄
        }
        else if (moveDirection < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // 왼쪽을 바라봄
        }

        // 보스의 좌우 이동
        transform.position += new Vector3(speed * moveDirection * Time.deltaTime, 0, 0);

        // 일정 범위에서 방향을 반전
        if (transform.position.x > 12.0f || transform.position.x < -12.0f) // x 좌표 범위 예시
        {
            moveDirection *= -1.0f; // 방향 반전
        }
    }

    IEnumerator AutoAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown); // 2초마다 공격

            if (player == null) continue;

            nowAttack = true;
            BossAtk1();
            yield return new WaitForSeconds(2.0f); // 1초마다 공격

            nowAttack = true;
            BossAtk2();
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
            GameObject director2 = GameObject.Find("GameDirector");
            director2.GetComponent<GameDirector>().DecreaseBoss2Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director2 = GameObject.Find("GameDirector");
            director2.GetComponent<GameDirector>().DecreaseBoss2Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss2Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss2Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss2Hp(0.1f);
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
        StartCoroutine(BossDeathWithDelay(2.5f));
    }
    //보스가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    
    //보스 공격1
    public void BossAtk1()
    {
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(FireBallWithDelay(0.8f));
    }
    //공격 모션 fireBall 발사
    IEnumerator FireBallWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FireBall();
        nowAttack = false;
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


    //보스 공격2
    public void BossAtk2()
    {
        this.animator.SetTrigger("Atk1Trigger");
        StartCoroutine(AttackWithDelay(0.8f));
    }
    IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
        nowAttack = false;
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

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
