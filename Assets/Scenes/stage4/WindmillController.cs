using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillController : MonoBehaviour
{
    private float lifetime = 2.0f; // ������� ���� �ʾ��� �� ����� �ð�

    // Start is called before the first frame update
    void Start()
    {
        //2�� �ڿ� ����� ���� �Լ� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            //fire�� �浹�ߴٰ� ���� ��ũ��Ʈ�� ���� �� GameDirector�� DecreaseHp() �Լ� ����
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);

            Destroy(this.gameObject);
        }
    }
    private void GiveRewardIfMissed()
    {
        // ����� ����
        Destroy(this.gameObject);
    }
}
