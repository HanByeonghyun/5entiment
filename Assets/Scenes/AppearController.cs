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

    //2�� �Ŀ� ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
