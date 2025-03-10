using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController1 : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float minX = -3f; // X축 최소값
    public float maxX = 3f; // X축 최대값

    private bool movingRight = true; // 방향 전환

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
        // 현재 X 위치가 설정한 범위를 벗어나지 않도록 함
        if (transform.position.x < minX)
        {
            movingRight = true; // 오른쪽으로 전환
        }
        else if (transform.position.x > maxX)
        {
            movingRight = false; // 왼쪽으로 전환
        }

        // 방향에 따라 이동
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}
