using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monsController : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float minX = -3f; // Y축 최소값
    public float maxX = 3f; // Y축 최대값

    private bool movingRight = true; // 방향 전환
    private Animator animator;
    private Rigidbody2D rb;

    public float damageInterval = 1.5f; //공격 쿨타임 간격
    private float nextDamgeTime = 0f; //다음 데미지를 줄 시간

    public bool isDead = false;
    public bool nowAttack = false;
    public bool nowHit = false;

    private Image monsHP; // 개별 체력바 참조

    void Start()
    {
        this.animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        nextDamgeTime = Time.time + damageInterval; //초기 다음 데미지 시간 설정

        // 몬스터 프리팹 내 자식 캔버스의 체력바 Image 컴포넌트 찾기
        monsHP = transform.Find("Canvas/monsHP").GetComponent<Image>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        //죽거나 공격중이거나 맞는중일 때 움직임 정지
        if (isDead) return;
        if (nowAttack) return;
        if (nowHit) return;
        // 현재 X 위치가 설정한 범위를 벗어나지 않도록 함
        if (transform.position.x < minX)
        {
            movingRight = true; // 오른쪽으로 전환
        }
        else if (transform.position.x > maxX)
        {
            movingRight = false; // 왼쪽으로 전환
        }

        // 방향에 따라 이동 속도 및 이동 애니메이션 설정
        float direction = movingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        animator.SetBool("WalkBool", true); // 이동 애니메이션 활성화

        // 방향에 따른 스케일 설정
        transform.localScale = new Vector3(direction, 1, 1);
    }

    // 오브젝트에 닿았을 때
    void OnTriggerStay2D(Collider2D other)
    {
        //플레이어와 닿고 쿨타임이 지났을 때
        if (other.gameObject.tag == "player" && Time.time >= nextDamgeTime)
        {
            nowAttack = true;
            rb.velocity = Vector2.zero; // 공격 중 이동 멈춤
            this.animator.SetTrigger("AtkTrigger");

            //GameDirector의 플레이어 체력 감소 함수 호출
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            nextDamgeTime = Time.time + damageInterval;
            StartCoroutine(AtkWithDelay(1.0f));
        }
    }
    IEnumerator AtkWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowAttack = false;
    }

    //무언가와 닿았을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        //플레이어의 공격과 스킬에 맞았을 때
        if (other.gameObject.tag == "arrow")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //몬스터 체력감소 함수 호출
            DecreaseMonsHp(0.25f);
        }
        if (other.gameObject.tag == "rain")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(0.25f);
        }
        if (other.gameObject.tag == "skill1")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(0.5f);
        }
        if (other.gameObject.tag == "skill2")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            DecreaseMonsHp(1.0f);
        }
        if (other.gameObject.tag == "skill3")
        {
            nowHit = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            this.animator.SetTrigger("HitTrigger");
            StartCoroutine(HitWithDelay(0.3f));
            //강화공격3에 맞으면 느려지는 함수 호출
            StartCoroutine(SlowTime(speed));
            DecreaseMonsHp(0.5f);
        }
    }
    IEnumerator HitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nowHit = false;
    }

    //속도 느려지는 함수
    IEnumerator SlowTime(float originalSpeed)
    {
        speed = originalSpeed / 3.0f;
        yield return new WaitForSeconds(2.0f); //2초뒤 속도 정상화
        speed = originalSpeed;
    }

    //체력감소 함수
    public void DecreaseMonsHp(float damage)
    {
        if (monsHP != null)
        {
            monsHP.fillAmount -= damage;
            // 몬스터의 HP가 0이 됐을 때
            if (monsHP.fillAmount <= 0)
            {
                MonsDeath();
            }
        }
    }

    //몬스터 사망 함수
    public void MonsDeath()
    {
        if (!isDead)
        {
            isDead = true;
            rb.velocity = Vector2.zero; // 이동을 멈춤
            GetComponent<Collider2D>().enabled = false; //충돌 비활성화
            rb.isKinematic = true; //물리 효과 비활성화
            this.animator.SetTrigger("DeathTrigger");
            StartCoroutine(MonsDeathWithDelay(1.5f));
        }
    }//몬스터가 죽는 애니메이션 발생 후 1.5초 후 사라지게 딜레이를 주기 위함
    IEnumerator MonsDeathWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //오브젝트 삭제
        Destroy(this.gameObject);
    }
}
