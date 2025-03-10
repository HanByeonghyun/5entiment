using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라가 플레이어를 쫓아가게 하는 코드
public class CameraController : MonoBehaviour
{
    public GameObject player;

    // 카메라 범위 설정](Inspector에서 조절 가능)
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void Start()
    {
        this.player = GameObject.FindWithTag("player");
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player object with tag 'player' not found.");
            return;
        }

        // 플레이어의 위치
        Vector3 playerPos = this.player.transform.position;
        // 플레이어의 x, y 좌표
        float cameraX = playerPos.x;
        float cameraY = playerPos.y;

        // 카메라의 x, y 좌표를 플레이어의 x, y 좌표로 설정 및 최소, 최대 범위 제한
        float clampedX = Mathf.Clamp(cameraX, minX, maxX);
        float clampedY = Mathf.Clamp(cameraY, minY, maxY);

        // 설정한 x, y좌표를 카메라에 적용
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    // 플레이어가 변신해서 플레이어 오브젝트가 바뀔 때마다 플레이어를
    // 바뀐 오브젝트로 초기화 하는 코드
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}

