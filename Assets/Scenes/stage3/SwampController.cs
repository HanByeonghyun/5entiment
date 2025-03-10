using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampController : MonoBehaviour
{
    public float damageInterval = 2.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

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
            //swamp�� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    //10�� �Ŀ� ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
