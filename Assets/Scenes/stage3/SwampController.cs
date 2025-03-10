using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampController : MonoBehaviour
{
    public float damageInterval = 2.0f; //데미지 간격
    private float nextDamgeTime = 0f; //다음 데미지를 줄 시간

    void Start()
    {
    }

    void Update()
    {
        StartCoroutine(DestroyAfterDelay(10.0f));
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "player" && Time.time >= nextDamgeTime)
        {
            //swamp에 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    //10초 후에 오브젝트를 삭제하는 코루틴
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
