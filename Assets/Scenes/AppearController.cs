using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearController : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(DestroyAfterDelay(2.0f));
    }

    //2초 후에 오브젝트를 삭제하는 코루틴
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
