using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class portalController : MonoBehaviour
{
    private Renderer portalRenderer; // ��Ż�� ������ ������Ʈ
    private bool playerInPortal = false; // �÷��̾ ��Ż�� �ִ��� ����
    private float timeInPortal = 0f; // ��Ż�� �ӹ� �ð�
    public float portalStayTime = 2f; // ��Ż�� �ӹ����� �ϴ� �ð�
    public string nextSceneName; // �̵��� �� �̸�

    GameObject playerHP;

    public bool visible = false; //��Ż Ȱ��ȭ ����

    void Start()
    {
        // ��Ż�� Renderer ������Ʈ�� ������ ��Ȱ��ȭ
        portalRenderer = GetComponent<Renderer>();
        portalRenderer.enabled = false; // ó������ ��Ż�� ������ �ʵ��� ����
        this.playerHP = GameObject.Find("Hpbar");
    }

    void Update()
    {
        
        GameObject[] sentiment0 = GameObject.FindGameObjectsWithTag("sentiment0");
        GameObject[] sentiment1 = GameObject.FindGameObjectsWithTag("sentiment1");
        GameObject[] sentiment2 = GameObject.FindGameObjectsWithTag("sentiment2");
        GameObject[] sentiment3 = GameObject.FindGameObjectsWithTag("sentiment3");
        GameObject[] sentiment4 = GameObject.FindGameObjectsWithTag("sentiment4");
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        GameObject[] boss1 = GameObject.FindGameObjectsWithTag("Boss1");
        GameObject[] boss2 = GameObject.FindGameObjectsWithTag("Boss2");
        GameObject[] boss3 = GameObject.FindGameObjectsWithTag("Boss3");
        GameObject[] boss4 = GameObject.FindGameObjectsWithTag("Boss4");

        //���ͳ� ������ ��� ��������� Ȯ��
        // ���Ͱ� ��� ����� ��� ��Ż�� ���̰� ��
        if (sentiment0.Length == 0 && sentiment1.Length == 0 && sentiment2.Length == 0 && sentiment3.Length == 0 
            && sentiment4.Length == 0 && monsters.Length == 0 && boss1.Length == 0 && boss2.Length == 0
            && boss3.Length == 0 && boss4.Length == 0)
        {
            portalRenderer.enabled = true; // ��Ż�� ���̰� ����
            visible = true; //��Ż Ȱ��ȭ ����
        }

        // �÷��̾ ��Ż�� �ְ� ��Ż�� Ȱ��ȭ ���� ��� Ÿ�̸� ����
        if (playerInPortal && visible)
        {
            timeInPortal += Time.deltaTime;

            // 3�� ���� ��Ż ���� ���� ��� ���� ������ �̵�
            if (timeInPortal >= portalStayTime)
            {
                LoadNextScene();
            }
        }
        else
        {
            timeInPortal = 0f; // ��Ż�� ������ Ÿ�̸� �ʱ�ȭ
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��Ż�� �÷��̾ ������ Ÿ�̸� ����
        if (other.CompareTag("player"))
        {
            Debug.Log("Player entered the portal."); // ����� �α� �߰�
            playerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ��Ż���� �÷��̾ ������ Ÿ�̸� �ʱ�ȭ
        if (other.CompareTag("player"))
        {
            Debug.Log("Player exited the portal."); // ����� �α� �߰�
            playerInPortal = false;
            timeInPortal = 0f;
        }
    }

    //���� ���������� �Ѿ�� �Լ�
    private void LoadNextScene()
    {
        // ������ �̸��� ������ �̵�
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            //�÷��̾� ü�� �ʱ�ȭ
            this.playerHP.GetComponent<Image>().fillAmount = 1.0f;
            Debug.Log("Loading next scene: " + nextSceneName); // ����� �α� �߰�
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("���� �� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }
}
