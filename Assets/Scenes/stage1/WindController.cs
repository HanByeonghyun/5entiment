using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        //���� ���� ������ ������ �����
        if (transform.position.x < -10 || transform.position.x > 10)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //�÷��̾�� ����� ��
        if (other.gameObject.tag == "player")
        {
            //�ٶ��� �浹�ߴٰ� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            //����
            Destroy(this.gameObject);
        }
    }
}
