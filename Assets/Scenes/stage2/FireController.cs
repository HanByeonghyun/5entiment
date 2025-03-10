using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    void Start()
    {

    }
        

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -3)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(0, -0.1f, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //플레이어에게 맞았을 때
        if (other.gameObject.tag == "player")
        {
            //fire이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);

            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Cliff") //바닥에 닿았을 때
        {
            Destroy(this.gameObject);
        }
    }
}
