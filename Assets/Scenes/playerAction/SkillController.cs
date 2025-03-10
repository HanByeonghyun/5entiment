using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private float lifetime = 1.0f; // 화살이 맞지 않았을 때 지속시간(1초)
    Animator animator;
    Rigidbody2D rigid2D;

    void Start()
    {
        // lifetime 후에 화살이 맞지 않은 것으로 간주하고 삭제하는 함수 호출
        Invoke(nameof(GiveRewardIfMissed), lifetime);
        this.animator = GetComponent<Animator>();
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //화살이 보스1에게 맞았을 때
        if (other.gameObject.tag == "Boss1")
        {
            //맞는 효과
            StartCoroutine(HitDelay());
        }

        //화살이 보스2에게 맞았을 때
        if (other.gameObject.tag == "Boss2")
        {
            //맞는 효과
            StartCoroutine(HitDelay());
        }

        //화살이 보스3에게 맞았을 때
        if (other.gameObject.tag == "Boss3")
        {
            //맞는 효과
            StartCoroutine(HitDelay());
        }

        //화살이 보스4에게 맞았을 때
        if (other.gameObject.tag == "Boss4")
        {
            //맞는 효과
            StartCoroutine(HitDelay());
        }
        //화살이 몬스터에게 맞았을 때
        if (other.gameObject.tag == "monster")
        {
            //맞는 효과
            StartCoroutine(HitDelay());
        }
        if (other.gameObject.tag == "Ground2" || other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator HitDelay()
    {
        rigid2D.velocity = Vector2.zero; // 속도 초기화
        this.animator.SetTrigger("HitTrigger");
        yield return new WaitForSeconds(0.5f);
        // 화살 제거
        Destroy(this.gameObject);
    }
    private void GiveRewardIfMissed()
    {
        // 화살 제거
        Destroy(this.gameObject);
    }
}
