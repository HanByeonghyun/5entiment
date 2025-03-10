using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeController : MonoBehaviour
{
    // 이펙트의 지속시간
    private float lifetime = 0.2f;

    void Start()
    {
        //라이프 타임 0.2초 뒤에 사라지는 함수 호출
        Invoke(nameof(DestroyChange), lifetime); 
    }

    void Update()
    {
        
    }
    // 사라지는 함수
    void DestroyChange()
    {
        Destroy(this.gameObject);
    }
}
