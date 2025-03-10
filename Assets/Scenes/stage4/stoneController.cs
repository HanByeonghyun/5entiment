using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneController : MonoBehaviour
{
    private float lifetime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //5�ʰ� ������ ��ֹ� �����ϴ� �Լ� ȣ��
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //�ٴڿ� ��ֹ��� ������ ����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fall2")
        {
            Destroy(this.gameObject);
        }
    }

    //��ֹ� ����
    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
