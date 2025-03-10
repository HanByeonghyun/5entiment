using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // ��ų ��� Ƚ��
    public int skill1Count = 0;
    public int skill2Count = 0;
    public int skill3Count = 0;
    public int rainCount = 0;

    // ��ų ������� ����
    public bool canSkill0 = false; 
    public bool canSkill1 = false; 
    public bool canSkill2 = false; 
    public bool canSkill3 = false;
    public bool canSkill4 = false;

    public GameObject player0Prefab;

    public GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("player");

        DontDestroyOnLoad(this.gameObject); // �� ��ȯ �� ����
    }

    //��ų ��Ÿ�� ����(��Ÿ�� ������ �� ��ų ��� Ƚ�� 0���� �ʱ�ȭ)-------------------------------------------
    public IEnumerator Skill1Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime); //��ų ��Ÿ��
        skill1Count = 0; // ��ų ���Ƚ�� �ʱ�ȭ
    }
    public IEnumerator Skill2Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        skill2Count = 0; // ��ų ���Ƚ�� �ʱ�ȭ
    }
    public IEnumerator Skill3Cooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        skill3Count = 0; // ��ų ���Ƚ�� �ʱ�ȭ
    }
    public IEnumerator RainCooltime(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);
        rainCount = 0; // ��ų ���Ƚ�� �ʱ�ȭ
    } // -----------------------------------------------------------------------------------------

    //��ų ���� ȣ��Ǵ� �Լ�--------------------------------------------------------------
    public void Skill1CountUp()
    {
        skill1Count++; //��ų ��� Ƚ�� ����
        //��ų ��� Ƚ�� �ִ� ����
        if (skill1Count == 8)
        {
            //������ Ǯ�� �ٽ� �⺻ ���� ���ư�
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            //��ų ��Ÿ�� ���� �Լ� ȣ��(��ų ��Ÿ��)
            StartCoroutine(Skill1Cooltime(10.0f));
        }
    }
    public void Skill2CountUp()
    {
        skill2Count++;
        if (skill2Count == 5)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(Skill2Cooltime(15.0f));
        }
    }
    public void Skill3CountUp()
    {
        skill3Count++;
        if (skill3Count == 8)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(Skill3Cooltime(10.0f));
        }
    }
    public void RainCountUp()
    {
        rainCount++;
        if (rainCount == 2)
        {
            player.GetComponent<PlayerController>().TransformToPrefab(player0Prefab, null);
            StartCoroutine(RainCooltime(20.0f));
        }
    }
    //------------------------------------------------------------------------------------------

    //�÷��̾� ���� �� ������ ���������� ���� �ʱ�ȭ
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}