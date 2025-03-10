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

    //보스들의 체력 깎는 함수들
    public void DecreaseBoss1Hp(float damage)
    {
        this.boss1HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습안된 보스1의 HP가 0이 됐을 때
        if (this.boss1HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director1 = GameObject.Find("1_Boss");
            director1.GetComponent<Boss1Controller>().BossDeath();
        }
    }
    public void DecreaseBoss11Hp(float damage)
    {
        this.boss11HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습된 보스1의 HP가 0이 됐을 때
        if (this.boss11HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director2 = GameObject.Find("1_Boss");
            director2.GetComponent<BossAgent>().BossDeath();
        }
    }
    public void DecreaseBoss2Hp(float damage)
    {
        this.boss2HP.GetComponent<Image>().fillAmount -= 0.1f;
        // AI학습안된 보스2의 HP가 0이 됐을 때
        if (this.boss2HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("2_Boss");
            director.GetComponent<Boss2Controller>().BossDeath();
        }
    }
    public void DecreaseBoss22Hp(float damage)
    {
        this.boss22HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습된 보스2의 HP가 0이 됐을 때
        if (this.boss22HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("2_Boss");
            director.GetComponent<BossAgent2>().BossDeath();
        }
    }
    public void DecreaseBoss3Hp(float damage)
    {
        this.boss3HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습안된 보스3의 HP가 0이 됐을 때
        if (this.boss3HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("3_Boss");
            director.GetComponent<Boss3Controller>().BossDeath();
        }
    }
    public void DecreaseBoss33Hp(float damage)
    {
        this.boss33HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습된 보스3의 HP가 0이 됐을 때
        if (this.boss33HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("3_Boss");
            director.GetComponent<BossAgent3>().BossDeath();
        }
    }
    public void DecreaseBoss4Hp(float damage)
    {
        this.boss4HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습안된 보스4의 HP가 0이 됐을 때
        if (this.boss4HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("4_Boss");
            director.GetComponent<Boss4Controller>().BossDeath();
        }
    }
    public void DecreaseBoss44Hp(float damage)
    {
        this.boss44HP.GetComponent<Image>().fillAmount -= damage;
        // AI학습된 보스4의 HP가 0이 됐을 때
        if (this.boss44HP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            GameObject director = GameObject.Find("4_Boss");
            director.GetComponent<BossAgent4>().BossDeath();
        }
    }

    //플레이어의 체력을 깎는 함수
    public void DecreasePlayerHp(float damage)
    {
        this.playerHP.GetComponent<Image>().fillAmount -= damage;
        GameObject director = GameObject.FindWithTag("player");
        director.GetComponent<PlayerController>().PlayerHit();
        //플레이어의 HP가 0이 됐을 때
        if (this.playerHP.GetComponent<Image>().fillAmount == 0)
        {
            //HP가 다 달았다고 전달 후 BossDeath() 함수 실행
            director.GetComponent<PlayerController>().PlayerDeath();
        }
    }
    //플레이어의 스킬 사용 가능 횟수를 깎는 함수
    public void DecreasePlayerMp(float damage)
    {
        this.playerMP.GetComponent<Image>().fillAmount -= damage;
    }
}
