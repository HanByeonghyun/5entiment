using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject boss1HP;
    GameObject boss11HP;
    GameObject boss2HP;
    GameObject boss22HP;
    GameObject boss3HP;
    GameObject boss33HP;
    GameObject boss4HP;
    GameObject boss44HP;
    GameObject playerHP;
    GameObject playerMP;

    void Start()
    {
        this.boss1HP = GameObject.Find("Boss1HP");
        this.boss11HP = GameObject.Find("Boss11HP");
        this.boss2HP = GameObject.Find("Boss2HP");
        this.boss22HP = GameObject.Find("Boss22HP");
        this.boss3HP = GameObject.Find("Boss3HP");
        this.boss33HP = GameObject.Find("Boss33HP");
        this.boss4HP = GameObject.Find("Boss4HP");
        this.boss44HP = GameObject.Find("Boss44HP");
        this.playerHP = GameObject.Find("Hpbar");
        this.playerMP = GameObject.Find("Mpbar");
    }

    void Update()
    {
        
    }

    //�������� ü�� ��� �Լ���
    public void DecreaseBoss1Hp(float damage)
    {
        this.boss1HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��ȵ� ����1�� HP�� 0�� ���� ��
        if (this.boss1HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director1 = GameObject.Find("1_Boss");
            director1.GetComponent<Boss1Controller>().BossDeath();
        }
    }
    public void DecreaseBoss11Hp(float damage)
    {
        this.boss11HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��� ����1�� HP�� 0�� ���� ��
        if (this.boss11HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director2 = GameObject.Find("1_Boss");
            director2.GetComponent<BossAgent>().BossDeath();
        }
    }
    public void DecreaseBoss2Hp(float damage)
    {
        this.boss2HP.GetComponent<Image>().fillAmount -= 0.1f;
        // AI�н��ȵ� ����2�� HP�� 0�� ���� ��
        if (this.boss2HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("2_Boss");
            director.GetComponent<Boss2Controller>().BossDeath();
        }
    }
    public void DecreaseBoss22Hp(float damage)
    {
        this.boss22HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��� ����2�� HP�� 0�� ���� ��
        if (this.boss22HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("2_Boss");
            director.GetComponent<BossAgent2>().BossDeath();
        }
    }
    public void DecreaseBoss3Hp(float damage)
    {
        this.boss3HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��ȵ� ����3�� HP�� 0�� ���� ��
        if (this.boss3HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("3_Boss");
            director.GetComponent<Boss3Controller>().BossDeath();
        }
    }
    public void DecreaseBoss33Hp(float damage)
    {
        this.boss33HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��� ����3�� HP�� 0�� ���� ��
        if (this.boss33HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("3_Boss");
            director.GetComponent<BossAgent3>().BossDeath();
        }
    }
    public void DecreaseBoss4Hp(float damage)
    {
        this.boss4HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��ȵ� ����4�� HP�� 0�� ���� ��
        if (this.boss4HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("4_Boss");
            director.GetComponent<Boss4Controller>().BossDeath();
        }
    }
    public void DecreaseBoss44Hp(float damage)
    {
        this.boss44HP.GetComponent<Image>().fillAmount -= damage;
        // AI�н��� ����4�� HP�� 0�� ���� ��
        if (this.boss44HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            GameObject director = GameObject.Find("4_Boss");
            director.GetComponent<BossAgent4>().BossDeath();
        }
    }

    //�÷��̾��� ü���� ��� �Լ�
    public void DecreasePlayerHp(float damage)
    {
        this.playerHP.GetComponent<Image>().fillAmount -= damage;
        GameObject director = GameObject.FindWithTag("player");
        director.GetComponent<PlayerController>().PlayerHit();
        //�÷��̾��� HP�� 0�� ���� ��
        if (this.playerHP.GetComponent<Image>().fillAmount == 0)
        {
            //HP�� �� �޾Ҵٰ� ���� �� BossDeath() �Լ� ����
            director.GetComponent<PlayerController>().PlayerDeath();
        }
    }
    //�÷��̾��� ��ų ��� ���� Ƚ���� ��� �Լ�
    public void DecreasePlayerMp(float damage)
    {
        this.playerMP.GetComponent<Image>().fillAmount -= damage;
    }
}
