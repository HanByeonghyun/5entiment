using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private float lifetime = 1.0f; // 화살이 맞지 않았을 때 지속 시간(1초)

    void Start()
    {
        // lifetime 후에 화살이 맞지 않은 것으로 간주하고 삭제하는 함수
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //화살이 보스1에게 맞았을 때
        if (other.gameObject.tag == "Boss1")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }

        //화살이 보스2에게 맞았을 때
        if (other.gameObject.tag == "Boss2")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }

        //화살이 보스3에게 맞았을 때
        if (other.gameObject.tag == "Boss3")
        {
            //화살 삭제
            Destroy(this.gameObject);
        }

        //화살이 보스4에게 맞았을 때
        if (other.gameObject.tag == "Boss4")
        {
            // 화살 제거
            Destroy(this.gameObject);
        }
        //화살이 몬스터에게 맞았을 때
        if (other.gameObject.tag == "monster")
        {
            // 화살 제거
            Destroy(this.gameObject);
        }
        //화살이 벽에 맞았을 때
        if (other.gameObject.tag == "Ground2")
        {
            Destroy(this.gameObject);
        }
    }
    private void GiveRewardIfMissed()
    {
        // 화살 제거
        Destroy(this.gameObject);
    }
}
