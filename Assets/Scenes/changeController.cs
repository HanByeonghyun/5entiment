using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeController : MonoBehaviour
{
    // ����Ʈ�� ���ӽð�
    private float lifetime = 0.2f;

    void Start()
    {
        //������ Ÿ�� 0.2�� �ڿ� ������� �Լ� ȣ��
        Invoke(nameof(DestroyChange), lifetime); 
    }

    void Update()
    {
        
    }
    // ������� �Լ�
    void DestroyChange()
    {
        Destroy(this.gameObject);
    }
}
