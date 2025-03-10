using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;

    float playerPosX;
    float playerPosY;
    GameObject player;

    //공격 쿨타임
    float attackCooldown = 3f;

    //웅덩이 프리팹과 발사 위치
    public GameObject swampPrefab;
    public GameObject warningPrefab;

    //웅덩이 생기는 위치 변수
    public float pointX;
    public float pointY;

    //경고 마커를 저장할 변수
    private GameObject warningMarker;

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
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;

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
        if (transform.position.x > 17.0f || transform.position.x < -9.0f) // x 좌표 범위 예시
        {
            moveDirection *= -1.0f; // 방향 반전
        }
    }

    IEnumerator AutoAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown); // 3초마다 공격

            if (player == null) continue;

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // 플레이어와의 거리에 따라 다른 공격 사용
            if (distanceToPlayer > 3.5f) // 먼 거리에서 공격1
            {

                nowAttack = true;
                BossAtk3();
            }
            else if (distanceToPlayer <= 3.5f) // 가까운 거리에서 공격2
            {
                nowAttack = true;
                BossAtk2();
            }
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
            GameObject director3 = GameObject.Find("GameDirector");
            director3.GetComponent<GameDirector>().DecreaseBoss3Hp(0.05f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director3 = GameObject.Find("GameDirector");
            director3.GetComponent<GameDirector>().DecreaseBoss3Hp(0.05f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.1f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.2f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            StartCoroutine(SlowTime(speed));
            //화살이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseBoss3Hp(0.1f);
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
        //DeathTrigger 발생 후 2.5초 뒤 보스 사라짐
        StartCoroutine(BossDeathWithDelay(2.5f));
    }
    //보스가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator BossDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


    //보스 공격2
    public void BossAtk2()
    {
        this.animator.SetTrigger("Atk2Trigger");
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



    //보스 공격3
    public void BossAtk3()
    {
        this.animator.SetTrigger("Atk3Trigger");
        StartCoroutine(SwampWithWarning(1.0f)); //1초 동안 경고 후 swamp 생성
    }
    //웅덩이 관련 함수
    //웅덩이 생성
    void Swamp() //swamp 
    {
        if (swampPrefab != null)
        {
            //swamp 생성
            GameObject newSwamp = Instantiate(swampPrefab);
            newSwamp.transform.position = new Vector3(pointX, (-2.0f), 0);
            nowAttack = false;
        }
        else
        {
            Debug.LogError("swamp prefab is not set");
        }
    }
    IEnumerator SwampWithWarning(float warningDuration)
    {
        pointX = playerPosX;
        pointY = playerPosY;
        ShowWarning(); // 경고 신호 함수
        yield return new WaitForSeconds(warningDuration); // 예고 시간동안 대기

        Swamp(); // 실제 Swamp 함수 호출
    }

    void ShowWarning()
    {
        // 예를 들어 경고 마커를 생성하거나 해당 위치의 색을 변화시키는 등의 로직 구현
        GameObject warningMarker = Instantiate(warningPrefab, new Vector3(playerPosX, -2, 0), Quaternion.identity);
        Destroy(warningMarker, 1f); // 1초 후에 경고 마커 제거
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
