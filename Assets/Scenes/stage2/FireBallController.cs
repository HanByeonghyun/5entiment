using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    private float lifetime = 4.0f; // 공격이 맞지 않았을 때 지속시간

    void Start()
    {
        // lifetime 후에 공격을 삭제하는 함수
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            //fire이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);

            Destroy(this.gameObject);
        } 
    }
    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
