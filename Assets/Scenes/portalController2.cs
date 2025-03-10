using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class portalController2 : MonoBehaviour
{
    private Renderer portalRenderer; // ��Ż�� ������ ������Ʈ
    private bool playerInPortal = false; // �÷��̾ ��Ż�� �ִ��� ����
    private float timeInPortal = 0f; // ��Ż�� �ӹ� �ð�
    public float portalStayTime = 2f; // ��Ż�� �ӹ����� �ϴ� �ð�
    public string nextSceneName; // �̵��� �� �̸�

    GameObject playerHP;

    public bool visible = false;

    public PlayerStateManager playerState;

    void Start()
    {
        // ��Ż�� Renderer ������Ʈ�� ������ ��Ȱ��ȭ
        portalRenderer = GetComponent<Renderer>();
        portalRenderer.enabled = false; // ó������ ��Ż�� ������ �ʵ��� ����
        this.playerHP = GameObject.Find("Hpbar");
        playerState = FindAnyObjectByType<PlayerStateManager>();
    }

    void Update()
    {
        //�÷��̾ ���������� �Ϸ��� ���ɰ� ��ų�� ����� ��
        if (playerState.canSkill1)
        {
            portalRenderer.enabled = true; // ��Ż�� ���̰� ����
            visible = true; //��Ż Ȱ��ȭ ����
        }

        // �÷��̾ ��Ż�� ���� ��� Ÿ�̸� ����
        if (playerInPortal && visible)
        {
            timeInPortal += Time.deltaTime;

            // 2�� ���� ��Ż ���� ���� ��� ���� ������ �̵�
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
