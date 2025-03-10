using UnityEngine;

public class AtkProjectTile : MonoBehaviour
{
    private BossAgent2_1 bossAgent;   // 보스 에이전트를 참조하여 보상을 줄 수 있도록 설정
    private float lifetime = 2.0f; // 바람이 맞지 않았을 때 패널티를 줄 시간 (2초 후)

    public float damageInterval = 3.0f; //데미지 간격
    private float nextDamgeTime = 0f; //다음 데미지를 줄 시간

    private void Start()
    {
        // BossAgent를 찾거나 설정합니다. 이 코드로 씬에 있는 BossAgent 컴포넌트를 참조합니다.
        bossAgent = FindFirstObjectByType<BossAgent2_1>();

        // lifetime 후에 공격이 맞지 않은 것으로 간주하고 삭제하는 함수
        Invoke(nameof(GiveRewardIfMissed), lifetime);
    }

    // 근거리 공격을 맞췄을 때
    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "player") && (Time.time >= nextDamgeTime))
        {
            // PlayerHealth 컴포넌트를 가져와서 체력을 감소시킴
            PlayerProjectTile2 playerHealth = other.GetComponent<PlayerProjectTile2>();
            if (playerHealth != null)
            {
                Debug.Log("player Hit!!");
                playerHealth.TakeDamage();  // 플레이어에게 10의 데미지를 줌
                // 근거리 공격 후 삭제
                Destroy(this.gameObject);
            }
            nextDamgeTime = Time.time + damageInterval;
        }
    }

    private void GiveRewardIfMissed()
    {
        // 공격이 충돌 없이 lifetime이 경과한 경우 보상 지급
        if (bossAgent != null)
        {
            bossAgent.ResetConsecutiveHits();  // 연속 히트 수 초기화
        }

        // 공격 제거
        Destroy(this.gameObject);
    }
}
