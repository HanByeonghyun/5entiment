using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBallController : MonoBehaviour
{
    public float damageInterval = 1.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    void Start()
    {
        nextDamgeTime = Time.time + damageInterval;
    }
    void Update()
    {
        StartCoroutine(DestroyAfterDelay(1.5f));
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

    //1.5�� �Ŀ� ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
