using UnityEngine;

//주석은 보스3를 학습할 때 추가해서 사용한 코드
public class AtkProjectTile3 : MonoBehaviour
{
    //private BossAgent3 bossAgent;   // 보스 에이전트를 참조하여 보상을 줄 수 있도록 설정
    private float lifetime = 2.0f; // 바람이 맞지 않았을 때 패널티를 줄 시간 (2초 후)

    public float damageInterval = 3.0f; //데미지 간격
    private float nextDamgeTime = 0f; //다음 데미지를 줄 시간

    private void Start()
    {
        // BossAgent를 찾거나 설정합니다. 이 코드로 씬에 있는 BossAgent 컴포넌트를 참조합니다.
        //bossAgent = FindFirstObjectByType<BossAgent3>();

        // lifetime 후에 화살이 맞지 않은 것으로 간주하고 보상을 주는 메서드 호출
        Invoke(nameof(GiveRewardIfMissed), lifetime);

        //nextDamgeTime = Time.time + damageInterval; //초기 다음 데미지 시간 설정
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            // PlayerHealth 컴포넌트를 가져와서 체력을 감소시킴
            //PlayerProjectTile3 playerHealth = other.GetComponent<PlayerProjectTile3>();

            //Debug.Log("player Hit!!");
            //playerHealth.TakeDamage();  // 플레이어에게 10의 데미지를 줌
            //fire이 충돌했다고 감독 스크립트에 전달 후 GameDirector의 DecreaseHp() 함수 실행

            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreasePlayerHp(0.1f);
            // Atk 프로젝트 타격 후 삭제
            Destroy(this.gameObject);

            // 보상 지급
            //if (bossAgent != null)
            //{
            //    bossAgent.AddReward(15.0f + (5.0f * bossAgent.consecutiveHits));  // 기본 보상 + 연속 히트 보상
            //    bossAgent.IncreaseConsecutiveHits();  // 연속 히트 수 증가
            //}
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        Destroy(this.gameObject);
    }
}
