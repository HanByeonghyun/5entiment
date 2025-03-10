using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 
public class SentimentController : MonoBehaviour
{
    private Renderer portalRenderer; // ������ ������ ������Ʈ
    private Collider2D portalCollider;
    public bool visible = false;     // ������ ���̴��� ����

    void Start()
    {
        // ��Ż�� Renderer, Collider2D ������Ʈ�� ������ ��Ȱ��ȭ
        portalRenderer = GetComponent<Renderer>();
        portalCollider = GetComponent<Collider2D>();

        portalRenderer.enabled = false; // ó������ ������ ������ �ʵ��� ����
        if (portalCollider != null)
        {
            portalCollider.enabled = false; // ó������ Collider(�ε����� �ϴ� ȿ��)�� ��Ȱ��ȭ
        }
    }

    void Update()
    {
        //���ͳ� ������ ��� ��������� Ȯ��
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        //���Ϳ� ������ ���� ��
        if (monsters.Length == 0 && boss1.Length == 0 && boss2.Length == 0
            && boss3.Length == 0 && boss4.Length == 0)
        {
            portalRenderer.enabled = true; // ������ ���̰� ����
            visible = true;
            if (portalCollider != null)
            {
                portalCollider.enabled = true; // Collider Ȱ��ȭ
            }
        } else
        {
            visible = false;
            if (portalCollider != null)
            {
                portalCollider.enabled = false; // Collider ��Ȱ��ȭ
            }
        }
    }
}
