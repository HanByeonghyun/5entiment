using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneController : MonoBehaviour
{
    private float lifetime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //5초가 지나면 장애물 삭제하는 함수 호출
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //바닥에 장애물이 닿으면 삭제
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fall2")
        {
            Destroy(this.gameObject);
        }
    }

    //장애물 삭제
    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
