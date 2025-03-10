using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire3Controller : MonoBehaviour
{
    public float damageInterval = 2.0f; //������ ����
    private float nextDamgeTime = 0f; //���� �������� �� �ð�

    void Start()
    {
        nextDamgeTime = Time.time + damageInterval; //�ʱ� ���� ������ �ð� ����
    }

    void Update()
    {
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
}
