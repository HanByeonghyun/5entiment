using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillController : MonoBehaviour
{
    private float lifetime = 2.0f; // 윈드밀이 맞지 않았을 때 사라질 시간

    // Start is called before the first frame update
    void Start()
    {
        //2초 뒤에 윈드밀 삭제 함수 호출
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
        // 윈드밀 제거
        Destroy(this.gameObject);
    }
}
