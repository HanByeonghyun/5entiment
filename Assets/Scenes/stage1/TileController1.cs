using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController1 : MonoBehaviour
{
    public float speed = 2f; // �̵� �ӵ�
    public float minX = -3f; // X�� �ּҰ�
    public float maxX = 3f; // X�� �ִ밪

    private bool movingRight = true; // ���� ��ȯ

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // ���� X ��ġ�� ������ ������ ����� �ʵ��� ��
        if (transform.position.x < minX)
        {
            movingRight = true; // ���������� ��ȯ
        }
        else if (transform.position.x > maxX)
        {
            movingRight = false; // �������� ��ȯ
        }

        // ���⿡ ���� �̵�
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}
