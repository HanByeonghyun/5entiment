using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    void Start()
    {

    }
        

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -3)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(0, -0.1f, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //�÷��̾�� �¾��� ��
        if (other.gameObject.tag == "player")
        {
            //fire�� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);

            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Cliff") //�ٴڿ� ����� ��
        {
            Destroy(this.gameObject);
        }
    }
}
