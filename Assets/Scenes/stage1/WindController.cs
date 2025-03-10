using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        //일정 범위 밖으로 나가면 사라짐
        if (transform.position.x < -10 || transform.position.x > 10)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //플레이어에게 닿았을 때
        if (other.gameObject.tag == "player")
        {
            //바람이 충돌했다고 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            //삭제
            Destroy(this.gameObject);
        }
    }
}
