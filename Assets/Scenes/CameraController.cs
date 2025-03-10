using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� �÷��̾ �Ѿư��� �ϴ� �ڵ�
public class CameraController : MonoBehaviour
{
    public GameObject player;

    // ī�޶� ���� ����](Inspector���� ���� ����)
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

        // �÷��̾��� ��ġ
        Vector3 playerPos = this.player.transform.position;
        // �÷��̾��� x, y ��ǥ
        float cameraX = playerPos.x;
        float cameraY = playerPos.y;

        // ī�޶��� x, y ��ǥ�� �÷��̾��� x, y ��ǥ�� ���� �� �ּ�, �ִ� ���� ����
        float clampedX = Mathf.Clamp(cameraX, minX, maxX);
        float clampedY = Mathf.Clamp(cameraY, minY, maxY);

        // ������ x, y��ǥ�� ī�޶� ����
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    // �÷��̾ �����ؼ� �÷��̾� ������Ʈ�� �ٲ� ������ �÷��̾
    // �ٲ� ������Ʈ�� �ʱ�ȭ �ϴ� �ڵ�
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}

